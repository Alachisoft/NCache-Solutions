using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Caching
{
    public class NCacheCachingCorsPolicyService<TCorsPolicyService> : ICorsPolicyService
        where TCorsPolicyService : ICorsPolicyService
    {
        private readonly string _keyPrefix = typeof(TCorsPolicyService).FullName;
        private readonly TCorsPolicyService _inner;
        private readonly ILogger<NCacheCachingCorsPolicyService<TCorsPolicyService>> _logger;
        private readonly IdentityServerOptions _options;

        private readonly ICache<IsOriginAllowed> _cache;

        public NCacheCachingCorsPolicyService(
            TCorsPolicyService inner,
            ILogger<NCacheCachingCorsPolicyService<TCorsPolicyService>> logger,
            ICache<IsOriginAllowed> cache,
            IdentityServerOptions options)
        {
            _inner = inner;
            _logger = logger;
            _cache = cache;
            _options = options;
        }
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            string cacheKey = GetKey(origin);

            var isOriginAllowed = await _cache.GetAsync(cacheKey);

            if (isOriginAllowed != null)
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        $"Result received from NCache. Origin allowed:" +
                        $"{isOriginAllowed.OriginAllowed}");
                }
                return isOriginAllowed.OriginAllowed;
            }
            else
            {
                bool dummy = await _inner.IsOriginAllowedAsync(origin);
                isOriginAllowed = new IsOriginAllowed
                {
                    OriginAllowed = dummy
                };
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        $"Result received from Inner CorsPolicyService. Origin allowed:" +
                        $"{isOriginAllowed.OriginAllowed}");
                }

                await _cache.SetAsync(
                    cacheKey, 
                    isOriginAllowed, 
                    _options.Caching.CorsExpiration);

                return dummy;
            }
        }

        private string GetKey(string origin)
        {
            return $"{_keyPrefix}:{origin}";
        }
    }
    public class IsOriginAllowed
    {
        public bool OriginAllowed { get; set; }
    }
}
