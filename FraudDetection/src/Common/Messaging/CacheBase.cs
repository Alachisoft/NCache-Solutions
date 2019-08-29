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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CacheBase
    {  
       // instance of cache
        protected ICache cache;
        // initialie a cache with respective options
        public ICache InitializeCache(string cacheName, CacheConnectionOptions cacheConnectionOptions = null)
        {
            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentNullException("Cache Name is empty.");
            try
            {
                cache = CacheManager.GetCache(cacheName, cacheConnectionOptions);
            }
            catch
            {
                throw;
            }
            return cache;
        }
        public void PublishMessageOnTopic(string topicName, object customer, TimeSpan? timeSpan)
        {
            try
            {
                //publishes messages on topic
                Message message = CreateMessage(customer, timeSpan);
                ITopic topic = null;
                if (!string.IsNullOrEmpty(topicName))
                    topic = GetTopic(topicName);
                if (topic == null)
                    throw new Exception("No such topic exist");
                topic.Publish(message, DeliveryOption.All);
            }
            catch
            {
                throw;
            }

        }
        public void CreateTopic(string topicName)
        {
            try
            {
                if (!string.IsNullOrEmpty(topicName))
                {
                    GetTopic(topicName);
                }
            }
            catch
            {
                throw;
            }
        }
        public ITopic GetTopic(string topicName)
        {
            try
            {
                // get topic form cache, if not present creates one
                ITopic topic = null;
                if (!string.IsNullOrEmpty(topicName))
                {
                    topic = cache.MessagingService.GetTopic(topicName);
                    if (topic == null)
                        topic = cache.MessagingService.CreateTopic(topicName);

                }
                return topic;
            }
            catch
            {
                throw;
            }
        }
        public Message CreateMessage(object messagePayload, TimeSpan? timeSpan)
        {
            // creates a message
            try
            {
                Message message = new Message(messagePayload, timeSpan);
                return message;
            }
            catch
            {
                throw;
            }
        }
        public IDurableTopicSubscription CreateSubscriptions(string topicName, MessageReceivedCallback messageReceivedCallback)
        {
            // cretews a subscription against a topic
            try
            {
                ITopic topic = GetTopic(topicName);
                return topic.CreateDurableSubscription(Guid.NewGuid().ToString(), SubscriptionPolicy.Exclusive, messageReceivedCallback, null);
            }
            catch
            {
                throw;
            }

        }
        public void Dispose()
        {
            if (cache != null)
                cache.Dispose();
        }
        public void DeleteTopic(string topicName)
        {
            try
            {
                ITopic topic = null;
                if (topicName != null)
                    topic = cache.MessagingService.GetTopic(topicName);
                if (topic != null)
                    cache.MessagingService.DeleteTopic(topicName);
            }
            catch
            {

            }
        }

    }
}
