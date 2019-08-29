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

using Alachisoft.NCache.Client;
using Alachisoft.NCache.PersistenceProvider;
using Alachisoft.NCache.Runtime.Caching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alachisoft.NCache.PersistenceStore
{
    public static class TypeCastUtil
    {
        /// <summary>
        /// Convert StoreItem to ProviderItemBase based on the serializer provided
        /// </summary>
        /// <param name="storeItem"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static ProviderItemBase ToProviderItemBase(this StoreItem storeItem,ISerializer serializer)
        {
            ProviderItemBase providerItem = null;
            switch (storeItem.ItemType)
            {
                case ItemType.CacheItem:
                    providerItem = new ProviderCacheItem(storeItem.UserObject);
                    SetProviderItemBaseProperties(storeItem, providerItem);

                    providerItem.ToProviderCacheItem().SetValue(storeItem.UserObject);
                    return providerItem;

            }
            return providerItem;
        }

        /// <summary>
        /// Converting provider Item to Provider Cache Item
        /// </summary>
        /// <param name="providerItem"></param>
        /// <returns></returns>
        public static ProviderCacheItem ToProviderCacheItem(this ProviderItemBase providerItem)
        {
            return (ProviderCacheItem)providerItem;
        }

        /// <summary>
        /// Converting ProviderItemBase to ProviderDataTypeItem
        /// </summary>
        /// <param name="providerItem"></param>
        /// <returns></returns>
        public static ProviderDataTypeItem<object> ToProviderDataTypeItem(this ProviderItemBase providerItem)
        {
            return (ProviderDataTypeItem<object>)providerItem;
        }

        public static StoreItem ToSerializedStoreItem(this StoreItem storeItem, ProviderItemBase providerItem,ISerializer serializer)
        {
            ProviderCacheItem providerCacheItem = providerItem as ProviderCacheItem;
            if (providerCacheItem == null)
                throw new Exception("providerItembase is not cache item");
            storeItem.UserObject = serializer.Serialize(providerCacheItem.GetValue<object>());
            return storeItem;
        }


        private static ProviderItemBase SetProviderItemBaseProperties(StoreItem storeItem,ProviderItemBase providerItem)
        {
            providerItem.Expiration = new Expiration(storeItem.ExpirationType, storeItem.TimeToLive);
            providerItem.Group = storeItem.Group;
            providerItem.ItemPriority = storeItem.ItemPriority;
            return providerItem;
        }



    }
}
