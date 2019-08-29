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
using FraudDetector.Messaging;
using FraudDetector.NCacheLogic;
using System;

namespace FraudDetector
{
    public  class FraudDetection
    {
        private CacheHandler _cache = null;
        private FraudDetectionManager _fraudManger= null;
        public void StartDetection(string cacheName)
        {
            try
            {
                _cache = InitiliazeCache(cacheName, null);
                // initialize the respective manager

                // transacion manager is required to give input of the customer where as fraud manager has the logic of completeing 
                // the transaction and publishing result
                _fraudManger = InitiliazeFraudManager(_cache);

                Console.WriteLine("Press any key to stop fraud manager...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private FraudDetectionManager InitiliazeFraudManager(CacheHandler cache)
        {
            FraudDetectionManager fraudMnager = new FraudDetectionManager(cache);
            fraudMnager.Initiliaze();
            return fraudMnager;
        }

        private CacheHandler InitiliazeCache(string cacheName, CacheConnectionOptions cacheConnectionOptions)
        {
            CacheHandler cache = new CacheHandler();
            cache.InitializeCache(cacheName, cacheConnectionOptions);
            return cache;
        }


        public void Dispose()
        {
            if (_fraudManger != null)
                _fraudManger.Dispose();
            if (_cache != null)
                _cache.Dispose();
        }
    }
}
