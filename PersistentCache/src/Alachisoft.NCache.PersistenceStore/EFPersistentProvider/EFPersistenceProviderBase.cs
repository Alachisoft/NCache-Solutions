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
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Alachisoft.NCache.PersistenceProvider;

namespace Alachisoft.NCache.PersistenceStore
{
    public abstract class EFPersistenceProviderBase : IPersistenceProvider
    {   
        #region fields and properties
        private PersistentStoreContext _dbContext;
        private ISerializer _serializer;

        /// <summary>
        /// Db context to be set from init
        /// </summary>
        public PersistentStoreContext DbContext
        {
            set
            {
                _dbContext = value;
            }
        }
        /// <summary>
        /// Serialier used to serialize user object
        /// </summary>
        public ISerializer Serializer
        {
            set
            {
                _serializer = value;
            }
        }
        #endregion

        #region Initialization of Db
        public abstract void Init(string connectionString);
        public virtual void Dispose() { _dbContext.Dispose(); }
        #endregion

        #region Basic Operations

        public IDictionary<string, ProviderItemBase> Get(ICollection<string> keys)
        {
            IDictionary<string, ProviderItemBase> dictionary = new Dictionary<string, ProviderItemBase>();
            var storeItems =
                            from storeItem in _dbContext.CacheItem
                            where (keys.Contains(storeItem.Key))
                            select storeItem;

            var itemsToRemove = new List<string>();
            foreach (var storeItem in storeItems)
            {
                DateTime expirationTime = storeItem.InsertionTime.Add(storeItem.TimeToLive);
                if (DateTime.Now > expirationTime)
                {
                    itemsToRemove.Add(storeItem.Key);
                }
                dictionary.Add(storeItem.Key, storeItem.ToProviderItemBase(_serializer));
            }
            if (itemsToRemove.Count > 0) Remove(itemsToRemove);
            return dictionary;
        }

        public IDictionary<string, ProviderItemBase> GetAll(string hint=null)
        {
            IDictionary<string, ProviderItemBase> dictionary =
            new Dictionary<string, ProviderItemBase>();
            var storeItems = _dbContext.CacheItem;

            foreach (var storeItem in storeItems)
            {
                dictionary.Add(storeItem.Key, storeItem.ToProviderItemBase(_serializer));
            }
            return dictionary;
        }
        public void Add(IDictionary<string, ProviderItemBase> providerItems)
        {
            List<StoreItem> storeItems = new List<StoreItem>();
            foreach (var providerItm in providerItems)
            {
                storeItems.Add(new StoreItem(providerItm.Key, providerItm.Value).ToSerializedStoreItem(providerItm.Value,_serializer)
                    );
            }
            _dbContext.CacheItem.AddRange(storeItems);
            _dbContext.SaveChanges();
        }

        public void Insert(IDictionary<string,ProviderItemBase> providerItems)
        {
            List<StoreItem> storeItems = new List<StoreItem>();
            foreach (var providerItm in providerItems)
            {
                StoreItem storeItem = new StoreItem();

                storeItems.Add(new StoreItem(providerItm.Key, providerItm.Value).ToSerializedStoreItem(providerItm.Value,_serializer));
            }
            _dbContext.CacheItem.UpdateRange(storeItems);
            _dbContext.SaveChanges();
        }

        public IDictionary<string, ProviderItemBase> Remove(ICollection<string> keys)
        {
            var storeItems = GetStoreItemInternal(keys);
            var providerItems = new Dictionary<string, ProviderItemBase>();

            foreach (var item in storeItems)
            {
                _dbContext.CacheItem.Remove(item.Value);
                providerItems.Add(item.Key, item.Value.ToProviderItemBase(_serializer));
            }
            _dbContext.SaveChanges();
            return providerItems;
        }
        #endregion

        #region DataStructures operations
        public virtual void AddToDataType(string key,ProviderItemBase item, DistributedDataType dataType)
        {
            //To add in Data structure
            switch (dataType)
            {
                case DistributedDataType.List:
                    //According to the user's logic
                    break;
            }
        }

        public virtual void RemoveFromDataType(string keys, DistributedDataType dataType)
        {
            //Remove from data structure
            switch (dataType)
            {
                case DistributedDataType.List:
                    //According to the user's logic
                    break;
            }

        }

        public virtual void UpdateToDataType(string key,ProviderItemBase items, DistributedDataType dataType)
        {
            //update to data structure
            switch (dataType)
            {
                case DistributedDataType.List:
                    //According to the user's logic
                    break;
            }

        }

        #endregion

        #region private Methods
        private IDictionary<string, StoreItem> GetStoreItemInternal(ICollection<string> keys)
        {
            IDictionary<string, StoreItem> dictionary =
                new Dictionary<string, StoreItem>();

            var storeItems =
            from storeItem in _dbContext.CacheItem
            where (keys.Contains(storeItem.Key))
            select storeItem;

            foreach (var storeItem in storeItems)
            {
                dictionary.Add(storeItem.Key, storeItem);
            }
            return dictionary;
        }

        #endregion

    }
}
