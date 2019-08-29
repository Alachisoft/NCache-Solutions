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
using CardPaymentApp.Messaging;
using Common;
using CustomerSample;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CardPaymentApp
{
    public class TransactionApp
    {
        private Dictionary<string, bool> transactionTable;
        private CacheBase transactionCache = null;
        private SqlDataSource dataSource;
        private TransactionRequestManager transactionManger;
        public void Start(string connectionString, string cacheName)
        {
            bool tryNextTransaction = true;
            transactionTable = new Dictionary<string, bool>();

            // initialize the cache 
            transactionCache = InitiliazeCache(cacheName, null);

            // initialize the datasource
            dataSource = new SqlDataSource();
            dataSource.Connect(connectionString);
            
            // initialize the respective manager
            // transacion manager is required to give input of the customer where as fraud manager has the logic of completeing 
            // the transaction and publishing result
            transactionManger = InitiliazeTransactionManager(transactionCache);

            // assing a  transaction completed event with transaction manager, so we will know if transaction has been completed. 

            transactionManger.PublishTransactionResult += PublishTransactionResult; // assign an event for transaction completed

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();

            do
            {
                try
                {
                    Console.WriteLine("\n");
                    //create a random customer instance 
                    Customer customer = GetCustomer(new Random().Next(111, 115));

                    Console.WriteLine($"Do you want to make Transaction of '${customer.CardInfo.TransactionAmount}' for " +
                        $"'{ customer.CardInfo.CardHolder}'? Press Y/yes to continue or N/No to cancel.");

                    string validTransaction = Console.ReadLine();

                    if (validTransaction.Equals("y", StringComparison.InvariantCultureIgnoreCase) || validTransaction.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        bool transactionInProress = true;

                        //perform transaction through manager
                        var transsactionID = transactionManger.TryTransaction(customer);

                        AddTransactionID(transsactionID);

                        while (transactionInProress) // wait till transaction is completed 
                        {
                            if (TransactionExist(transsactionID) && GetTransaction(transsactionID))
                            {
                               transactionInProress = false;
                            }
                            // wait for transaction completion
                        }

                        RemoveTransactionID(transsactionID);
                    }
                    Console.WriteLine("Do you want to make another Transaction? press Y or yes to continue.");

                    string input = Console.ReadLine();

                    tryNextTransaction = input.Equals("y", StringComparison.InvariantCultureIgnoreCase) || input.Equals("yes", StringComparison.InvariantCultureIgnoreCase);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            } while (tryNextTransaction);

        }

        private CacheBase InitiliazeCache(string cacheName, CacheConnectionOptions cacheConnectionOptions)
        {
            CacheBase cache = new CacheBase();
            cache.InitializeCache(cacheName, cacheConnectionOptions);
            return cache;
        }

        private TransactionRequestManager InitiliazeTransactionManager(CacheBase cache)
        {
            TransactionRequestManager transactionManger = new TransactionRequestManager(cache);
            transactionManger.Initiliaze();
            return transactionManger;
        }

        private void PublishTransactionResult(object sender, TransactionResultArgs transactionResult)
        {
            if (TransactionExist(transactionResult.TransactionID))
            {
                Console.WriteLine("");
                Console.WriteLine("Transaction Completed");
                Console.WriteLine($"Customer Name : {transactionResult.CustomerName}");
                Console.WriteLine($"Customer ID : {transactionResult.CustomerID}");
                Console.WriteLine($"TransactionResult : {transactionResult.TransactionResult}\n\n\n");

                UpdateTransaction(transactionResult.TransactionID);
            }
        }

        private Customer GetCustomer(int customerID)
        {
            // this data should be user input
            IList<long> associatedCards = dataSource.GetCardNumbers(customerID);
            Customer customer = dataSource.LoadCustomersCardInfo(associatedCards[0]);
            switch (customerID)
            {
                case 111:
                    customer.CardInfo.TransactionAmount = 15000;
                    customer.IPAdress = "20.200.20.34";
                    customer.EmailID = "abc1@abc.com";
                    customer.City = "Munich";
                    customer.Country = "Berlin";
                    break;
                case 112:
                    customer.CardInfo.TransactionAmount = 10000;
                    customer.IPAdress = "20.200.20.34";
                    customer.EmailID = "";
                    customer.City = "abc";
                    customer.Country = "Indonesia";
                    break;
                case 113:
                    customer.CardInfo.TransactionAmount = 5000;
                    customer.IPAdress = "172.30.35.40"; ;
                    customer.EmailID = "Mark@gmail.com";
                    customer.City = "Munich";
                    customer.Country = "Indonesia";
                    break;
                case 114:
                    customer.CardInfo.TransactionAmount = 1000;
                    customer.IPAdress = "20.200.20.34";
                    customer.EmailID = "abc@gmail.com";
                    customer.Country = "Yugoslavia";
                    break;
                case 115:
                    customer.CardInfo.TransactionAmount = 5000;
                    customer.IPAdress = "172.19.35.40";
                    customer.EmailID = "abcd@abc.com";
                    customer.City = "Munich";
                    break;
            }
            customer.CustomerID = customerID;
            return customer;
        }

        public void Dispose()
        {
            if (transactionManger != null)
                transactionManger.Dispose();
            if (transactionCache != null)
                transactionCache.Dispose();
            dataSource.DisConnect();
        }
        
        #region Handle Transactions Syncronizations

        private void AddTransactionID(string transactionID)
        {
            if (string.IsNullOrEmpty(transactionID))
                return;
            else
            {
                if (!transactionTable.ContainsKey(transactionID))
                    transactionTable.Add(transactionID, false);
            }

        }
        private void RemoveTransactionID(string transactionID)
        {
            if (string.IsNullOrEmpty(transactionID))
                return;
            else
            {
                if (transactionTable.ContainsKey(transactionID))
                    transactionTable.Remove(transactionID);
            }

        }
        private bool TransactionExist(string transactionID)
        {
            return transactionTable.ContainsKey(transactionID);
        }
        private void UpdateTransaction(string transactionID)
        {
            transactionTable[transactionID] = true;
        }

        private bool GetTransaction(string transactionID)
        {
            return transactionTable[transactionID];
        }
        #endregion
    }
}
