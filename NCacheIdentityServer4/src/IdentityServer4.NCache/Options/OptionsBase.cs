namespace IdentityServer4.NCache.Options
{
    public abstract class OptionsBase
    {
        /// <summary>
        /// Name of the NCache cache to connect to (required). The server and port of the
        /// cache are read by NCache from the "ncache" section of appsettings.json.
        /// </summary>
        public virtual string CacheId { get; set; } =
            "default";

        /// <summary>
        /// Optional, advanced connection settings.
        /// </summary>
        public virtual NCacheConnectionOptions ConnectionOptions { get; set; } =
            null;
    }
}
