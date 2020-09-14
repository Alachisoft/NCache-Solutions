using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Stores.Interfaces;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IConfigurationStoreRepository<Client> _repository;
        private readonly ILogger<ClientStore> _logger;
        private readonly bool _isDebugLoggingEnabled;
        private readonly bool _isErrorLoggingEnabled;

        public ClientStore(
            IConfigurationStoreRepository<Client> repository,
            ILogger<ClientStore> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository), "Repository parameter can't be null");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isErrorLoggingEnabled = logger.IsEnabled(LogLevel.Error);
            _isDebugLoggingEnabled = logger.IsEnabled(LogLevel.Debug);
            
        }
        public async Task<Models.Client> FindClientByIdAsync(string clientId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(clientId))
                {
                    throw new ArgumentNullException(
                        "client Id can't be null or white space");
                }

                var ncacheClient =
                        await _repository.GetSingleAsync($"{clientId}");

                if (ncacheClient == null)
                {
                    if (_isErrorLoggingEnabled)
                    {
                        _logger.LogError($"Client with ID {clientId} not found"); 
                    }
                    return null;
                }

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug($"Client with ID {clientId} found"); 
                }
                return ncacheClient.ToModel();
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                        _logger.LogError(
                            ex, 
                            $"Something went wrong with FindClientByIdAsync " +
                            $"for client id {clientId}");
                }
                throw;
            }
        }
    }
}
