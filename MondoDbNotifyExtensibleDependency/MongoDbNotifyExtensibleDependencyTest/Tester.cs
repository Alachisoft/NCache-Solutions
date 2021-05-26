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
using Alachisoft.NCache.Runtime.Dependencies;
using Microsoft.Extensions.Configuration;
using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace MongoDbNotifyExtensibleDependencyTest
{
    public static class Tester
    {
        private static string _conString;                           // MongoDB connection string
        private static string _databaseName;                        // MongoDB dtabase name
        private static string _collectionName;                      // MongoDB collection name to be watched
        private static string _cacheName;                           // Name of cache to be synced with MongoDB
        private static string _providerName;                        // Name of Notify Extensible Dependency provider deployed on cache

        private static MongoClient _client;                         // MongoDB client instance
        private static IMongoDatabase _db;                          // MongoDB database instance
        private static IMongoCollection<Customer> _coll;            // MongoDB collection instance
        private static ICache _cache;                               // Cache instance
        private static List<Customer> _customers;                   // List of customers to be added in database and cache

        private static int _customerCount;                          // Number of items to be added in database and cache          

        // This test shows data in the cache is removed when the corresponding database contents are changed
        public static void Run()
        {
            Console.WriteLine("Initializing tester...");
            Initialize();

            Console.WriteLine("Initializing database...");
            InitializeDatabase();

            Console.WriteLine("Initializing cache...");
            InitializeCache();

            Console.WriteLine("Initializing data...");
            InitializeData();

            Console.WriteLine($"Inserting {_customerCount} customers in database...");
            InsertDataInDb();

            Console.WriteLine($"Inserting {_customerCount} customers with dependency in cache...");
            InsertDataInCache();

            Console.WriteLine("Updating data in database...");
            UpdateDataInDb();

            Console.WriteLine("Verifying results...");
            VerifyResults();
        }

        // This method initializes input parameters required to initialize the cache handle and the database client
        public static void Initialize()
        {
            _conString = ConfigurationManager.AppSettings["ConnectionString"];
            _databaseName = ConfigurationManager.AppSettings["Database"];
            _collectionName = ConfigurationManager.AppSettings["Collection"];
            _cacheName = ConfigurationManager.AppSettings["CacheName"];
            _providerName = ConfigurationManager.AppSettings["ProviderName"];
            _customerCount = Int32.Parse(ConfigurationManager.AppSettings["InstanceCount"]);
        }

        // This method initializes the database client
        private static void InitializeDatabase()
        {
            _client = new MongoClient(_conString);
            _db = _client.GetDatabase(_databaseName);
            _coll = _db.GetCollection<Customer>(_collectionName);
        }

        // This method initializes the cache handle
        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache(_cacheName);
            _cache.Clear();
        }

        // This method inserts data in MongoDB database
        private static void InsertDataInDb()
        {
            _coll.InsertMany(_customers);
        }

        // This method inserts data in cache with dependency on items in database
        private static void InsertDataInCache()
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ConString", _conString);
            parameters.Add("DatabaseName", _databaseName);
            parameters.Add("CollectionName", _collectionName);

            foreach (var customer in _customers)
            {
                CacheItem item = new CacheItem(customer);
                item.Dependency = new CustomDependency(_providerName, parameters);

                items.Add("Customer:" + customer.CustomerId, item);
            }

            _cache.InsertBulk(items);
        }

        // This method updates data in MongoDB database to trigger the dependency for items present in cache
        private static void UpdateDataInDb()
        {
            _coll.UpdateMany(c => c.CustomerId != "", "{$set: {'Address':'Updated Address'}}");
        }

        // This method verifies if the dependency triggered and all dependent items were removed from cache
        private static void VerifyResults()
        {
            int count = 0;
            while (_cache.Count > 0)
            {
                if (count >= 30)
                {
                    Console.WriteLine($"Failed to remove {_cache.Count} dependent items from cache.");
                    break;
                }
                Thread.Sleep(500);

                Console.Write("-");
                count++;
            }

            if (count < 30)
            {
                Console.WriteLine("");
                Console.WriteLine("Test successful. All dependent items were removed from cache.");
            }
        }

        // This method generates random customers and returns them in the form of a list
        public static void InitializeData()
        {
            int j = 0;
            _customers = new Bogus.Faker<Customer>()
                .RuleFor(i => i.CustomerId, (fake) => $"{j++}")
                .RuleFor(i => i.ContactName, (fake) => fake.Person.FullName)
                .RuleFor(i => i.Address, (fake) => fake.Address.StreetAddress())
                .RuleFor(i => i.City, (fake) => fake.Address.City())
                .RuleFor(i => i.Country, (fake) => fake.Address.Country())
                .RuleFor(i => i.CompanyName, (fake) => fake.Company.CompanyName())
                .Generate(_customerCount);
        }
    }
}
