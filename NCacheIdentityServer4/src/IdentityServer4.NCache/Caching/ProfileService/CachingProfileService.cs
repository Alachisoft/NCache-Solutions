using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.NCache.Options;
using System;

namespace IdentityServer4.NCache.Caching
{
    public class CachingProfileService<TProfileService> : IProfileService
    where TProfileService : class, IProfileService
    {
        private readonly TProfileService inner;

        private readonly ICache<IsActiveContextCacheEntry> cache;

        private readonly ProfileServiceCachingOptions<TProfileService> options;

        private readonly ILogger<CachingProfileService<TProfileService>> logger;

        private readonly Func<IsActiveContext, string> _keySelector;

        private readonly string _keyPrefix;

        private readonly Func<IsActiveContext, bool> _shouldCache;

        private readonly TimeSpan _expiration;

        public CachingProfileService(
            TProfileService inner, 
            ICache<IsActiveContextCacheEntry> cache,
            ProfileServiceCachingOptions<TProfileService> options, 
            ILogger<CachingProfileService<TProfileService>> logger)
        {
            this.inner = inner;
            this.logger = logger;
            this.cache = cache;
            this.options = options;

            _keySelector = options.KeySelector;
            _keyPrefix = options.KeyPrefix;
            _shouldCache = options.ShouldCache;
            _expiration = options.Expiration;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await this.inner.GetProfileDataAsync(context);
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var key = $"{_keyPrefix}{_keySelector(context)}";

            if (_shouldCache(context))
            {
                var entry = await cache.GetAsync(key, _expiration,
                              async () =>
                              {
                                  await inner.IsActiveAsync(context);
                                  return new IsActiveContextCacheEntry 
                                  { IsActive = context.IsActive };
                              },
                              logger);

                context.IsActive = entry.IsActive;
            }
            else
            {
                await inner.IsActiveAsync(context);
            }
        }
    }
    public class IsActiveContextCacheEntry
    {
        public bool IsActive { get; set; }
    }
}
