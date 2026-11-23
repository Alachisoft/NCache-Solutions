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

using System;
using System.Collections;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Runtime.Caching;

namespace Alachisoft.NCache.PersistenceProvider
{

    public class PersistenceStartupLoader : ProviderBase, ICacheLoader
    {
        private const int DefaultCachePort = 9800;

        private string _hint = null;

        private readonly int _singleIterationSize = 1000;

        private ICache _cache = null;

        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
            try
            {
                base.Init(new Hashtable(new Dictionary<string, string>(parameters)), cacheName);
                if (parameters.ContainsKey("distributionhint"))
                {
                    _hint = parameters["distributionhint"];
                }
                _cache = CacheManager.GetCache(cacheName, CreateConnectionOptions(parameters));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public object LoadDatasetOnStartup(string dataset)
        {
            try
            {
                Dictionary<string, CacheItem> batch = new Dictionary<string, CacheItem>();
                foreach (KeyValuePair<string, ProviderItemBase> item in PersistenceProvider.GetAll(_hint))
                {
                    if (item.Value is ProviderCacheItem providerCacheItem)
                    {
                        batch.Add(item.Key, ToCacheItem(providerCacheItem));
                    }
                    if (batch.Count == _singleIterationSize)
                    {
                        _cache.InsertBulk(batch);
                        batch.Clear();
                    }
                }
                if (batch.Count > 0)
                {
                    _cache.InsertBulk(batch);
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public object RefreshDataset(string dataset, object userContext)
        {
            return userContext;
        }

        public IDictionary<string, RefreshPreference> GetDatasetsToRefresh(IDictionary<string, object> userContexts)
        {
            return new Dictionary<string, RefreshPreference>();
        }

        public override void Dispose()
        {
            _cache?.Dispose();
            base.Dispose();
        }

        private static ConnectionOptions CreateConnectionOptions(IDictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("cacheserver", out var server) || string.IsNullOrWhiteSpace(server))
            {
                throw new ArgumentException("The 'cacheserver' parameter of the startup loader is missing.");
            }
            int result = DefaultCachePort;
            if (parameters.TryGetValue("cacheport", out var portText) && !int.TryParse(portText, out result))
            {
                throw new ArgumentException("The 'cacheport' parameter of the startup loader is not a valid port: '" + portText + "'.");
            }
            return new ConnectionOptions
            {
                Servers = new List<ServerInfo>
                {
                    new ServerInfo(server, result)
                }
            };
        }

        private static CacheItem ToCacheItem(ProviderCacheItem providerCacheItem)
        {
            return new CacheItem(providerCacheItem.GetValue<object>())
            {
                Expiration = providerCacheItem.Expiration,
                Group = providerCacheItem.Group,
                Tags = providerCacheItem.Tags,
                NamedTags = providerCacheItem.NamedTags,
                Priority = providerCacheItem.ItemPriority
            };
        }
    }
}
