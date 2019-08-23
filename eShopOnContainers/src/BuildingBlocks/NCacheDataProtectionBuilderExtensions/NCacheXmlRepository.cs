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
using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.AspNetCore.DataProtection
{
    public sealed class NCacheXmlRepository : IXmlRepository
    {
        private readonly string _cacheID;
        private readonly CacheConnectionOptions _options;

        public NCacheXmlRepository(string cacheID, CacheConnectionOptions options = null)
        {
            _cacheID = cacheID;
            _options = options;
        }


        public IReadOnlyCollection<XElement> GetAllElements()
        {
            using (var _cache = CacheManager.GetCache(_cacheID, _options))
            {
                var items = _cache.SearchService.GetByTag<string>(new Tag("NCacheDataProtectionKeys"));
                var list = new List<XElement>();


                if (items != null)
                {
                    foreach (var item in items)
                    {
                        list.Add(XElement.Parse(item.Value));
                    }
                }

                return list;
            }
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            using (var cache = CacheManager.GetCache(_cacheID, _options))
            {
                var xml = element.ToString(SaveOptions.DisableFormatting);

                var Id = Guid.NewGuid().ToString();
                if (friendlyName != null)
                {
                    Id = friendlyName;
                }

                var cacheItem = new CacheItem(xml)
                {
                    Tags = new[] { new Tag("NCacheDataProtectionKeys") }
                };

                cache.Insert(Id, cacheItem);
            }
        }
    }
}
