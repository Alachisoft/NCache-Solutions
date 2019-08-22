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

namespace PersistenceProviders
{
    /// <summary>
    /// IPersistenceProvider this interface is need to be implemented in order to use some other db
    /// </summary>
    public interface IPersistenceProvider : IDsPersistenceProvider,IDisposable
    {
        /// <summary>
        /// Initialization and  connection related  is need to be done here
        /// </summary>
        /// <param name="connectionString">connection string use to connect with database</param>
        void Init(string connectionString);
        /// <summary>
        /// Add into database
        /// </summary>
        /// <param name="table">The key value pair to add in to cache</param>
        void Add(IDictionary<string, ProviderItemBase> table);
        /// <summary>
        /// Insert into database
        /// </summary>
        /// <param name="table">The key value pair to Insert in to cache</param>
        void Insert(IDictionary<string,ProviderItemBase> table);
        /// <summary>
        /// Get an items from persistent store
        /// </summary>
        /// <param name="keys">The collection of keys to get from cache</param>
        /// <returns></returns>
        IDictionary<string, ProviderItemBase> Get(ICollection<string> keys);

        /// <summary>
        /// Get an item from persistent store
        /// </summary>
        /// <param name="hint">use to load data on the basis of hint from multiple nodes</param>
        /// <returns></returns>
        IDictionary<string, ProviderItemBase> GetAll(string hint=null);

        /// <summary>
        /// Remove from database on removing from cache
        /// </summary>
        /// <param name="keys">The collection of keys to be removed from Persistent store</param>
        IDictionary<string, ProviderItemBase> Remove(ICollection<string> keys);      
    }
}
