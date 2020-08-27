using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Alachisoft.NCache.Client;

namespace NCache.StackExchange.Redis
{
    /// <summary>
    /// Represents an inter-related group of connections to redis servers
    /// </summary>
    public class ConnectionMultiplexer
    {
        public static Dictionary<string, ICache> CacheConnections = new Dictionary<string, ICache>();
        public static ConnectionMultiplexer Connect(string cacheName)
        {
            return new ConnectionMultiplexer(cacheName);
        }

        private ICache _cache;
        public ConnectionMultiplexer(string cacheName)
        {
            if (!CacheConnections.ContainsKey(cacheName.ToLower()))
            {
                var cache = CacheManager.GetCache(cacheName);
                CacheConnections.Add(cacheName.ToLower(), cache);
            }
            _cache = CacheConnections[cacheName];
        }

        public INCacheDatabase GetDatabase()
        {
            return new NCacheDatabase(_cache);
        }

        internal static void TraceWithoutContext(string message)
        {
            throw new NotImplementedException();
        }

        public ICache GetNCacheInterface(string cacheName)
        {
            return CacheConnections[cacheName];
        }
    }
    

    
}
