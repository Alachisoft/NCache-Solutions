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

        /// <summary>
        /// Connects to the cache. NCache reads the server and port of the cache from the
        /// "ncache" section of the application's appsettings.json (or App.config / Web.config).
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        public static ConnectionMultiplexer Connect(string cacheName)
        {
            return new ConnectionMultiplexer(cacheName);
        }

        /// <summary>
        /// Connects to the cache with additional connection options.
        /// </summary>
        public static ConnectionMultiplexer Connect(string cacheName, ConnectionOptions connectionOptions)
        {
            return new ConnectionMultiplexer(cacheName, connectionOptions);
        }

        private ICache _cache;

        public ConnectionMultiplexer(string cacheName)
            : this(cacheName, new ConnectionOptions())
        {
        }

        public ConnectionMultiplexer(string cacheName, ConnectionOptions connectionOptions)
        {
            if (string.IsNullOrWhiteSpace(cacheName))
                throw new ArgumentException("Cache name cannot be null or empty.", nameof(cacheName));
            if (connectionOptions == null)
                throw new ArgumentNullException(nameof(connectionOptions));

            var key = cacheName.ToLower();
            if (!CacheConnections.ContainsKey(key))
            {
                var cache = CacheManager.GetCache(cacheName, connectionOptions);
                CacheConnections.Add(key, cache);
            }
            _cache = CacheConnections[key];
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
            return CacheConnections[cacheName.ToLower()];
        }
    }
    

    
}
