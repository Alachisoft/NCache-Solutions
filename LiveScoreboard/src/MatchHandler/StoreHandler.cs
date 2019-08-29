//  Copyright (c) 2018 Alachisoft
//  
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//     http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchHandler
{
    public class StoreHandler
    {
        // handler for cache
        private ICache _cache;

        // read/write thru options for cache
        ReadThruOptions readThruOptions;
        WriteThruOptions writeThruOptions;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="cacheName">name of cache</param>
        /// <param name="readThruProviderName">readthru provider name</param>
        /// <param name="writeThruProviderName">writethru provider name</param>
        public StoreHandler(string cacheName, string readThruProviderName, string writeThruProviderName)
        {
            // initialize cache
            _cache = CacheManager.GetCache(cacheName);

            // assign null to provider names if empty provider names are received
            if (readThruProviderName.Equals(string.Empty))
                readThruProviderName = null;
            if (writeThruProviderName.Equals(string.Empty))
                writeThruProviderName = null;

            // initialize read/write thru options for later use
            readThruOptions = new ReadThruOptions(ReadMode.ReadThru, readThruProviderName);
            writeThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProviderName);
        }

        /// <summary>
        /// fetch stored updates from cache
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <returns>list of <seealso cref="MatchUpdate"/></returns>
        public List<MatchUpdate> FetchUpdates(string matchName)
        {
            // fetch list handler from cache
            IDistributedList<MatchUpdate> distributedList = _cache.DataTypeManager.GetList<MatchUpdate>(matchName, readThruOptions);

            // if there is no list initialized, create new list with writethru enabled
            if (distributedList == null)
            {
                distributedList = _cache.DataTypeManager.CreateList<MatchUpdate>(matchName, null, writeThruOptions);
            }

            // fetch list of updates
            List<MatchUpdate> list = (from matchUpdate in distributedList select matchUpdate).ToList();

            // return list to user
            return list;
        }

        /// <summary>
        /// store new update in cache
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <param name="matchUpdate">instance of <seealso cref="MatchUpdate"/> containing a single update</param>
        public void SaveUpdates(string matchName, MatchUpdate matchUpdate)
        {
            // fetch list handler from cache
            IDistributedList<MatchUpdate> distributedList = _cache.DataTypeManager.GetList<MatchUpdate>(matchName, readThruOptions);

            // if there is no list initialized, create new list with writethru enabled
            if (distributedList == null)
            {
                distributedList = _cache.DataTypeManager.CreateList<MatchUpdate>(matchName, null, writeThruOptions);
            }

            // add new update to list
            distributedList.Add(matchUpdate);
        }

        /// <summary>
        /// Empty all updates on the specified match name
        /// </summary>
        /// <param name="matchName">name of match</param>
        public void ClearUpdates(string matchName)
        {
            // fetch list handler from cache
            IDistributedList<MatchUpdate> distributedList = _cache.DataTypeManager.GetList<MatchUpdate>(matchName, readThruOptions);

            // check if list already exists
            if (distributedList != null)
            {
                // clear the list
                distributedList.Clear();
            }
        }
    }
}
