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

namespace NCache.StackExchange.Redis
{
    public class ChannelMessage
    {
        /// <summary>
        /// See Object.ToString
        /// </summary>
        public override string ToString() => ((string)Channel) + ":" + ((string)Message);

        /// <summary>
        /// See Object.GetHashCode
        /// </summary>
        public override int GetHashCode() => Channel.GetHashCode() ^ Message.GetHashCode();

        /// <summary>
        /// See Object.Equals
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare.</param>
        public override bool Equals(object obj) => obj is ChannelMessage cm
            && cm.Channel == Channel && cm.Message == Message;
        public ChannelMessage(ChannelMessageQueue queue, in RedisChannel channel, in RedisValue value)
        {
            _queue = queue;
            Channel = channel;
            Message = value;
        }

        /// <summary>
        /// The channel that the subscription was created from
        /// </summary>
        public RedisChannel SubscriptionChannel => _queue.Channel;

        private ChannelMessageQueue _queue;

        /// <summary>
        /// The channel that the message was broadcast to
        /// </summary>
        public RedisChannel Channel { get; }
        /// <summary>
        /// The value that was broadcast
        /// </summary>
        public RedisValue Message { get; }
    }

    /// <summary>
    /// Represents a message queue of ordered pub/sub notifications
    /// </summary>
    /// <remarks>To create a ChannelMessageQueue, use ISubscriber.Subscribe[Async](RedisKey)</remarks>
}
