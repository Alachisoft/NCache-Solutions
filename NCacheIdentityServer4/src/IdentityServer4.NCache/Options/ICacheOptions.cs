namespace IdentityServer4.NCache.Options
{
    public class ICacheOptions: OptionsBase
    {
        public double DurationOfBreakInSeconds { get; set; } = 30;
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 1;
        public CacheExpirationType Expiration { get; set; } =
            CacheExpirationType.Absolute;
    }

    public enum CacheExpirationType
    {
        Absolute = 0,
        Sliding = 1
    }
}
