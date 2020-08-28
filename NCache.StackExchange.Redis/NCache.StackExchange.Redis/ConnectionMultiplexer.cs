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
