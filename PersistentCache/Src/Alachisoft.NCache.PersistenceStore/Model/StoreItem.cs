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

using Alachisoft.NCache.Caching.EvictionPolicies;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;
using Newtonsoft.Json;
using PersistentNCache;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersistenceProviders.Model
{
    public class StoreItem
    {
        public StoreItem()
        {

        }
        public StoreItem(string key, ProviderItemBase providerItem,ISerializer serializer)
        {
            InsertionTime = DateTime.Now;
            if(providerItem is ProviderCacheItem)
            {
                MakeStoreItem(key, (ProviderCacheItem)providerItem, serializer);
            }
            else 
            {
                MakeStoreItem(key, (ProviderDataTypeItem<object>)providerItem);
            }
        }
        public StoreItem MakeStoreItem(string key, ProviderCacheItem providerItem, ISerializer serializer)
        {
            Key = key;
            ExpirationType = providerItem.Expiration.Type;
            TimeToLive = providerItem.Expiration.ExpireAfter;
            ItemPriority= providerItem.ItemPriority;
            Group= providerItem.Group;
            UserObject = serializer.Serialize(providerItem.GetValue<object>());

            return this;
        }
        public StoreItem MakeStoreItem(string key, ProviderDataTypeItem<object> providerItem)
        {
            Key = key;
            ExpirationType = providerItem.Expiration.Type;
            TimeToLive = providerItem.Expiration.ExpireAfter;
            ItemPriority = providerItem.ItemPriority;
            Group = providerItem.Group;

            return this;
        }       

        [Key]
        [JsonProperty("id")]
        public string Key { get; set; }
        public string UserObject { get; set; }
        public ItemType ItemType { get; set; }


        public Alachisoft.NCache.Runtime.Caching.ExpirationType ExpirationType { get; set; }
        public DateTime InsertionTime { get; set; }

        public TimeSpan TimeToLive{ get; set; }

        public EvictionHintType EvictionHint { get; set; }

        public CacheItemPriority ItemPriority { get; set; }

        public string Group { get; set; }

        public virtual ICollection<DataTypeItem> DataTypeItems { get; set; }
        /* This Code is commented to do after
        public Dependency dependency { get; set; }
        ResyncOptions

        public NamedTagsDictionary NamedTagsDictionary { get; set; }
        public List<string> Tags { get; set; }

        */
    }
}
