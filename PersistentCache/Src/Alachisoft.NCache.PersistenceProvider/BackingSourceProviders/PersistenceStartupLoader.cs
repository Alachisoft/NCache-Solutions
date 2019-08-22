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

using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.SocketServer.CacheLoader;
using PersistenceProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PersistentNCache.BackingSourceProviders
{
    /// <summary>
    /// PersistenceStartupLoader implementation which use IPersistenceStore
    /// to fetch items from persisted store on cache stratup
    /// </summary>
    public class PersistenceStartupLoader : ProviderBase, ICacheLoader
    {
        private string _hint = "";
        private int currentIndex=0;
        private int singleIterationSize = 1000;
        private IDictionary<string, ProviderItemBase> persistentItems = null;
        /// <summary>
        /// Init is need to be override as a more specific implementation in startup loader is needed
        /// </summary>
        /// <param name="parameters"> parameters passed at cache configuration</param>
        /// <param name="cacheId">cache name</param>
        public override void Init(IDictionary parameters, string cacheId)
        {
            try
            {
                base.Init(parameters, cacheId);
                // Get distribution hints – distributionhint is a keyword
                if (parameters.Contains("distributionhint"))
                {
                    _hint = parameters["distributionhint"].ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

        }
        /// <summary>
        /// Load Next is to be call till HasMoreData is false
        /// </summary>
        /// <param name="userContext">user object to be fetched in next LoadNext call</param>
        /// <returns></returns>
        public LoaderResult LoadNext(object userContext)
        {
            try
            {
                if(persistentItems==null)
                    persistentItems = _persistenceProvider.GetAll();

                LoaderResult result = new LoaderResult();
                result.UserContext = userContext;
                result.HasMoreData = false;

                foreach (var item in persistentItems)
                {
                    result.Data.Add(new KeyValuePair<string, ProviderItemBase>(item.Key, item.Value));
                    currentIndex++;
                    if (currentIndex % singleIterationSize == 0) break;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

        }
    }
}
