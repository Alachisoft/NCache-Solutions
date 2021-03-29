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

using Alachisoft.NCache.Runtime.CustomDependencyProviders;
using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    // The NotifyCustomDependencyProvider implements ICustomDependencyProvider interface which   
    // returns an instance of CosmosDbNotificationDependency. This instance registers Cosmos DB
    // dependency with cache
    [Serializable]
    public class NotifyCustomDependencyProvider : ICustomDependencyProvider
    {
        public ExtensibleDependency CreateDependency(string key, IDictionary<string, string> parameters)
        {
            //Validate all arguments
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (!parameters.ContainsKey("Key"))
            {
                throw new Exception("Key for cache item not found. Please provide a valid item key.");
            }
            if (!parameters.ContainsKey("CacheId"))
            {
                throw new Exception("Cache ID not found. Please provide a valid Cache ID.");
            }
            if (!parameters.ContainsKey("EndPoint"))
            {
                throw new Exception("End point not found. Please provide a valid Cosmos DB end point.");
            }
            if (!parameters.ContainsKey("AuthKey"))
            {
                throw new Exception("Authentication key not found. Please provide a valid Cosmos DB authentication key.");
            }
            if (!parameters.ContainsKey("DatabaseName"))
            {
                throw new Exception("Database name not found. Please provide a valid Cosmos DB database name.");
            }
            if (!parameters.ContainsKey("MonitoredCollection"))
            {
                throw new Exception("Collection name not found. Please provide a valid Cosmos DB collection name.");
            }

            if (!parameters.ContainsKey("LeaseEndPoint"))
            {
                throw new Exception("Lease end point not found. Please provide a valid Cosmos DB lease end point.");
            }
            if (!parameters.ContainsKey("LeaseAuthKey"))
            {
                throw new Exception("Lease authentication key not found. Please provide a valid Cosmos DB lease authentication key.");
            }
            if (!parameters.ContainsKey("LeaseDatabaseName"))
            {
                throw new Exception("Lease database name not found. Please provide a valid Cosmos DB lease database name.");
            }
            if (!parameters.ContainsKey("LeaseCollection"))
            {
                throw new Exception("Lease collection name not found. Please provide a valid Cosmos DB lease collection name.");
            }
            foreach (var item in parameters)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    throw new Exception($"Null or empty value against key: {item.Key}. Please provide a valid value.");
                }
            }

            return new CosmosDbNotificationDependency(
                parameters["Key"],
                parameters["CacheId"],
                parameters["EndPoint"],
                parameters["AuthKey"],
                parameters["DatabaseName"],
                parameters["MonitoredCollection"],
                parameters["LeaseEndPoint"],
                parameters["LeaseAuthKey"],
                parameters["LeaseDatabaseName"],
                parameters["LeaseCollection"]);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(IDictionary<string, string> parameters, string cacheName)
        {

        }
    }
}
