using Alachisoft.NCache.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis
{
    public interface ISubscription
    {
        ChannelMessageQueue Subscribe(RedisChannel topic);
        void UnSubscribe(ChannelMessageQueue channel);
    }
}
