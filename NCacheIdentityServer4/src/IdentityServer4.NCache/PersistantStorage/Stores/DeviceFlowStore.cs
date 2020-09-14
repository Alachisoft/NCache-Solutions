using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Stores.Interfaces;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly IDeviceStoreRepository _repository;
        private readonly ILogger<DeviceFlowStore> _logger;
        private readonly bool _isDebugLoggingEnabled;
        private readonly bool _isErrorLoggingEnabled;

        public DeviceFlowStore(
            IDeviceStoreRepository repository,
            ILogger<DeviceFlowStore> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(
                nameof(repository), "Repository parameter can't be null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isDebugLoggingEnabled = logger.IsEnabled(LogLevel.Debug);
            _isErrorLoggingEnabled = logger.IsEnabled(LogLevel.Error);
        }

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceCode))
                {
                    throw new ArgumentNullException(
                        "device code can't be null or whitespace");
                }

                var deviceFlowCode =
                    await _repository.GetSingleAsync($"{deviceCode}");

                var model = deviceFlowCode?.ToModel();

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                        $"{deviceCode} found in NCache: {model != null}");
                }

                return model;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with FindByDeviceCodeAsync " +
                        $"for {deviceCode}");
                }
                throw;
            }
        }

        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userCode))
                {
                    throw new ArgumentNullException(
                        "User code can't be null or whitespace");
                }

                var deviceFlowCodes =
                        await _repository.GetMultipleByTagsAsync(
                            new string[] { $"{userCode}" });

                var deviceFlowCode = deviceFlowCodes.FirstOrDefault();

                var model = deviceFlowCode?.ToModel();

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                        $"{userCode} found in NCache: {model != null}");
                }

                return model;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with FindByUserCodeAsync " +
                        $"for {userCode}");
                }
                throw;
            }
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceCode))
                {
                    throw new ArgumentNullException(
                        "device code can't be null or whitespace");
                }

                await _repository.DeleteAsync($"{deviceCode}");

                if (_isDebugLoggingEnabled)
                {
                    _logger
                        .LogDebug($"Device code {deviceCode} removed if present");
                }

            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with RemoveByDeviceCodeAsync " +
                        $"for {deviceCode}");
                }
                throw;
            }
        }

        public async Task StoreDeviceAuthorizationAsync(
            string deviceCode,
            string userCode,
            DeviceCode data)
        {
            try
            {
                if ((await FindByUserCodeAsync(userCode)) != null ||
                    (await FindByDeviceCodeAsync(deviceCode)) != null)
                {
                    throw new Exception(
                        $"Device flow codes with given user code and/or device " +
                        $"code already exists");
                }


                var deviceFlowCode = data?.ToEntity(deviceCode, userCode);
                await _repository.AddAsync(deviceFlowCode);

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                        $"Device code with id {deviceCode} and user " +
                        $"code {userCode} stored");
                }
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                                ex,
                                $"Something went wrong with " +
                                $"StoreDeviceAuthorizationAsync " +
                                $"for user code {userCode} and id {deviceCode}");
                }

                throw;
            }
        }

        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userCode))
                {
                    throw new ArgumentNullException(
                        "user code can't be null or whitespace");
                }
                if (data == null)
                {
                    throw new ArgumentNullException("data can't be null");
                }

                var deviceFlowCodes =
                                await _repository.GetMultipleByTagsAsync(
                                    new string[] { $"{userCode}" });

                var existing = deviceFlowCodes.FirstOrDefault();

                if (existing == null)
                {
                    if (_isErrorLoggingEnabled)
                    {
                        _logger.LogError(
                            "{userCode} not found in NCache", userCode);
                    }
                    throw new InvalidOperationException(
                        "Could not update device code");
                }

                var entity = data?.ToEntity(existing.DeviceCode, userCode);

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug("{userCode} found in NCache", userCode);
                }

                existing.SubjectId =
                    data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;

                existing.Data = entity.Data;

                await _repository.AddAsync(existing);
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with UpdateByUserCodeAsync " +
                        $"for {userCode}");
                }
                throw;
            }
        }
    }
}
