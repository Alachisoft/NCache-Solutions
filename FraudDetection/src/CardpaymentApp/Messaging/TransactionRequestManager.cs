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
using Common;
using CustomerSample;
using System;

namespace CardPaymentApp.Messaging
{
    /// <summary>
    /// This class contains transaction manager for client side. 
    /// For every transaction it creates a transaction message and publish it on Transaction topics
    ///These messages are recieve3d by fraud manager and resut is sent back to this class
    /// </summary>
    public class TransactionRequestManager : BaseManager
    {
        private readonly CacheBase _cache;
        
        private IDurableTopicSubscription transactionSubscription;

        public event EventHandler<TransactionResultArgs> PublishTransactionResult;

        public TransactionRequestManager(CacheBase cache) : base(cache)
        {
            //assign cache instance for transaction manager
            _cache = cache;
        }

        public void Initiliaze()
        {
            // initialize the required objects for fraud manager.
            //cretes two topics for transactions and initializes the subscriptions accordingly
            try
            {
                CreateRelaventTopics(Topics.TRANSACTIONTOPICS);
                CreateRelevantSubscribtion();
            }
            catch
            {
                throw;
            }
        }

        public string TryTransaction(Customer customer)
        {
            // sending messages on respective topic
            if (customer != null)
            {
                TransactionRequest transactionRequest = CreateTransactionFromCustomer(customer);
                _cache.PublishMessageOnTopic(Topics.TRANSACTIONTOPICS, transactionRequest, null);
                return transactionRequest.TransactionID ;
            }
            return null;
        }
       
        public override void OnRequest(MessageEventArgs messageArgs)
        {
            // recieve results back and send result back to client
            if (messageArgs.TopicName.Equals(Topics.REPLIESTOPICS, StringComparison.InvariantCultureIgnoreCase))
            {
                if (messageArgs != null && messageArgs.Message != null)
                {
                    if (messageArgs.Message.Payload is TransactionResult transactionResult)
                    {
                        if (PublishTransactionResult != null)
                        {
                            TransactionResultArgs transactionResultArgs = new TransactionResultArgs(transactionResult.Result, transactionResult.CustomerID, transactionResult.CustomerName, transactionResult.TransactionID);
                            PublishTransactionResult(this, transactionResultArgs);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            //deletes the subscription
            try
            {
                if (transactionSubscription != null)
                {
                    transactionSubscription.UnSubscribe();
                }
            }
            catch
            {
                // handle accordingly
            }
        }

        private void CreateRelevantSubscribtion()
        {
            try
            {
                MessageReceivedCallback transactonmessageReceivedCallback = new TransactionCompletedMessage(this).MessageReceivedCallback;
                transactionSubscription = CreateRelevantSubscribtions(Topics.REPLIESTOPICS, transactonmessageReceivedCallback);
            }
            catch
            {
                throw;
            }
        }

        private TransactionRequest CreateTransactionFromCustomer(Customer customer)
        {
            // creates a message from customer Info
            TransactionRequest transaction = new TransactionRequest
            {
                CustomerName = customer.CardInfo.CardHolder,
                CardNumber = customer.CardInfo.CardNumber,
                City = customer.City,
                Country = customer.Country,
                IPAdress = customer.IPAdress,
                EmailID = customer.EmailID,
                CustomerID = customer.CustomerID,
                TransactionAmount = customer.CardInfo.TransactionAmount,
                TransactionID = Guid.NewGuid().ToString()
            };
            return transaction;
        }
    }
}

