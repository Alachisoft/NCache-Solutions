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
using Alachisoft.NCache.Runtime.DatasourceProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alachisoft.NCache.PersistenceProvider
{
    /// <summary>
    /// PersistenceReadThruProvider implementation which use IPersistenceStore
    /// to get items from persisted store
    /// </summary>
    public class PersistenceReadThruProvider : ProviderBase, IReadThruProvider
    {

        /// <summary>
        /// LoadDataTypeFromSource to load data type from source.
        /// </summary>
        /// <param name="key">key of data type</param>
        /// <param name="dataType">type of datatype</param>
        /// <returns>It returns the ProviderDataTypeItem IEnerable</returns>
        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            try
            {
                //Data structures are avaialable in ProviderDataTypeItem<IEnumerable>
                return (ProviderDataTypeItem<IEnumerable>)PersistenceProvider.Get(new string[] { key })?[key];
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
                throw e;
            }
        }
        /// <summary>
        /// Load From Source for provider cache item
        /// </summary>
        /// <param name="key">key of cacheItem</param>
        /// <returns>returns ProviderCacheItem</returns>
        public ProviderCacheItem LoadFromSource(string key)
        {
            try
            {
                var providerItems=PersistenceProvider.Get(new string[] { key });
                if (providerItems.ContainsKey(key))
                {
                    return (ProviderCacheItem)providerItems[key];
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                throw e;
            }

        }
        /// <summary>
        /// Load from source for bulk for ProviderCacheItem
        /// </summary>
        /// <param name="keys">< collection of keys against ProviderCacheItems/param>
        /// <returns>returns dictionary of ProviderCacheItem</returns>
        public IDictionary<string, ProviderCacheItem> LoadFromSource(ICollection<string> keys)
        {
            try
            {
                IDictionary<string, ProviderCacheItem> providerCacheItems = new Dictionary<string, ProviderCacheItem>();
                foreach (var providerItem in PersistenceProvider.Get(keys))
                {
                    providerCacheItems.Add(providerItem.Key,(ProviderCacheItem)providerItem.Value);
                }
                return providerCacheItems;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
