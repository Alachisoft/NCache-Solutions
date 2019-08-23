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
using System.Collections.Generic;
using System.Text;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache
{
    public class DefaultNCachePersistentConnection : INCachePersistantConnection
    {
        bool _disposed;

        private readonly string _cacheID;
        private readonly string _topicID;
        private readonly IEnumerable<string> _ipAddresses;
        private readonly ILogger<DefaultNCachePersistentConnection> _logger;
        private readonly int _retryCount;
        private readonly bool _enableClientLogs;
        private ITopic _topic;
        private ICache _cache;

        public DefaultNCachePersistentConnection(
            string cacheID,
            string topicID,
            ILogger<DefaultNCachePersistentConnection> logger,
            IEnumerable<string> ipAddresses,
            bool enableClientLogs = false,
            int retryCount = 5)
        {
            _cacheID = cacheID ?? throw new ArgumentNullException(nameof(cacheID));
            _topicID = topicID ?? "IntegrationEvents";
            _ipAddresses = ipAddresses ?? throw new ArgumentNullException(nameof(ipAddresses));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _enableClientLogs = enableClientLogs;
            _retryCount = retryCount;
            _topic = CreateModel();
        }

        private ICache CreateCache(string cacheID, bool enableClientLogs)
        {
            try
            {
                if (_cache == null)
                {
                    _logger.LogInformation($"Attempting to initialize {cacheID}");

                    List<ServerInfo> servers = new List<ServerInfo>();
                    foreach(var ipAddress in _ipAddresses)
                    {
                        servers.Add(new ServerInfo(ipAddress));
                    }

                    return CacheManager.GetCache(cacheID, new CacheConnectionOptions
                    {
                        ServerList = servers,
                        EnableClientLogs = enableClientLogs,
                        LogLevel = Alachisoft.NCache.Client.LogLevel.Debug,
                        ConnectionRetries = _retryCount,
                        EnableKeepAlive = true,
                        KeepAliveInterval = TimeSpan.FromSeconds(60)
                    });
                }

                return _cache;
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ITopic CreateModel()
        {
            try
            {
                if (_topic == null)
                {
                    _cache = CreateCache(_cacheID, _enableClientLogs);
                    _topic = _cache.MessagingService.GetTopic(_topicID);
                    if (_topic == null || _topic.IsClosed)
                    {
                        _topic = _cache.MessagingService.CreateTopic(_topicID);
                        _topic.MessageDeliveryFailure += _topic_MessageDeliveryFailure;
                    }
                }

                return _topic;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Unable to create topic due to following exception:");
                _logger.LogCritical($"ExceptionDetails:\n{e}");
                return null;
            }
        }

        private void _topic_MessageDeliveryFailure(object sender, MessageFailedEventArgs args)
        {
            var reasonForFailure = args.MessgeFailureReason.ToString();
            var messageID = args.Message.MessageId;
            var topicName = args.TopicName;

            _logger.LogError($"Message {messageID} could not be delivered on topic {topicName} for reason:{reasonForFailure}");
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
        }

        public string GetCacheID()
        {
            return _cacheID;
        }
    }
}
