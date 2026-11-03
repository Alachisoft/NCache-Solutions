namespace IdentityServer4.NCache.Options
{
    public abstract class OptionsBase
    {
        /// <summary>
        /// Name of the NCache cache to connect to (required).
        /// </summary>
        public virtual string CacheId { get; set; } =
            "default";

        /// <summary>
        /// IP address or host name of the NCache server the client connects to
        /// (required unless servers are supplied through
        /// <see cref="NCacheConnectionOptions.ServerList"/>).
        /// </summary>
        public virtual string CacheServer { get; set; } =
            null;

        /// <summary>
        /// Client port of the NCache server. Defaults to 9800.
        /// </summary>
        public virtual int CachePort { get; set; } =
            9800;

        /// <summary>
        /// Optional, advanced connection settings.
        /// </summary>
        public virtual NCacheConnectionOptions ConnectionOptions { get; set; } =
            null;
    }
}
