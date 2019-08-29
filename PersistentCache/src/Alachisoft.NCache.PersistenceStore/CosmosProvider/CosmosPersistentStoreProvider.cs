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

using Alachisoft.NCache.PersistenceProvider;
using Alachisoft.NCache.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alachisoft.NCache.PersistenceStore
{
    /// <summary>
    /// Provider for cosmos db 
    /// </summary>
    public class CosmosPersistentStoreProvider : IPersistenceProvider
    {
        ISerializer _serializer = null;
        DocumentPersistentStoreRepository<StoreItem> _documentDb = null;

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
                _documentDb= new DocumentPersistentStoreRepository<StoreItem>();
                _documentDb.Initialize(arguments[0], arguments[1], arguments[2], arguments[3]);
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
                var result = _documentDb.CreateItemAsync(new StoreItem(item.Key, item.Value).ToSerializedStoreItem(item.Value,_serializer)).Result;
            }
        }
        /// <summary>
        /// Insert in to Backing Source
        /// </summary>
        /// <param name="table">key value pair of string and ProviderItemBase</param>
        public void Insert(IDictionary<string, ProviderItemBase> providerItems)
        {
            IDictionary<string, ProviderItemBase> itemsExists = Get(providerItems.Keys);
            foreach (var providerItem in itemsExists)
            {
                //if does not contain in database then add
                if (providerItem.Value == null)
                {
                    var result = _documentDb.CreateItemAsync(new StoreItem(providerItem.Key, providerItems[providerItem.Key]).ToSerializedStoreItem(providerItems[providerItem.Key], _serializer)).Result;
                }
                //if already exists in database then update
                else
                {
                    var result = _documentDb.UpdateItemAsync(providerItem.Key, new StoreItem(providerItem.Key, providerItems[providerItem.Key]).ToSerializedStoreItem(providerItems[providerItem.Key], _serializer)).Result;
                }
            }
            
        }
        /// <summary>
        /// Remove Collection of keys from backing source
        /// </summary>
        /// <param name="keys">collection of keys</param>
        /// <returns></returns>
        public IDictionary<string, ProviderItemBase> Remove(ICollection<string> keys)
        {

            IDictionary<string, ProviderItemBase> providerItemTable = new Dictionary<string, ProviderItemBase>();
            foreach (var key in keys)
            {
                var storeItem = _documentDb.GetItemAsync(key).Result;
                providerItemTable.Add(key, storeItem.ToProviderItemBase(_serializer));
                var result = _documentDb.DeleteItemAsync(key);
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
            List<string> itemsToRemove = new List<string>();
            foreach (var key in keys)
            {
                var storeItem = _documentDb.GetItemAsync(key).Result;
                if (storeItem != null)
                {
                    DateTime expirationTime = storeItem.InsertionTime.Add(storeItem.TimeToLive);
                    if (DateTime.Now > expirationTime)
                    {
                        itemsToRemove.Add(storeItem.Key);
                    }
                    else
                    {
                        storeItems.Add(storeItem);
                    }
                }
            }
            if(itemsToRemove.Count>0)
                Remove(itemsToRemove);
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
            var storeItems = _documentDb.GetItemsAsync(item => item.Key!=null).Result;
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
