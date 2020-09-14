namespace IdentityServer4.NCache.Options
{
    public class PersistedStoreCachingOptions:ICacheOptions
    {
        public double TimeSpanInSeconds { get; set; } = 600;
        public bool UsePersistedGrantExpiration { get; set; } = false;
    }
}
