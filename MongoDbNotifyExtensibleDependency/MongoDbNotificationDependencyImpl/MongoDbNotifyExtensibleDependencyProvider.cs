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
using MongoDbNotificationDependencyNamespace;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbNotifyExtensibleDependencyImpl
{
    // The MongoDbNotifyExtensibleDependencyProvider implements ICustomDependencyProvider interface that   
    // returns an instance of MongoDbNotificationDependency class. This instance registers MongoDB dependency with cache2

    [Serializable]
    class MongoDbNotifyExtensibleDependencyProvider : ICustomDependencyProvider
    {
        public ExtensibleDependency CreateDependency(string key, IDictionary<string, string> parameters)
        {
            // Validate Parameters

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (!parameters.ContainsKey("ConString"))
            {
                throw new Exception("Connection String not found. Please provide a valid Mongo DB Connection String.");
            }
            if (!parameters.ContainsKey("DatabaseName"))
            {
                throw new Exception("Database name not found. Please provide a valid Mongo DB database name.");
            }
            if (!parameters.ContainsKey("CollectionName"))
            {
                throw new Exception("Collection name not found. Please provide a valid Mongo DB collection name.");
            }

            foreach (var item in parameters)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    throw new Exception($"Null or empty value against key: {item.Key}. Please provide a valid value.");
                }
            }

            // Return MongoDbNotificationDependency instance
            return new MongoDbNotificationDependency(
                key,
                parameters["ConString"],
                parameters["DatabaseName"],
                parameters["CollectionName"]
                );
        }

        public void Dispose()
        {
        }

        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
        }
    }
}
