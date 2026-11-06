using Alachisoft.NCache.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IdentityServer4.NCache.Options
{
    /// <summary>
    /// Optional, advanced connection settings. This is a thin wrapper around
    /// <see cref="Alachisoft.NCache.Client.ConnectionOptions"/>, which replaces the
    /// deprecated CacheConnectionOptions class in NCache 5.3.
    /// For the common case, leave it unset and give the server and port of the
    /// cache in the "ncache" section of appsettings.json.
    /// </summary>
    /// <remarks>
    /// Timeouts, retries, keep-alive, load balancing, client cache sync mode and
    /// client logging are no longer client-side options in NCache 5.3; they are
    /// configured in the server-side cache configuration.
    /// </remarks>
    public class NCacheConnectionOptions
    {
        /// <summary>
        /// Additional cache servers to connect to.
        /// </summary>
        public IList<NCacheServerInfo> ServerList { get; set; }

        public string AppName { get; set; }

        /// <summary>
        /// Maps to <see cref="Alachisoft.NCache.Client.ConnectionOptions.BindToIP"/>.
        /// </summary>
        public string ClientBindIP { get; set; }

        /// <summary>
        /// <see cref="NCacheIsolationLevel.InProc"/> maps to
        /// <see cref="Alachisoft.NCache.Client.ConnectionOptions.IsInproc"/> = true;
        /// anything else connects out-of-process (thin client).
        /// </summary>
        public NCacheIsolationLevel? Mode { get; set; }

        /// <summary>
        /// Maps to <see cref="Alachisoft.NCache.Client.ConnectionOptions.Security"/>.
        /// </summary>
        public NCacheCredentials UserCredentials { get; set; }

        internal ConnectionOptions ToConnectionOptions()
        {
            var connectionOptions = new ConnectionOptions
            {
                Servers = new List<ServerInfo>()
            };

            if (ServerList != null)
            {
                foreach (var server in ServerList.Where(x => x != null))
                {
                    connectionOptions.Servers.Add(server.serverInfo);
                }
            }

            if (!string.IsNullOrWhiteSpace(AppName))
            {
                connectionOptions.AppName = AppName;
            }

            if (!string.IsNullOrWhiteSpace(ClientBindIP))
            {
                connectionOptions.BindToIP = ClientBindIP;
            }

            if (Mode.HasValue)
            {
                connectionOptions.IsInproc = Mode.Value == NCacheIsolationLevel.InProc;
            }

            if (UserCredentials != null)
            {
                connectionOptions.Security = SecurityOptions.CreateWithCredentials(
                                                    UserCredentials.UserId,
                                                    UserCredentials.Password);
            }

            return connectionOptions;
        }
    }

    public class NCacheCredentials
    {
        internal string UserId { get; }
        internal string Password { get; }

        public NCacheCredentials(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }
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
}
