using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using IdentityServer4.Services;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using Polly.Wrap;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Caching
{
    public class Cache<T> : ICache<T> where T:class
    {
        private readonly string entityType = $"{typeof(T).FullName}";

        private readonly CacheHandle _handle;
        private readonly IPersistentGrantSerializer _serializer = 
            new PersistentGrantSerializer();
        private readonly ILogger<Cache<T>> _logger;
        private readonly bool _debugLoggingEnabled;
        private readonly bool _errorLoggingEnabled;

        private readonly AsyncPolicyWrap<string> _getFallBack;
        private readonly AsyncPolicyWrap<bool> _setFallBack;
        public Cache(
            CacheHandle handle,
            ILogger<Cache<T>> logger)
        {
            _handle = handle ?? throw new ArgumentNullException(nameof(handle));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _debugLoggingEnabled = 
                logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug);
            _errorLoggingEnabled =
                logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error);
            _getFallBack = _handle.getFallBack;
            _setFallBack = _handle.setFallBack;
        }

        public async Task<T> GetAsync(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var cacheKey = GetKey(key);

                string json = await _getFallBack
                    .ExecuteAsync(async () =>
                        await Task.Run(
                            () => _handle.cache.Get<string>(cacheKey))
                        .ConfigureAwait(false)
                        )
                    .ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            "Cache Miss: No item with key {key} in NCache", key); 
                    }
                    return null;
                }
                else if (json.Equals(CacheHandle.CACHE_PROBLEMS))
                {
                    if (_errorLoggingEnabled)
                    {
                        _logger.LogError(
                            $"Cache problems when accessing {key}");
                    }
                    return null;
                }
                else
                {
                    if (_debugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            "Cache Hit: Item with key {key} found in NCache", key);
                    }
                    return _serializer.Deserialize<T>(json);
                }
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                                ex,
                                $"Something wrong with GetAsync for key {key}"); 
                }
                throw;
            }

        }

        public async Task SetAsync(
            string key,
            T item,
            TimeSpan expiration)
        {
            var result = await _setFallBack
                .ExecuteAsync(async () =>
                    await SetAsyncInner(key, item, expiration)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (result)
            {
                if (_debugLoggingEnabled)
                {
                    _logger.LogDebug(
                        $"Cache insert operation successful for {key}");
                }
            }
            else
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogDebug(
                                $"Caching problems when setting {key}"); 
                }
            }
        }

        private async Task<bool> SetAsyncInner(
            string key, 
            T item, 
            TimeSpan expiration)
        {
            try
            {
                var cacheKey = GetKey(key);
                await _handle.cache.InsertAsync(
                    cacheKey,
                    new CacheItem(_serializer.Serialize<T>(item))
                    {
                        Expiration =
                            GetExpiration(expiration)
                    }) ;

                return true;
            }
            catch (Exception ex)
            {
                if (_errorLoggingEnabled)
                {
                    _logger.LogError(
                        ex, 
                        $"Something wrong with SetAsync for key {key}"); 
                }
                throw;
            }
        }

        private string GetKey(string key)
        {
            return $"{entityType}:{key.Trim()}";
        }

        private Expiration GetExpiration(TimeSpan timeSpan)
        {
            if (_handle.options.Expiration == 
                    Options.CacheExpirationType.Absolute)
            {
                return new Expiration(ExpirationType.Absolute, timeSpan);
            }
            else
            {
                return new Expiration(ExpirationType.Sliding, timeSpan);
            }
        }
    }
}
