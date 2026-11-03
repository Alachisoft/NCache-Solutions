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
using Models;
using MongoDB.Driver;
using MongoDbNotifyExtensibleDependencyImpl;
using System;

namespace MongoDbNotificationDependencyNamespace
{
    // This class is responsible for initializing the resources required to trigger dependency

    [Serializable]
    public class MongoDbNotificationDependency : NotifyExtensibleDependency
    {
        public static string ConString { get; set; }                        // MongoDB connection string
        public static string Database { get; set; }                         // MongoDB dtabase name
        public static string Collection { get; set; }                       // MongoDB collection name to be watched

        public static MongoClient _client;                                  // MongoDB client instance
        public static IMongoDatabase _db;                                   // MongoDB database instance
        public static IMongoCollection<Customer> _coll;                     // MongoDB collection instance

        private string _itemKey;                                            // Dependent item's key

        public MongoDbNotificationDependency(string itemKey, string conString, string db, string coll)
        {
            ConString = conString;
            Database = db;
            Collection = coll;
            _itemKey = itemKey;
        }

        // This function is called everytime when an item is inserted in cache with dependency.
        public override bool Initialize()
        {
            try
            {
                _client = new MongoClient(ConString);
                _db = _client.GetDatabase(Database);
                _coll = _db.GetCollection<Customer>(Collection);

                // Method of a static class MongoDbChangeStreamListner that registers dependency of a cache item.
                MongoDbChangeStreamListner.RegisterDependency(_itemKey, this);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
