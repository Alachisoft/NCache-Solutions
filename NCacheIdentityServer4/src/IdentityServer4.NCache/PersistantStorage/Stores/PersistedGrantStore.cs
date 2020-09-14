using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Stores.Interfaces;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantStoreRepository _repository;
        private readonly ILogger<PersistedGrantStore> _logger;
        private readonly bool _isErrorLoggingEnabled;
        private readonly bool _isDebugLoggingEnabled;
        public PersistedGrantStore(
            IPersistedGrantStoreRepository repository,
            ILogger<PersistedGrantStore> logger)
        {
            _repository =
                repository ??
                throw new ArgumentNullException(
                    nameof(repository),
                    "Repository parameter can't be null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isDebugLoggingEnabled = logger.IsEnabled(LogLevel.Debug);
            _isErrorLoggingEnabled = logger.IsEnabled(LogLevel.Error);
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

                var ncachePersistedGrants =
                    await _repository.GetMultipleByTagsAsync(
                        new string[] { $"{subjectId}" });

                if (ncachePersistedGrants.Count(x => true) == 0)
                {
                    if (_isDebugLoggingEnabled)
                    {
                        _logger.LogDebug(
                            $"No persisted grants found for {subjectId}");
                    }
                    return new List<Models.PersistedGrant>();
                }

                var persistedGrants =
                    ncachePersistedGrants.Select(x => x.ToModel()).ToList();

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                                $"{persistedGrants.Count} persisted grants found " +
                                $"for {subjectId}");
                }

                return persistedGrants;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
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
                    throw new ArgumentException(
                        "key cant be null or white space", nameof(key));
                }

                var ncachePersistantGrant =
                    await _repository.GetSingleAsync($"{key}");

                var persistedGrant = ncachePersistantGrant?.ToModel();

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                                $"{key} found in NCache: " +
                                $"{persistedGrant != null}");
                }

                return persistedGrant;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
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
                await _repository.DeleteByTagsAsync(
                        new string[] {  $"{subjectId}",
                                        $"{clientId}"
                        });

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                        $"Removed all persisted grants for subject " +
                        $"{subjectId}, clientId {clientId}");
                }
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
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
                        $"subject Id and/or client Id and/or type is " +
                        $"empty or white space");
                }
                await _repository.DeleteByTagsAsync(
                        new string[] {  $"{subjectId}",
                                        $"{clientId}",
                                        $"{type}" });

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                                $"Removed all persisted grants for " +
                                $"subject {subjectId}, " +
                                $"clientId {clientId}, type {type}");
                }
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
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
                        nameof(key), "key can not be null or white space");
                }

                await _repository.DeleteAsync($"{key}");

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug($"Key if any removed");
                }
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
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
                    throw new ArgumentNullException("grant cant be null");
                }

                var ncachePersistantGrant = grant.ToEntity();

                await _repository.AddAsync(ncachePersistantGrant);

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug("Grant {key} stored", grant.Key);
                }
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"something went wrong with StoreAsync for " +
                        $"grant key {grant.Key}");
                }
                throw;
            }
        }
    }
}
