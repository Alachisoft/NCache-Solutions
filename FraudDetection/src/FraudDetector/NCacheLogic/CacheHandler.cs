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
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Common;
using CustomerSample;

using System;

namespace FraudDetector.NCacheLogic
{
    public class CacheHandler :CacheBase
    {
        // add a customer info in cache 
        public void AddtCustomerInCache (string key, DataTypeAttributes attributes, FraudRequest customer)
        {
            try
            {
                // create a list with write behind option
                // so whenever a cahcnge happen in data it is upadated back to data source
                WriteThruOptions options = new WriteThruOptions(WriteMode.WriteBehind);
                IDistributedList<FraudRequest> list = cache.DataTypeManager.CreateList<FraudRequest>(key, attributes);
                list.WriteThruOptions = options;
                list.Add(customer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void UpdateCustomerInfoInCache(string key, FraudRequest cutomerInfo)
        {
            try
            {
                // update info of a customer aginst its id
                WriteThruOptions options = new WriteThruOptions(WriteMode.WriteBehind);
                IDistributedList<FraudRequest> list = cache.DataTypeManager.GetList<FraudRequest>(key);
                if (list == null)
                    AddtCustomerInCache(key, null, cutomerInfo);
                else
                {
                    list.WriteThruOptions = options;
                    list.Add(cutomerInfo);
                }
            }
            catch
            {
                throw;
            }
        }
        
        public void RemoveCustomerInfofromCache(string key, CacheItem cacheItem)
        {
            // deleted entire info of a customer from database and cache
            try
            {
                WriteThruOptions options = new WriteThruOptions(WriteMode.WriteBehind);
                cache.DataTypeManager.Remove(key, writeThruOptions:options);
            }
            catch
            {
                throw;
            }
        }
        public IDistributedList<FraudRequest> FetchDataFromCache(string key)
        {
            try
            {
                // gets data from cache against a customer
                if (cache.Contains(key))
                {
                    ReadThruOptions options = new ReadThruOptions(ReadMode.ReadThru);
                    IDistributedList<FraudRequest> list;

                    list = cache.DataTypeManager.GetList<FraudRequest>(key, options);
                    return list;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        public IDistributedList<string> CreateListInCache (string key)
        {
            try
            {

                IDistributedList<string> list = cache.DataTypeManager.CreateList<string>(key);
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateTrainedDataInCache(string key, string traineddata)
        {
            try
            {
                IDistributedList<string> list = null;
                if (cache.Contains(key))
                    list = cache.DataTypeManager.GetList<string>(key);
                else 
                    list= CreateListInCache(key);

                list.Add(traineddata);
            }
            catch
            {
                //throw;
            }
        }

        public IDistributedList<string> GetTrainedDataFromCache(string key)
        {
            try
            {
                // gets data from cache against a customer
                if (cache.Contains(key))
                {
                    IDistributedList<string> list;
                    list = cache.DataTypeManager.GetList<string>(key);
                    return list;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
    }
}
