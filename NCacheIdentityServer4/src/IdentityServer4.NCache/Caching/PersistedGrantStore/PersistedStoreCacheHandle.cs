using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Exceptions;
using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Utils;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
using System;
using System.Collections.Generic;

namespace IdentityServer4.NCache.Caching
{
    public class CachedPersistedStoreCacheHandle
    {
        internal readonly PersistedStoreCachingOptions options;
        internal readonly ICache cache;

        internal readonly AsyncPolicyWrap<bool> setFallBackPolicy;
        internal readonly AsyncPolicyWrap<Dictionary<string, PersistedGrant>>
            getPersistedGrantDictionaryPolicy;
        internal readonly AsyncPolicyWrap<PersistedGrant> getPersistedGrantPolicy;

        private readonly ILogger<CachedPersistedStoreCacheHandle> _logger;

        public CachedPersistedStoreCacheHandle(
            PersistedStoreCachingOptions options,
            ILogger<CachedPersistedStoreCacheHandle> logger)
        {
            this.options = options;
            cache = Utilities.CreateCache(options);
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            var circuitBreaker = Utilities
                .GetCircuitBreakerPolicy(
                    options,
                    logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug),
                    logger,
                    $"PersistedGrantStoreCache is open circuited.\n " +
                        $"Duration: {options.DurationOfBreakInSeconds}",
                    $"PersistedGrantStoreCache circuit breaker is reset",
                    $"PersistedGrantStoreCache circuit breaker is half-open");

            getPersistedGrantPolicy =
                GetFallBackPolicy<PersistedGrant>(circuitBreaker);
            getPersistedGrantDictionaryPolicy =
                GetFallBackPolicy<Dictionary<string,
                                        PersistedGrant>>(circuitBreaker);
            setFallBackPolicy =
                SetFallBackPolicy(circuitBreaker);
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
        private AsyncPolicyWrap<U> GetFallBackPolicy<U>(
                AsyncCircuitBreakerPolicy circuitBreakerPolicy) where U : class
        {
            return Policy<U>
                .Handle<BrokenCircuitException>()
                .Or<CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE
                )
                .FallbackAsync((U)null)
                .WrapAsync(circuitBreakerPolicy);
        }
    }
}
