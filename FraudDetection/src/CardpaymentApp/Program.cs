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
using System.Configuration;

namespace CardPaymentApp
{
    class Program
    {
        static void Main(string[] args)
        //This is main function to call  the further processing.
        {
            try
            {
                string cacheName = ConfigurationManager.AppSettings["CacheID"];

                if (string.IsNullOrEmpty(cacheName))
                {
                    Console.WriteLine("The CacheID cannot be null or empty.");
                    return;
                }

                string connstring = ConfigurationManager.AppSettings["ConnString"];

                if (string.IsNullOrEmpty(connstring))
                {
                    Console.WriteLine("The ConnString cannot be null or empty.");
                    return;
                }

                string cacheServer = ConfigurationManager.AppSettings["cacheServer"];
                if (string.IsNullOrWhiteSpace(cacheServer)
                    || !int.TryParse(ConfigurationManager.AppSettings["cachePort"], out int cachePort))
                    throw new ConfigurationErrorsException(
                        "App.config must define appSettings cacheServer and cachePort (thin-client connection to the cache server).");

                Console.WriteLine("CardPayment Application\n\n");
                TransactionApp transactionApp = new TransactionApp();
                transactionApp.Start(connstring, cacheName, cacheServer, cachePort);
                Console.WriteLine("Thank you...");
                transactionApp.Dispose();
                Console.ReadLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
