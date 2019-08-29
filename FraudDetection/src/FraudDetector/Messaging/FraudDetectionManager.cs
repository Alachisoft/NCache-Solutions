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
using FraudDetector.NCacheLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDetector.Messaging
{
    /// <summary>
    ///This calss contains main logicc of performing on transaction requests. 
    ///it creates a request against every transaction and verifies if transaction is valid or not 
    ///than publish these results back to transaction manager
    /// </summary>
    public class FraudDetectionManager : BaseManager
    {
        private readonly CacheHandler _cache;
        private readonly RequestValidator _trasactionValidator;
        private IDurableTopicSubscription _subscriptions = null;
       // initialize insatance of fraud requests, Trained logic lies in fraud request module
        public FraudDetectionManager(CacheHandler cache):base(cache)
        {
            _cache = cache;
            _trasactionValidator = new RequestValidator(cache);
        }

        public void Initiliaze()
        {
            // initialize the required objects for fraud manager.
            //cretes two topics for transactions and initializes the subscriptions accordingly
            try
            {
                CreateRelaventTopics(Topics.REPLIESTOPICS);
                CreateRelaventTopics(Topics.TRANSACTIONTOPICS);
                CreateRelevantSubscribtion();
            }
            catch
            {
                throw;
            }
        }

        public override void OnRequest(MessageEventArgs messageArgs)
        {
            // sends request to fraud requests class and on receiving result, publish them back to transaction manager
            if (messageArgs.TopicName.Equals(Topics.TRANSACTIONTOPICS, StringComparison.InvariantCultureIgnoreCase))
            {
                if (messageArgs != null && messageArgs.Message != null)
                {
                    if (messageArgs.Message.Payload is TransactionRequest transactionMessage)
                    {
                        Result result = _trasactionValidator.PerformTransaction(transactionMessage);
                        PublishResult(result, transactionMessage);
                    }
                }
            }
        }

        public void Dispose()
        {
            //deletes the subscription
            try
            {
                if (_subscriptions != null)
                {
                    _subscriptions.UnSubscribe();
                }
                _cache.DeleteTopic(Topics.REPLIESTOPICS);
                _cache.DeleteTopic(Topics.TRANSACTIONTOPICS);

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
                // initializes the relevant subscribers with their call backs
                MessageReceivedCallback fraudManagermessageReceivedCallback = new TransactionStartedMessage(this).MessageReceivedCallback;
                _subscriptions = CreateRelevantSubscribtions(Topics.TRANSACTIONTOPICS, fraudManagermessageReceivedCallback);
            }
            catch
            {
                throw;
            }
        }

        private void PublishResult(Result result, TransactionRequest transaction)
        {
            // publish messages back to all subscriptions of relative topic
            try
            {
                TransactionResult transactionResult = new TransactionResult(transaction.CustomerID, transaction.CustomerName, result,transaction.TransactionID);
                _cache.PublishMessageOnTopic(Topics.REPLIESTOPICS, transactionResult, null);
            }
            catch
            {
                // handle accordingly
            }
        }


    }
}
