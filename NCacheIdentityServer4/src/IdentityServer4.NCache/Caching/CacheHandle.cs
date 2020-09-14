using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Exceptions;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Utils;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
using System;

namespace IdentityServer4.NCache.Caching
{
    public class CacheHandle
    {
        internal readonly ICacheOptions options;
        internal readonly ICache cache;

        internal readonly AsyncPolicyWrap<string> getFallBack;
        internal readonly AsyncPolicyWrap<bool> setFallBack;

        private readonly ILogger<CacheHandle> _logger;

        internal const string CACHE_PROBLEMS = "ICache<T> Cache Problems";

        public CacheHandle(
            ICacheOptions options,
            ILogger<CacheHandle> logger)
        {
            this.options = options ?? 
                throw new ArgumentNullException(nameof(options));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            cache = Utilities.CreateCache(options);
            
            var circuitBreaker = Utilities
                .GetCircuitBreakerPolicy(
                    options,
                    logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug),
                    logger,
                    $"ICache<T> is open circuited.\n " +
                        $"Duration: {options.DurationOfBreakInSeconds}",
                    $"ICache<T> circuit breaker is reset",
                    $"ICache<T> circuit breaker is half-open");

            getFallBack = GetFallBackPolicy(circuitBreaker);
            setFallBack = SetFallBackPolicy(circuitBreaker);
        }

        private AsyncPolicyWrap<bool> SetFallBackPolicy(
            AsyncCircuitBreakerPolicy circuitBreakerPolicy)
        {
            return Policy<bool>
                .Handle<BrokenCircuitException>()
                .Or<CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE
                )
                .FallbackAsync(false)
                .WrapAsync(circuitBreakerPolicy);
        }
        private AsyncPolicyWrap<string> GetFallBackPolicy(
                AsyncCircuitBreakerPolicy circuitBreakerPolicy)
        {
            return Policy<string>
                .Handle<BrokenCircuitException>()
                .Or<CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE
                )
                .FallbackAsync(CACHE_PROBLEMS)
                .WrapAsync(circuitBreakerPolicy);
        }
    }
}
