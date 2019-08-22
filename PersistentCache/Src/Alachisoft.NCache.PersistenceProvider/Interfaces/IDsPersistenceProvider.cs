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
    /// Datastructures operations to be persist using this interface
    /// </summary>
    public interface IDsPersistenceProvider
    {
        /// <summary>
        /// Ad to data type
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="items"></param>
        void AddToDataType(string key, ProviderItemBase items, DistributedDataType dataType);
        /// <summary>
        /// Remove an item from DataType
        /// </summary>
        /// <param name="keys"></param>
        void RemoveFromDataType(string keys, DistributedDataType dataType);
        /// <summary>
        /// Update something from DataType
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="items"></param>
        void UpdateToDataType(string key, ProviderItemBase items, DistributedDataType dataType);
    }
}
