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

using Alachisoft.NCache.Runtime.Dependencies;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.ChangeFeedProcessor.PartitionManagement;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Alachisoft.NCache.Samples
{
    // The CosmosDbNotificationDependency class implements NotifyExtensibleDependency and provides
    // real-time synchronization with a Cosmos DB SQL collection using the Change Feed Processor (CFP)
    // library
    [Serializable]
    public class CosmosDbNotificationDependency :
        NotifyExtensibleDependency, IEquatable<CosmosDbNotificationDependency>
    {
        // The key of the item in the database against which we are setting dependency
        private readonly string _cacheKey;
        // Unique id for the dependency instance
        private readonly string _dependencyId;
        // The change feed processor key against which we will be saving an instance of IChangeFeedProcessor
        private readonly string _changeFeedProcessorKey;
        // Lock to synchronize registration and removal of dependency instance from dependency tracking data structures
        private static readonly object lock_mutex =
            new object();

        // dependencyOnProcessors is a static dictionary that maps information on a 
        // change feed processor instance to the set of dependencies IDs. Everytime a 
        // dependency is invoked, the dependency count associated with the change 
        // feed processor is decremented. Once the number of dependencies for a given 
        // change feed processors is 0, we can stop the change feed processor.
        private static readonly Dictionary<string, IChangeFeedProcessor> changefeedProcessors =
            new Dictionary<string, IChangeFeedProcessor>();

        // changefeedProcessors is a static dictionary that maps a key to a 
        // IChangeFeedProcessorInstance and the same key is used in the 
        // dependencyOnProcessors mapping. Once the number of dependency Ids 
        // associated with a key reaches 0, the same key can be used to access the 
        // IChangeFeedProcessor instance in the changefeedProcessors mapping and 
        // stop it.
        private static readonly Dictionary<string, HashSet<string>> dependenciesOnProcessors =
            new Dictionary<string, HashSet<string>>();

        // dependencyOnKeys is a static dictionary that maps information on a 
        // collection key to the set of dependencies that rely on it as a single  
        // database document can have multiple dependent cached items
        internal static readonly Dictionary<string, HashSet<CosmosDbNotificationDependency>> dependenciesOnKeys =
            new Dictionary<string, HashSet<CosmosDbNotificationDependency>>();

        // Equality comparer used with the dependencyOnKeys mapped set of dependencies
        private static readonly CosmosDbNotificationDependencyEqualityComparer comparer =
            new CosmosDbNotificationDependencyEqualityComparer();

        public CosmosDbNotificationDependency(
            string cacheKey,              // The _id value of the document against which the dependency is set
            string cacheId,               // The cache Id of the cache
            string monitoredUriString,    // The Cosmos DB connection string where the monitored collection is deployed
            string monitoredAuthKey,      // The Comos DB authorization key where the monitored collection is deployed
            string monitoredDatabaseId,   // The Id of the database the monitored collection is included in
            string monitoredContainerId,  // The Id of the monitored collection
            string leaseUriString,        // The Cosmos DB connection string where the lease collection is deployed
            string leaseAuthKey,          // The Comos DB authorization key where the lease collection is deployed
            string leaseDatabaseId,       // The Id of the database the lease collection is included in
            string leaseContainerId       // The Id of the monitored collection
            )
        {
            // Validate all the arguments
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                throw new ArgumentNullException(nameof(cacheKey));
            }
            if (string.IsNullOrWhiteSpace(cacheId))
            {
                throw new ArgumentNullException(nameof(cacheId));
            }
            if (string.IsNullOrWhiteSpace(monitoredUriString))
            {
                throw new ArgumentNullException(nameof(monitoredUriString));
            }
            if (string.IsNullOrWhiteSpace(monitoredAuthKey))
            {
                throw new ArgumentNullException(nameof(monitoredAuthKey));
            }

            if (string.IsNullOrWhiteSpace(monitoredDatabaseId))
            {
                throw new ArgumentNullException(nameof(monitoredDatabaseId));
            }
            if (string.IsNullOrWhiteSpace(monitoredContainerId))
            {
                throw new ArgumentNullException(nameof(monitoredContainerId));
            }
            if (string.IsNullOrWhiteSpace(leaseUriString))
            {
                throw new ArgumentNullException(nameof(leaseUriString));
            }
            if (string.IsNullOrWhiteSpace(leaseAuthKey))
            {
                throw new ArgumentNullException(nameof(leaseAuthKey));
            }

            if (string.IsNullOrWhiteSpace(leaseDatabaseId))
            {
                throw new ArgumentNullException(nameof(leaseDatabaseId));
            }
            if (string.IsNullOrWhiteSpace(leaseContainerId))
            {
                throw new ArgumentNullException(nameof(leaseContainerId));
            }

            _dependencyId = $"cosmosDependency-{Guid.NewGuid().ToString()}";

            _cacheKey = cacheKey;

            _changeFeedProcessorKey = $"{monitoredUriString}-{monitoredAuthKey}-{monitoredDatabaseId}-{monitoredContainerId}-{leaseUriString}-{leaseAuthKey}-{leaseDatabaseId}-{leaseContainerId}-{cacheId}";
        }

        // Overall unique identifier used to track dependency against change feed 
        // processor instances on which the dependency is reliant
        public string DependencyId
        {
            get
            {
                return _dependencyId + _changeFeedProcessorKey + _cacheKey;
            }
        }

        // After the constructor is exited, the NCache handler will call **Initialize** to set up the required resources
        // and start monitoring for changes in the monitored collection
        public override bool Initialize()
        {
            var dependencyKey = $"{_changeFeedProcessorKey} {_cacheKey}";

            RegisterDependency(_changeFeedProcessorKey, dependencyKey, this);

            return true;
        }

        // In case the DependencyChanged event occurs, the **DependencyDispose** method is called to clean up 
        // any resources. **DependencyDispose** is also called in case of explicit deletion of item due to 
        // expiration, eviction or a remove command from the user
        protected override void DependencyDispose()
        {
            var dependencyKey = $"{_changeFeedProcessorKey} {_cacheKey}";
            UnregisterDependency(_changeFeedProcessorKey, dependencyKey, this);
        }

        // Implementation of the IEquatable interface. 
        public bool Equals(CosmosDbNotificationDependency other)
        {
            if (other == null)
            {
                return false;
            }

            return _dependencyId.Equals(other._dependencyId, StringComparison.Ordinal) &&
                    _changeFeedProcessorKey.Equals(other._changeFeedProcessorKey, StringComparison.Ordinal) &&
                    _cacheKey.Equals(other._cacheKey, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CosmosDbNotificationDependency other = obj as CosmosDbNotificationDependency;

            if (other == null)
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            string hashString = _changeFeedProcessorKey + _cacheKey + _dependencyId;
            return hashString.GetHashCode();
        }


        // **RegisterDependency** registers the dependency with the static data structures so as to be able to track
        // when a non-required change feed processors can be stopped
        private static void RegisterDependency(
            string changeFeedProcessorKey,
            string dependencyKey,
            CosmosDbNotificationDependency dependency)
        {
            lock (lock_mutex)
            {
                if (!dependenciesOnKeys.ContainsKey(dependencyKey))
                {
                    dependenciesOnKeys.Add(dependencyKey, new HashSet<CosmosDbNotificationDependency>(comparer));

                    if (!dependenciesOnProcessors.ContainsKey(changeFeedProcessorKey))
                    {
                        dependenciesOnProcessors.Add(changeFeedProcessorKey, new HashSet<string>(StringComparer.Ordinal));

                        changefeedProcessors.Add(
                            changeFeedProcessorKey, CreateChangeFeedProcessor(changeFeedProcessorKey));

                        changefeedProcessors[changeFeedProcessorKey].StartAsync().Wait();
                    }
                }

                dependenciesOnKeys[dependencyKey].Add(dependency);
                dependenciesOnProcessors[changeFeedProcessorKey].Add(dependency.DependencyId);

            }
        }

        // In case the **DependencyDispose** method is called, the dependency is removed from list of registered dependencies
        // and any change feed processors that don't have any dependencies relying on them will be stopped
        private static void UnregisterDependency(
            string changeFeedProcessorKey,
            string dependencyKey,
            CosmosDbNotificationDependency dependency)
        {
            lock (lock_mutex)
            {
                dependenciesOnKeys[dependencyKey].Remove(dependency);
                dependenciesOnProcessors[changeFeedProcessorKey].Remove(dependency.DependencyId);

                if (dependenciesOnKeys[dependencyKey].Count == 0)
                {
                    dependenciesOnKeys.Remove(dependencyKey);

                    if (dependenciesOnProcessors[changeFeedProcessorKey].Count == 0)
                    {
                        dependenciesOnProcessors.Remove(changeFeedProcessorKey);
                        changefeedProcessors[changeFeedProcessorKey].StopAsync().Wait();
                        changefeedProcessors.Remove(changeFeedProcessorKey);
                    }
                } 
            }
        }

        // In the **CreateChangeFeedProcessor** method, we take the change feed processor key 
        // and split out all the parameters needed to create a change feed processor
        private static IChangeFeedProcessor CreateChangeFeedProcessor(string changeFeedProcessorKey)
        {
            string[] changeFeedProcessorAttributes = changeFeedProcessorKey.Split('-');

            string monitoredUri = changeFeedProcessorAttributes[0];
            string monitoredAuthKey = changeFeedProcessorAttributes[1];
            string monitoreddatabaseId = changeFeedProcessorAttributes[2];
            string monitoredContainerId = changeFeedProcessorAttributes[3];

            string leaseUri = changeFeedProcessorAttributes[4];
            string leaseAuthKey = changeFeedProcessorAttributes[5];
            string leaseDatabaseId = changeFeedProcessorAttributes[6];
            string leaseContainerId = changeFeedProcessorAttributes[7];

            string cacheId = changeFeedProcessorAttributes[8];

            string hostName = "host-" + Guid.NewGuid().ToString();

            DocumentCollectionInfo monitoredContainerInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(monitoredUri),
                MasterKey = monitoredAuthKey,
                DatabaseName = monitoreddatabaseId,
                CollectionName = monitoredContainerId
            };

            DocumentCollectionInfo leaseContainerInfo = new DocumentCollectionInfo
            {
                Uri = new Uri(leaseUri),
                MasterKey = leaseAuthKey,
                DatabaseName = leaseDatabaseId,
                CollectionName = leaseContainerId
            };

            DocumentClient feedClient = new DocumentClient(
                serviceEndpoint: new Uri(monitoredUri),
                authKeyOrResourceToken: monitoredAuthKey,
                handler: new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                },
                connectionPolicy: new ConnectionPolicy
                {
                    EnableEndpointDiscovery = false,
                    ConnectionMode = ConnectionMode.Gateway,
                    ConnectionProtocol = Protocol.Tcp
                });

            DocumentClient leaseClient = new DocumentClient(
                serviceEndpoint: new Uri(leaseUri),
                authKeyOrResourceToken: leaseAuthKey,
                handler: new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                },
                connectionPolicy: new ConnectionPolicy
                {
                    EnableEndpointDiscovery = false,
                    ConnectionMode = ConnectionMode.Gateway,
                    ConnectionProtocol = Protocol.Tcp
                });

            return new ChangeFeedProcessorBuilder()
                .WithHostName(hostName)
                .WithFeedCollection(monitoredContainerInfo)
                .WithLeaseCollection(leaseContainerInfo)
                .WithFeedDocumentClient(feedClient)
                .WithLeaseDocumentClient(leaseClient)
                .WithObserverFactory(new CosmosDbChangFeedObserverFactory(changeFeedProcessorKey))
                .WithProcessorOptions(new ChangeFeedProcessorOptions
                {
                    // We create a separate lease document for each of the cache
                    // server nodes so as to avoid missing on updates to db state 
                    // changes due to partitioning of data on different nodes
                    LeasePrefix = $"NCache--{cacheId}-{Guid.NewGuid().ToString()}",
                    StartTime = DateTime.Now
                })
                .BuildAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}
