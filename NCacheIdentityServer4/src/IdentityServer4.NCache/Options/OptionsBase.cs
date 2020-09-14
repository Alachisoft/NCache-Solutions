namespace IdentityServer4.NCache.Options
{
    public abstract class OptionsBase
    {
        public virtual string CacheId { get; set; } = 
            "default";
        public virtual NCacheConnectionOptions ConnectionOptions { get; set; } = 
            null;
    }
}
