using System;
using System.Runtime.Serialization;

#pragma warning disable RCS1194 // Implement exception constructors.
namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// Indicates an issue communicating with redis
    /// </summary>
    [Serializable]
    public partial class RedisException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="RedisException"/>.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public RedisException(string message) : base(message) { }

        /// <summary>
        /// Creates a new <see cref="RedisException"/>.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public RedisException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Deserialization constructor; not intended for general usage
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="ctx">Serialization context.</param>
        protected RedisException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }

    /// <summary>
    /// Indicates an exception raised by a redis server
    /// </summary>
    [Serializable]
    public sealed partial class RedisServerException : RedisException
    {
        /// <summary>
        /// Creates a new <see cref="RedisServerException"/>.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public RedisServerException(string message) : base(message) { }

        private RedisServerException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
    }
}