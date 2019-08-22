// Copyright (c) 2019 Alachisoft
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

using Alachisoft.NCache.Runtime.Caching;
using PersistenceProviders;
using PersistenceProviders.Model;
using PersistentNCache.SerializationProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersistentNCache.DbProvider
{
    /// <summary>
    /// Provider for cosmos db 
    /// </summary>
    public class CosmosDbProvider : IPersistenceProvider
    {
        ISerializer _serializer = null;
        DocumentDBRepository<StoreItem> documentDb = null;

        #region Initialization
        /// <summary>
        /// Initialization of Db
        /// </summary>
        /// <param name="connectionString"></param>
        public void Init(string connectionString)
        {
            _serializer = new JSonSerializer();
            string[] arguments = connectionString.Split('~');
            if (arguments.Length == 4)
            {
                documentDb= new DocumentDBRepository<StoreItem>(arguments[0], arguments[1], arguments[2], arguments[3]);
            }
            else
            {
                throw new Exception("Params provided to provider are not correct");
            }
        }

        public void Dispose()
        {

        }
        #endregion

        #region Basic operations
        /// <summary>
        /// Add to Backing Source
        /// </summary>
        /// <param name="table">key value pair of string and providerItemBase</param>
        public void Add(IDictionary<string, ProviderItemBase> table)
        {
            foreach (var item in table)
            {
                var result = documentDb.CreateItemAsync(new StoreItem(item.Key, item.Value,_serializer)).Result;
            }
        }
        /// <summary>
        /// Insert in to Backing Source
        /// </summary>
        /// <param name="table">key value pair of string and ProviderItemBase</param>
        public void Insert(IDictionary<string, ProviderItemBase> table)
        {
            foreach (var item in table)
            {
                var result = documentDb.UpdateItemAsync(item.Key, new StoreItem(item.Key, item.Value,_serializer)).Result;
            }
        }
        /// <summary>
        /// Remove Collection of keys from backing source
        /// </summary>
        /// <param name="keys">collection of keys</param>
        /// <returns></returns>
        public IDictionary<string, ProviderItemBase> Remove(ICollection<string> keys)
        {
            var storeItems = documentDb.GetItemsAsync(item => item.Key.Equals(keys)).Result;
            IDictionary<string, ProviderItemBase> providerItemTable = new Dictionary<string, ProviderItemBase>();
            foreach (var item in storeItems)
            {
                providerItemTable.Add(item.Key, item.ToProviderItemBase(_serializer));
            }
            foreach (var key in keys)
            {
                var result = documentDb.DeleteItemAsync(key);
            }
            return providerItemTable;
        }
        /// <summary>
        /// Get collection of keys from backing source
        /// </summary>
        /// <param name="keys">collection of keys</param>
        /// <returns></returns>
        public IDictionary<string, ProviderItemBase> Get(ICollection<string> keys)
        {
            IDictionary<string, ProviderItemBase> providerItemTable= new Dictionary<string, ProviderItemBase>();
            List<StoreItem> storeItems = new List<StoreItem>();
            foreach(var key in keys)
            {
                var storeItem = documentDb.GetItemAsync(key).Result;
                if(storeItem!=null)
                    storeItems.Add(storeItem);
            }

            foreach (var item in storeItems)
            {
                providerItemTable.Add(item.Key, item.ToProviderItemBase(_serializer));
            }
            return providerItemTable;
        }

        /// <summary>
        /// Get complete store at cache start
        /// </summary>
        /// <param name="hint">optional feature</param>
        /// <returns></returns>
        public IDictionary<string, ProviderItemBase> GetAll(string hint = null)
        {
            IDictionary<string, ProviderItemBase> providerItemTable = new Dictionary<string, ProviderItemBase>();
            var storeItems = documentDb.GetItemsAsync(item => item.Key!=null).Result;
            foreach (var item in storeItems)
            {
                providerItemTable.Add(item.Key, item.ToProviderItemBase(_serializer));
            }
            return providerItemTable;
        }
        #endregion

        #region Data type operations
        public void AddToDataType(string key, ProviderItemBase items, DistributedDataType dataType)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromDataType(string keys, DistributedDataType dataType)
        {
            throw new NotImplementedException();
        }

        public void UpdateToDataType(string key, ProviderItemBase items, DistributedDataType dataType)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
