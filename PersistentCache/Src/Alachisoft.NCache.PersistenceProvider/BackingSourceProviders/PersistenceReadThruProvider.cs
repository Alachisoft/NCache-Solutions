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
using PersistenceProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PersistentNCache.BackingSourceProviders
{
    /// <summary>
    /// PersistenceReadThruProvider implementation which use IPersistenceStore
    /// to get items from persisted store
    /// </summary>
    public class PersistenceReadThruProvider : ProviderBase, IReadThruProvider
    {
             
        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            //Data structures are avaialable in ProviderDataTypeItem<IEnumerable>
            return (ProviderDataTypeItem<IEnumerable>)_persistenceProvider.Get(new string[] { key })?[key];
        }

        public ProviderCacheItem LoadFromSource(string key)
        {
            try
            {
                ProviderItemBase providerItem = (_persistenceProvider.Get(new string[] { key })[key]);
                return (ProviderCacheItem)providerItem;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }

        }

        public IDictionary<string, ProviderCacheItem> LoadFromSource(ICollection<string> keys)
        {
            try
            {
                IDictionary<string, ProviderCacheItem> providerCacheItems = new Dictionary<string, ProviderCacheItem>();
                foreach (var providerItem in _persistenceProvider.Get(keys))
                {
                    providerCacheItems.Add(providerItem.Key,(ProviderCacheItem)providerItem.Value);
                }
                return providerCacheItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
