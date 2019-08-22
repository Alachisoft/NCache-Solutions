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
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using PersistenceProviders.Model;
using PersistentNCache.DbProvider;
using PersistentNCache.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Name of cache to connect with
            // Providers must be deployed with that cache
            //Name of provider to be deployed
            //PersistenceReadThru , PersistenceWriteThru and PersistenceStartupLoadewr
            string cacheName = "persistentCache";
            //Key for which the item is to be added
            string key = "k1";

            //value can be any of user object 
            //to be stored against the key in cache
            string value = "value1";

            CacheItem cacheItem = new CacheItem(value);
            WriteThruOptions writeThru = new WriteThruOptions(WriteMode.WriteThru);
            ReadThruOptions readThru = new ReadThruOptions(ReadMode.ReadThru);


            ICache cache = CacheManager.GetCache(cacheName);

            //To clear thr cache in order to test these providers 
            cache.Clear();

            //Adding items to cache with writethru 
            //that will write it to the persistent store
            cache.Add(key, cacheItem, writeThru);

            //if the item is successfully added to cache
            // Go and verify it must also be added in persitent store or db
            if (cache.Count==1)
            {
                //check if the db contains item or not
            }
            //This is an example if the item is removed from cache
            // and not from persistent store.
            // This can either be done by remove without writethru
            // or by eviction , dependency trigger , expiration or due to any other reason.
            cache.Remove(key);

            //As the item is persisted to persistent store then on fetching the item
            //it must be fetched from persistent store even if it is not been present in cache
            string val = cache.Get<string>(key, readThru);

        }
    }
}
