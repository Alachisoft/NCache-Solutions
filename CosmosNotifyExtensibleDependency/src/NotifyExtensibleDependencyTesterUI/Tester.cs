// Copyright (c) 2020 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Alachisoft.NCache.Client;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    internal class Tester
    {
        // This test shows data in the cache is removed when the corresponding database contents are changed
        internal static void Test1()
        {
            Console.WriteLine("Conducting test1\n");
            Initialize();
            UpdateTest();
        }

        // This test shows that in case of node removal from a running cache, the re-distributed data in
        // partitioned and/or por topologies is removed when the corresponding database contents are changed,
        // demonstrating the persistance of state during data transfer between surviving nodes
        internal static void Test2()
        {
            Console.WriteLine("Conducting test2\n");
            Initialize();

            Console.WriteLine("Stop one node in cache and then press any key");
            Console.ReadKey();
            UpdateTest();
        }

        // Initialize method called in Test1 and Test2 to initialize the data, the cache handle and the database client
        private static void Initialize()
        {
            // In case the resources aren't already initialized in a previous test
            if (!_initialized)
            {
                Console.WriteLine($"Initializing data");
                InitializeData();
                Console.WriteLine($"Initializing database and seeding data");
                InitializeDatabase();
                Console.WriteLine($"Initializing cache");
                InitializeCache();

                _initialized = true;
            }

            Console.WriteLine($"Inserting {_customerCount} items in {_cacheId}");
            InsertItemsInCacheWithDependency();
            Console.WriteLine($"Number of items in cache before update: {_cache.Count}");
        }

        // UpdateTest is called in Test1 and Test2 to demonstrate the working of CosmosDbNotificationDependency on the server side
        private static void UpdateTest()
        {
            UpdateDatabase();
            Thread.Sleep(10000);
            Console.WriteLine($"Number of items in {_cacheId} after update: {_cache.Count}");
        }


        // Simulates a large number of customer instances to be added to cache and database
        private static void InitializeData()
        {
            int j = 0;
            _customers = new Bogus.Faker<Customer>()
                .RuleFor(i => i.Id, (fake) =>  $"{j++}")
                .RuleFor(i => i.ContactName, (fake) => fake.Person.FullName)
                .RuleFor(i => i.Address, (fake) => fake.Address.StreetAddress())
                .RuleFor(i => i.City, (fake) => fake.Address.City())
                .RuleFor(i => i.Country, (fake) => fake.Address.Country())
                .RuleFor(i => i.CompanyName, (fake) => fake.Company.CompanyName())
                .Generate(_customerCount);
        }


        // We start the Cosmos Db SQL API client and, if necessary, create the database as well as the monitored and leases 
        // collections
        private static void InitializeDatabase()
        {
            _client = new DocumentClient(
                serviceEndpoint: new Uri(_endPoint),
                authKeyOrResourceToken: _authKey,
                handler: _cosmosDbHandler,
                connectionPolicy: _cosmosDBConnectionPolicy);

            Database database = _client.CreateDatabaseIfNotExistsAsync(
                new Database { Id = _databaseName }).GetAwaiter().GetResult().Resource;

            DocumentCollection monitoredCollection = new DocumentCollection
            {
                Id = _monitoredCollection
            };

            monitoredCollection.PartitionKey.Paths.Add("/id");

            DocumentCollection leaseCollection = new DocumentCollection
            {
                Id = _leaseCollection
            };

            leaseCollection.PartitionKey.Paths.Add("/id");


            monitoredCollection = _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), monitoredCollection, new RequestOptions { OfferThroughput = 10000 }).GetAwaiter().GetResult().Resource;

            leaseCollection = _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), leaseCollection, new RequestOptions { OfferThroughput = 10000 }).GetAwaiter().GetResult().Resource;

            List<Task> tasks = new List<Task>();

            foreach (var customer in _customers)
            {
                tasks.Add(_client.UpsertDocumentAsync(monitoredCollection.DocumentsLink, customer, new RequestOptions { PartitionKey = new PartitionKey(customer.Id) }, disableAutomaticIdGeneration: true));
            }

            Task.WhenAll(tasks).Wait();
        }


        // Initialize the cache using the cache Id and the parameters given in the client.ncconf file given in the project folder
        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache(_cacheId);

        }

        // We insert the created list of customers to the cache together with the CosmosDbNotificationDependency instances as 
        // dependencies. The insertion is done in bulk to save on network latency times
        private static void InsertItemsInCacheWithDependency()
        {
            _cache.Clear();
            Dictionary<string, CacheItem> insertItems = new Dictionary<string, CacheItem>();
            foreach (var customer in _customers)
            {
                CacheItem item = new CacheItem(customer);
                item.Dependency = new CosmosDbNotificationDependency(
                    customer.Id,
                    _cacheId,
                    _endPoint,
                    _authKey,
                    _databaseName,
                    _monitoredCollection,
                    _endPoint,
                    _authKey,
                    _databaseName,
                    _leaseCollection);

                insertItems.Add(customer.Id, item);
            };

            _cache.InsertBulk(insertItems);
        }

        // We upsert updated customers to the database to demonstrate the CosmosDbNotificationDependency logic is working
        private static void UpdateDatabase()
        {
            List<Task> tasks = new List<Task>();

            foreach (var customer in _customers)
            {
                customer.Address += 1;
                Task t = _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _monitoredCollection, customer.Id), customer, new RequestOptions { PartitionKey = new PartitionKey(customer.Id) });
                tasks.Add(t);
            }

            Task.WhenAll(tasks).Wait();
        }


        // The cache handle we will use to communicate with the clustered cache
        private static ICache _cache;

        // The Cosmos Db SQL API client used to add data in the database
        private static DocumentClient _client = null;

        // The endpoint URI of Azure Cosmos DB SQL API
        private static readonly string _endPoint = ConfigurationManager.AppSettings["EndPoint"];

        // The authorization key of Azure Cosmos DB SQL API
        private static readonly string _authKey = ConfigurationManager.AppSettings["AuthKey"];

        // The id of the monitored collection
        private static readonly string _monitoredCollection = ConfigurationManager.AppSettings["MonitoredCollection"];

        // The id of the lease collection where the change feed metadata is kept
        private static readonly string _leaseCollection = ConfigurationManager.AppSettings["LeaseCollection"];

        // The id of the database where the monitored collection and lease collection reside
        private static readonly string _databaseName = ConfigurationManager.AppSettings["DatabaseName"];

        // The number of customers we are going to add to the cache and database to test 
        // CosmosDbNotificationDependency
        private static readonly int _customerCount = int.Parse(ConfigurationManager.AppSettings["InstanceCount"]);

        // The id of the clustered cache. The value in the appSettings section should have a corresponding entry in the 
        // client.ncconf value given in the project folder. The default given is 'pora'
        private static string _cacheId = ConfigurationManager.AppSettings["CacheID"];

        // The list of custom instances to add to the database and cache
        private static List<Customer> _customers;

        // We disable certificate validation for demonstration purposes only and should NOT be used in production environments
        private static readonly HttpClientHandler _cosmosDbHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (a, b, c, d) => true
        };

        // The connection policy attribute when initializing the Azure Cosmos Db SQL client
        private static readonly ConnectionPolicy _cosmosDBConnectionPolicy = new ConnectionPolicy
        {
            EnableEndpointDiscovery = false
        };

        // In case Test1 and Test2 are run one after the other in the same application run, the _initialized field
        // makes sure we don't waste time re-initializing the customer list, database and cache
        private static bool _initialized = false;
    }
}
