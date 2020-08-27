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
