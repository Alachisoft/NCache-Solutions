//Copyright(c) 2014 Stack Exchange

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

//===============================================

using Alachisoft.NCache.Client;
using Alachisoft.NCache.MapReduce;
using Alachisoft.NCache.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NCache.StackExchange.Redis
{
    public class ChannelMessageQueue
    {
        private ITopic _topic;
        private ITopicSubscription _topicSubscription;
        private Action<ChannelMessage> _handler;

        public RedisChannel Channel { get; set; }
        public ChannelMessageQueue(ICache cache, string topicName)
        {
            if (ReferenceEquals(cache, null))
                throw new ArgumentNullException(nameof(cache));

            _topic = cache.MessagingService.GetTopic(topicName);
        }

        public void OnMessage(Action<ChannelMessage> handler)
        {
            AssertOnMessage(handler);
            _handler = handler;

            _topicSubscription = _topic.CreateSubscription(MessageRecevied);
        }

        private void MessageRecevied(object sender, MessageEventArgs args)
        {
            Channel = new RedisChannel(args.TopicName, RedisChannel.PatternMode.Auto);
            var channelMessage = new ChannelMessage(null, Channel, RedisValue.Unbox(args.Message.Payload));
            _handler.Invoke(channelMessage);
        }

        private void AssertOnMessage(Action<ChannelMessage> handler)
        {
            if (_topic == null) throw new InvalidOperationException("Unable to subscribe when topic is null");
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (Interlocked.CompareExchange(ref _handler, handler, null) != null)
                throw new InvalidOperationException("Only a single " + nameof(OnMessage) + " is allowed");
        }

        /// <summary>
        /// Stop receiving messages on this channel.
        /// </summary>
        /// <param name="flags">The flags to use when unsubscribing.</param>
        public void Unsubscribe(CommandFlags flags = CommandFlags.None) 
        {
            _topicSubscription?.UnSubscribe();
            _handler = null;
            
        }

        /// <summary>
        /// Stop receiving messages on this channel.
        /// </summary>
        /// <param name="flags">The flags to use when unsubscribing.</param>
        public System.Threading.Tasks.Task UnsubscribeAsync(CommandFlags flags = CommandFlags.None)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                Unsubscribe(flags);
            });
        }
    }
}
