using Alachisoft.NCache.Client;
using Alachisoft.NCache.Common.ErrorHandling;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Exceptions;
using IdentityServer4.Models;
using IdentityServer4.NCache.Options;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;

namespace IdentityServer4.NCache.Utils
{
    internal static class Utilities
    {
        internal static ICache CreateCache(OptionsBase options)
        {
            if (options.ConnectionOptions == null)
            {
                return CacheManager.GetCache(
                    options.CacheId);
            }
            else
            {
                return CacheManager.GetCache(options.CacheId, options.ConnectionOptions.CacheConnectionOptions);
            }
        }

        internal static string CreateCachedPersistedGrantStoreKey(
            string key)
        {
            return $"PersistedGrant-{key.Trim()}";
        }

        internal static string CreateCachedPersistedGrantStoreKey(
            PersistedGrant grant)
        {
            return CreateCachedPersistedGrantStoreKey(grant.Key);
        }
        internal static Tag CreateCachedPersistedGrantStoreSubjectIdTag(
            string subjectId)
        {
            return new Tag($"PersistedGrant-subjectId-{subjectId.Trim()}");
        }

        internal static Tag CreateCachedPersistedGrantStoreClientIdTag(
            string clientId)
        {
            return new Tag($"PersistedGrant-clientId-{clientId.Trim()}");
        }

        internal static Tag CreateCachedPersistedGrantStoreTypeTag(string type)
        {
            return new Tag($"PersistedGrant-type-{type.Trim()}");
        }

        internal static Tag[] CreateCachePersistedGrantStoreTags(
            PersistedGrant grant)
        {
            return new Tag[]
            {
                new Tag("PersistedGrant"),
                CreateCachedPersistedGrantStoreClientIdTag(grant.ClientId),
                CreateCachedPersistedGrantStoreSubjectIdTag(grant.SubjectId),
                CreateCachedPersistedGrantStoreTypeTag(grant.Type)
            };
        }

        internal static Tag[] CreateCachePersistedGrantStoreTags(
            string subjectId = null,
            string clientId = null,
            string type = null)
        {
            List<Tag> tagsList = new List<Tag>();

            if (!string.IsNullOrWhiteSpace(subjectId))
            {
                tagsList.Add(CreateCachedPersistedGrantStoreSubjectIdTag(subjectId));
            }
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                tagsList.Add(CreateCachedPersistedGrantStoreClientIdTag(clientId));
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                tagsList.Add(CreateCachedPersistedGrantStoreTypeTag(type));
            }

            return tagsList.ToArray();
        }

        internal static Expiration DetermineCachePersistedGrantStoreExpiration(
            PersistedGrant grant,
            PersistedStoreCachingOptions options)
        {
            if (options.UsePersistedGrantExpiration &&
                grant.Expiration.HasValue)
            {
                return new Expiration(
                    ExpirationType.Absolute, 
                    grant.Expiration.Value - DateTime.UtcNow);
            }
            else
            {
                ExpirationType expirationType =
                    options.Expiration ==               
                        CacheExpirationType.Absolute ?
                                                ExpirationType.Absolute :
                                                ExpirationType.Sliding;
                TimeSpan timeSpan = TimeSpan.FromSeconds
                                        (options.TimeSpanInSeconds);
                return new Expiration(expirationType, timeSpan);
            }
        }

        internal static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(
            ICacheOptions options,
            bool debugLoggingEnabled,
            ILogger logger,
            string onBreakMessage = " ",
            string onResetMessage = " ",
            string onHalfOpenMessage = " "
            )
        {
            return Policy
                    .Handle<CacheException>(ex =>
                        ex.ErrorCode == 
                                ErrorCodes.Common.NO_SERVER_AVAILABLE
                    )
                    .CircuitBreakerAsync(
                        exceptionsAllowedBeforeBreaking:
                            options.ExceptionsAllowedBeforeBreaking < 0 ?
                                1 :
                                options.ExceptionsAllowedBeforeBreaking,
                        durationOfBreak:
                            TimeSpan.FromSeconds(
                                options.DurationOfBreakInSeconds < 0 ? 
                                30 :
                                options.DurationOfBreakInSeconds),
                        onBreak: (ex, ts) =>
                        {
                            if (debugLoggingEnabled)
                            {
                                logger.LogDebug(
                                    $"{onBreakMessage}\n" +
                                    $"Reason:{ex.Message}");
                            }
                        },
                        onReset: () =>
                        {
                            if (debugLoggingEnabled)
                            {
                                logger.LogDebug(onResetMessage);
                            }
                        },
                        onHalfOpen: () =>
                        {
                            if (debugLoggingEnabled)
                            {
                                logger.LogDebug(onHalfOpenMessage);
                            }
                        });

        }
    }
}
