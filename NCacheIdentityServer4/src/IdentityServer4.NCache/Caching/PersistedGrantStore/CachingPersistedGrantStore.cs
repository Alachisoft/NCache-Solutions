using Alachisoft.NCache.Client;
using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Utils;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Caching
{
    public class CachingPersistedGrantStore<T> : IPersistedGrantStore
        where T : IPersistedGrantStore
    {
        private readonly CachedPersistedStoreCacheHandle _handle;
        private readonly IPersistedGrantStore _inner;
        private readonly ILogger _logger;
        private readonly bool _debugLoggingEnabled;
        private readonly bool _errorLoggingEnabled;

        private readonly AsyncPolicyWrap<bool> _setFallBackPolicy;
        private readonly AsyncPolicyWrap<Dictionary<string, PersistedGrant>>
            _getPersistedGrantDictionaryPolicy;
        private readonly AsyncPolicyWrap<PersistedGrant> _getPersistedGrantPolicy;

        public CachingPersistedGrantStore(
            CachedPersistedStoreCacheHandle handle,
            T inner,
            ILogger<CachingPersistedGrantStore<T>> logger)
        {
            _handle = handle ?? throw new ArgumentNullException(nameof(handle));
            _inner = inner;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _debugLoggingEnabled =
                logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug);
            _errorLoggingEnabled =
                logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error);

            _getPersistedGrantPolicy = handle.getPersistedGrantPolicy;
            _getPersistedGrantDictionaryPolicy =
                handle.getPersistedGrantDictionaryPolicy;
            _setFallBackPolicy =
                handle.setFallBackPolicy;
        }


        public async Task<IEnumerable<Models.PersistedGrant>> GetAllAsync(
            string subjectId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subjectId))
                {
                    throw new ArgumentNullException(
                        "Subject id cant be null or white space", nameof(subjectId));
                }

                var dictionary = await _getPersistedGrantDictionaryPolicy
                    .ExecuteAsync(async () =>
                    {
                        return await Task.Run(() =>
                            new Dictionary<string, PersistedGrant>(
                                _handle.cache.SearchService.GetByTag<PersistedGrant>(
                                    Utilities
                                        .CreateCachedPersistedGrantStoreSubjectIdTag(
                                                    subjectId)))
                            
                        ).ConfigureAwait(false);
                    }).ConfigureAwait(false);

                List<Models.PersistedGrant> persistedGrants =
                    new List<Models.PersistedGrant>();

                if (dictionary == null || dictionary.Count == 0)
                {
                    persistedGrants = (await _inner.GetAllAsync(subjectId)).ToList();

                    string key;
                    CacheItem item;

                    Dictionary<string, CacheItem> cachePersistedGrants =
                        new Dictionary<string, CacheItem>(persistedGrants.Count());

                    foreach (var persistedGrant in persistedGrants)
                    {
                        key = Utilities
                                    .CreateCachedPersistedGrantStoreKey(
                                                                persistedGrant);

                        item = new CacheItem(persistedGrant.ToEntity())
                        {
                            Tags =
                                Utilities
                                    .CreateCachePersistedGrantStoreTags(
                                        persistedGrant),
                            Expiration =
                            Utilities
                                .DetermineCachePersistedGrantStoreExpiration(
                                                persistedGrant, _handle.options)
                        };

                        cachePersistedGrants.Add(key, item);

                    }

                    if (cachePersistedGrants.Count() > 0)
                    {
                        if (_debugLoggingEnabled)
                        {
                            _logger.LogDebug(
                                $"Cache Miss: All persisted grants with subject Id" +
                                $" {subjectId} gotten from IPersistedGrantStore" +
                                $" instance");
                        }

                        var result = await _setFallBackPolicy
                            .ExecuteAsync(async () =>
                            {
                                await Task.Run(() =>
                                    _handle.cache.InsertBulk(cachePersistedGrants))
                                .ConfigureAwait(false);
                                return true;
                            }).ConfigureAwait(false);

                        if (!result)
                        {
                            if (_errorLoggingEnabled)
                            {
                                _logger.LogError(
                                    $"Caching problems with persistant store cache");
                            }
                        }
                    }
                    else
                    {
                        if (_debugLoggingEnabled)
                        {
                            _logger.LogDebug(
                                $"No persisted grants found for subject id " +
                                $"{subjectId}");
                        }
                    }
                }
                else
                {
                    persistedGrants
                        .AddRange(dictionary
                                        .Values
                                            .ToList()
                                            .Select(x => x.ToModel()));
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"Cache Hit: All persisted grants with " +
                            $"subject Id {subjectId} gotten from Cache");
                    }
                }

                return persistedGrants;
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with GetAllAsync " +
                        $"for {subjectId}");
                }
                throw;
            }
        }

        public async Task<Models.PersistedGrant> GetAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(
                        "key cant be null or white space", nameof(key));
                }

                var cacheKey = Utilities.CreateCachedPersistedGrantStoreKey(key);

                var cachedPersistedGrant = await _getPersistedGrantPolicy
                    .ExecuteAsync(async () =>
                    {
                        return await Task.Run(() =>
                            _handle.cache.Get<PersistedGrant>(cacheKey)
                        ).ConfigureAwait(false);
                    }).ConfigureAwait(false);

                Models.PersistedGrant persistedGrant = null;

                if (cachedPersistedGrant == null)
                {
                    persistedGrant = await _inner.GetAsync(key);

                    if (persistedGrant != null)
                    {

                        if (_debugLoggingEnabled)
                        {
                            _logger.LogDebug(
                                $"Cache Miss: Persisted grant {key}" +
                                $"acquired from IPersistedGrantStore");
                        }
                        var item = new CacheItem(persistedGrant.ToEntity())
                        {
                            Tags =
                                Utilities.CreateCachePersistedGrantStoreTags(
                                                        persistedGrant),
                            Expiration =
                            Utilities.DetermineCachePersistedGrantStoreExpiration
                                                    (
                                                        persistedGrant,
                                                        _handle.options)
                        };

                        bool result = await _setFallBackPolicy
                                .ExecuteAsync(async () =>
                                {
                                    await _handle.cache.InsertAsync(cacheKey, item)
                                        .ConfigureAwait(false);
                                    return true;
                                }).ConfigureAwait(false);

                        if (_errorLoggingEnabled && !result)
                        {
                            _logger.LogError(
                                $"Caching problems with persistant store cache");
                        }
                    }
                    else
                    {
                        if (_debugLoggingEnabled)
                        {
                            _logger.LogDebug(
                                $"Persisted grant {key} not found");
                        }
                    }
                }
                else
                {
                    persistedGrant = cachedPersistedGrant.ToModel();
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"Cache Hit: Persisted grant {key}" +
                            $"acquired from NCache");
                    }
                }

                return persistedGrant;
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with GetAsync for {key}");
                }
                throw;
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subjectId) ||
                    string.IsNullOrWhiteSpace(clientId))
                {
                    throw new ArgumentNullException(
                        "subject Id and/or client Id is empty or white space");
                }

                await _inner.RemoveAllAsync(subjectId, clientId);

                bool result = await _setFallBackPolicy
                    .ExecuteAsync(async () =>
                    {
                        await Task.Run(() =>
                        {
                            _handle.cache.SearchService.RemoveByTags(
                                    Utilities
                                        .CreateCachePersistedGrantStoreTags(
                                                        subjectId,
                                                        clientId),
                                    TagSearchOptions.ByAllTags);
                        }).ConfigureAwait(false);
                        return true;
                    }).ConfigureAwait(false);


                if (result)
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug($"Persisted grants with subject id " +
                            $"{subjectId} and client id {clientId} " +
                            $"successfully removed");
                    }
                }
                else
                {
                    if (_errorLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"Caching problems with persistant store cache");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with RemoveAllAsync " +
                        $"for {subjectId}, {clientId}");
                }
                throw;
            }
        }

        public async Task RemoveAllAsync(
            string subjectId,
            string clientId,
            string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subjectId) ||
                    string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(type))
                {
                    throw new ArgumentNullException(
                        $"subject Id and/or client Id and/or type is empty " +
                        $"or white space");
                }

                await _inner.RemoveAllAsync(subjectId, clientId, type);

                bool result = await _setFallBackPolicy
                    .ExecuteAsync(async () =>
                    {
                        await Task.Run(() =>
                        {
                            _handle.cache.SearchService.RemoveByTags(
                                    Utilities
                                        .CreateCachePersistedGrantStoreTags(
                                                        subjectId,
                                                        clientId,
                                                        type),
                                    TagSearchOptions.ByAllTags);
                        }).ConfigureAwait(false);
                        return true;
                    }).ConfigureAwait(false);

                if (result)
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"Persisted grants with subject id " +
                            $"{subjectId}, client id {clientId} " +
                            $"and type {type} successfully removed");
                    }
                }
                else
                {
                    if (_errorLoggingEnabled)
                    {
                        _logger.LogError("Caching problems");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with RemoveAllAsync " +
                        $"for {subjectId}, {clientId},{type}");
                }
                throw;
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(
                        nameof(key),
                        "key can not be null or white space");
                }

                await _inner.RemoveAsync(key);

                var result = await _setFallBackPolicy
                    .ExecuteAsync(async () =>
                    {
                        await _handle.cache.RemoveAsync<PersistedGrant>(
                        Utilities
                            .CreateCachedPersistedGrantStoreKey(key))
                        .ConfigureAwait(false);
                        return true;
                    }).ConfigureAwait(false);


                if (result)
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"Persisted grants with key {key} " +
                            $"successfully removed");
                    }
                }
                else
                {
                    if (_errorLoggingEnabled)
                    {
                        _logger.LogError(
                            $"Caching problems with persistant store cache");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with RemoveAsync for key {key}");
                }
                throw;
            }
        }

        public async Task StoreAsync(Models.PersistedGrant grant)
        {
            try
            {
                if (grant == null)
                {
                    throw new ArgumentNullException(
                        nameof(grant),
                        "grant cant be null");
                }

                await _inner.StoreAsync(grant);

                var cacheKey = Utilities.CreateCachedPersistedGrantStoreKey(grant);
                var item = new CacheItem(grant.ToEntity())
                {
                    Tags = Utilities.CreateCachePersistedGrantStoreTags(grant),
                    Expiration =
                    Utilities.DetermineCachePersistedGrantStoreExpiration(
                                grant, _handle.options)
                };

                var result = await _setFallBackPolicy
                    .ExecuteAsync(async () =>
                    {
                        await _handle.cache.InsertAsync(cacheKey, item)
                                    .ConfigureAwait(false);
                        return true;
                    }).ConfigureAwait(false);


                if (result)
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                                        $"Persisted grant with key {grant.Key} " +
                                        $"successfully stored");
                    }
                }
                else
                {
                    if (_errorLoggingEnabled)
                    {
                        _logger.LogError(
                            $"Caching problems with persistant store cache");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with StoreAsync " +
                        $"for  grant key {grant.Key}");
                }
                throw;
            }
        }
    }
}
