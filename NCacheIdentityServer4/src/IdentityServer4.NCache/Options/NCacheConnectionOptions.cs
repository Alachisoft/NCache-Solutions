using Alachisoft.NCache.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IdentityServer4.NCache.Options
{
    public class NCacheConnectionOptions
    {
        internal readonly CacheConnectionOptions CacheConnectionOptions =
                                  new CacheConnectionOptions
                                  { 
                                      // These properties not in use anyway
                                      DefaultReadThruProvider = "",
                                      DefaultWriteThruProvider = "" 
                                  };
        public TimeSpan? ConnectionTimeout
        { 
            get
            {
                return CacheConnectionOptions.ConnectionTimeout;
            }
            set
            {
                CacheConnectionOptions.ConnectionTimeout = value;
            }
        }

        public NCacheCredentials UserCredentials
        {
            get
            {
                if (CacheConnectionOptions.UserCredentials != null)
                {
                    return new NCacheCredentials(CacheConnectionOptions.UserCredentials); 
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    CacheConnectionOptions.UserCredentials =
                                new Credentials(value.credentials.UserID,                   
                                                value.credentials.Password); 
                }
            }
        }
        public TimeSpan? KeepAliveInterval
        {
            get
            {
                return CacheConnectionOptions.KeepAliveInterval;
            }
            set
            {
                CacheConnectionOptions.KeepAliveInterval = value;
            }
        }

        public bool? EnableKeepAlive
        {
            get
            {
                return CacheConnectionOptions.EnableKeepAlive;
            }
            set
            {
                CacheConnectionOptions.EnableKeepAlive = value;
            }
        }

        public TimeSpan? CommandRetryInterval
        {
            get
            {
                return CacheConnectionOptions.CommandRetryInterval;
            }
            set
            {
                CacheConnectionOptions.CommandRetryInterval = value;
            }
        }
        public int? CommandRetries
        {
            get
            {
                return CacheConnectionOptions.CommandRetries;
            }
            set
            {
                CacheConnectionOptions.CommandRetries = value;
            }
        }
        public NCacheClientCacheSyncMode? ClientCacheMode
        {
            get
            {
                if (CacheConnectionOptions.ClientCacheMode.HasValue)
                {
                    return CacheConnectionOptions.ClientCacheMode.Value ==
                                ClientCacheSyncMode.Optimistic ?
                                    NCacheClientCacheSyncMode.Optimistic :
                                    NCacheClientCacheSyncMode.Pessimistic; 
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    CacheConnectionOptions.ClientCacheMode =
                                value == NCacheClientCacheSyncMode.Optimistic ?
                                    ClientCacheSyncMode.Optimistic :
                                    ClientCacheSyncMode.Pessimistic; 
                }
            }
        }
        public TimeSpan? RetryConnectionDelay
        {
            get
            {
                return CacheConnectionOptions.RetryConnectionDelay;
            }
            set
            {
                CacheConnectionOptions.RetryConnectionDelay = value;
            }
        }
        public TimeSpan? RetryInterval
        {
            get
            {
                return CacheConnectionOptions.RetryInterval;
            }
            set
            {
                CacheConnectionOptions.RetryInterval = value;
            }
        }
        public int? ConnectionRetries
        {
            get
            {
                return CacheConnectionOptions.ConnectionRetries;
            }
            set
            {
                CacheConnectionOptions.ConnectionRetries = value;
            }
        }
        public bool? EnableClientLogs
        {
            get
            {
                return CacheConnectionOptions.EnableClientLogs;
            }
            set
            {
                CacheConnectionOptions.EnableClientLogs = value;
            }
        }
        public TimeSpan? ClientRequestTimeOut
        {
            get
            {
                return CacheConnectionOptions.ClientRequestTimeOut;
            }
            set
            {
                CacheConnectionOptions.ClientRequestTimeOut = value;
            }
        }
        public bool? LoadBalance
        {
            get
            {
                return CacheConnectionOptions.LoadBalance;
            }
            set
            {
                CacheConnectionOptions.LoadBalance = value;
            }
        }
        public string AppName
        {
            get
            {
                return CacheConnectionOptions.AppName;
            }
            set
            {
                CacheConnectionOptions.AppName = value;
            }
        }
        public string ClientBindIP
        {
            get
            {
                return CacheConnectionOptions.ClientBindIP;
            }
            set
            {
                CacheConnectionOptions.ClientBindIP = value;
            }
        }
        public NCacheIsolationLevel? Mode
        {
            get
            {
                switch(CacheConnectionOptions.Mode)
                {
                    case IsolationLevel.InProc:
                        {
                            return NCacheIsolationLevel.InProc;
                        }
                    case IsolationLevel.OutProc:
                        {
                            return NCacheIsolationLevel.OutProc;
                        }
                    default:
                        {
                            return NCacheIsolationLevel.Default;
                        }
                }
            }
            set
            {
                switch(value)
                {
                    case NCacheIsolationLevel.InProc:
                        {
                            CacheConnectionOptions.Mode = IsolationLevel.InProc;
                        }
                        break;
                    case NCacheIsolationLevel.OutProc:
                        {
                            CacheConnectionOptions.Mode = IsolationLevel.OutProc;
                        }
                        break;
                    default:
                        {
                            CacheConnectionOptions.Mode = IsolationLevel.Default;
                        }
                        break;
                }
            }
        }
        public IList<NCacheServerInfo> ServerList
        {
            get
            {
                if (CacheConnectionOptions.ServerList != null)
                {
                    return CacheConnectionOptions
                                    .ServerList
                                        .Select(x => new NCacheServerInfo(x))
                                            .ToList(); 
                }

                return null;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    CacheConnectionOptions.ServerList =
                                new List<ServerInfo>(value.Select(x => x.serverInfo)); 
                }
            }
        }
        public NCacheLogLevel? LogLevel
        {
            get
            {
                switch (CacheConnectionOptions.LogLevel)
                {
                    case Alachisoft.NCache.Client.LogLevel.Debug:
                        {
                            return NCacheLogLevel.Debug;
                        }
                    case Alachisoft.NCache.Client.LogLevel.Info:
                        {
                            return NCacheLogLevel.Info;
                        }
                    default:
                        {
                            return NCacheLogLevel.Error;
                        }
                }
            }
            set
            {
                switch (value)
                {
                    case NCacheLogLevel.Debug:
                        {
                            CacheConnectionOptions.LogLevel = 
                                Alachisoft.NCache.Client.LogLevel.Debug;
                        }
                        break;
                    case NCacheLogLevel.Info:
                        {
                            CacheConnectionOptions.LogLevel =
                                Alachisoft.NCache.Client.LogLevel.Info;
                        }
                        break;
                    default:
                        {
                            CacheConnectionOptions.LogLevel =
                                Alachisoft.NCache.Client.LogLevel.Error;
                        }
                        break;
                }
            }
        }
    }

    public class NCacheCredentials
    {
        internal Credentials credentials {get; set;}

        public NCacheCredentials(string userId, string password)
        {
            credentials = new Credentials(userId, password);
        }

        internal NCacheCredentials(Credentials credentials)
        {
            this.credentials = credentials;
        }

    }

    public enum NCacheClientCacheSyncMode
    {
        Pessimistic = 0,
        Optimistic = 1
    }

    public enum NCacheIsolationLevel
    {
        Default = 0,
        InProc = 1,
        OutProc = 2
    }

    public class NCacheServerInfo
    {
        internal ServerInfo serverInfo { get; private set; }

        public NCacheServerInfo(string name, int port = 9800)
        {
            serverInfo = new ServerInfo(name, port);
        }

        public NCacheServerInfo(IPAddress ip, int port = 9800)
        {
            serverInfo = new ServerInfo(ip, port);
        }

        internal NCacheServerInfo(ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
        }
    }

    public enum NCacheLogLevel
    {
        Info = 0,
        Error = 1,
        Debug = 2
    }
}
