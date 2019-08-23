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
using Autofac;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache
{
    public class EventBusNCache : IEventBus
    {
        private readonly INCachePersistantConnection _persistantConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILogger<EventBusNCache> _logger;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "eshop_ncache_event_bus";
        private readonly string _subscriptionName;
        private IDurableTopicSubscription _subscription;

        public EventBusNCache(
            INCachePersistantConnection persistantConnection,
            IEventBusSubscriptionsManager subsManager,
            ILogger<EventBusNCache> logger,
            ILifetimeScope autofac,
            string subscriptionName = null)
        {
            _persistantConnection = persistantConnection ?? throw new ArgumentNullException(nameof(persistantConnection));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _autofac = autofac;
            _subscriptionName = subscriptionName;
        }

        public void Publish(IntegrationEvent @event)
        {
            try
            {
                var eventName = @event.GetType().Name;

                _logger.LogTrace($"Attempting NCache channel to publish event:{@event.Id} ({eventName})");
                var topic = _persistantConnection.CreateModel();

                if (topic == null)
                {
                    _logger.LogError($"Unable to get topic to publish event {@event.Id} ({eventName})");
                    return;
                }

                _logger.LogTrace($"Successfully acquired NCache channel to publish {@event.Id} ({eventName})");

                string body = JsonConvert.SerializeObject(@event);

                var payLoad = Tuple.Create(eventName, body);

                Message message = new Message(payLoad);

                _logger.LogTrace($"Attempting to publish {@event.Id} ({eventName})");

                topic.Publish(message, DeliveryOption.All, true);

                _logger.LogTrace($"Successfully published message {message.MessageId} to NCache Event Bus");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            DoInternalSubscription(eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());
            _subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());
            DoInternalSubscription(eventName);

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }


        public void Dispose()
        {
            if (_persistantConnection != null)
            {
                _persistantConnection.Dispose();
            }

            _subsManager.Clear();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);

            if (!containsKey)
            {
                if (_subscription == null)
                {
                    var topic = _persistantConnection.CreateModel();

                    if (topic == null)
                    {
                        _logger.LogError($"Unable to subscribe to event {eventName} due to topic creation issues");
						return;
                    }

                    _subscription = topic.CreateDurableSubscription(
                        _subscriptionName,
                        SubscriptionPolicy.Shared,
                        (o, args) =>
                        {
                            var payLoad = args.Message.Payload as Tuple<string, string>;
                            var eventName1 = payLoad.Item1;
                            var message = payLoad.Item2;

                            ProcessEvent(eventName1, message).Wait();
                        });
                }
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing NCache PubSub event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for NCache Pub Sub event: {EventName}", eventName);
            }
        }
        
    }
}
