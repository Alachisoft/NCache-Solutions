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

namespace Alachisoft.NCache.PersistenceProvider
{
    /// <summary>
    /// Datastructures operations to be persist using this interface
    /// </summary>
    public interface IDsPersistenceProvider
    {
        /// <summary>
        /// Add to data type
        /// </summary>
        /// <param name="key">key to be added</param>
        /// <param name="item">item to be added against the key</param>
        void AddToDataType(string key, ProviderItemBase item, DistributedDataType dataType);
        /// <summary>
        /// Remove an item from DataType
        /// </summary>
        /// <param name="key">Key to be removed</param>
        void RemoveFromDataType(string key, DistributedDataType dataType);
        /// <summary>
        /// Update something from DataType
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="item">item to be updated against the key</param>
        void UpdateToDataType(string key, ProviderItemBase item, DistributedDataType dataType);
    }
}
