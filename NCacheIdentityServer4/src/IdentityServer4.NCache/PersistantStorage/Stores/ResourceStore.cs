using IdentityServer4.Models;
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
    public class ResourceStore : IResourceStore
    {
        private readonly
            IConfigurationStoreRepository<Entities.ApiResource> _apiRepository;
        private readonly
            IConfigurationStoreRepository<Entities.IdentityResource> _idRepository;
        private readonly ILogger<ResourceStore> _logger;
        private readonly bool _isErrorLoggingEnabled;
        private readonly bool _isDebugLoggingEnabled;



        public ResourceStore(
            IConfigurationStoreRepository<Entities.ApiResource> apiRepository,
            IConfigurationStoreRepository<Entities.IdentityResource> idRepository,
            ILogger<ResourceStore> logger)
        {
            _apiRepository = apiRepository ??
                                throw new ArgumentNullException(
                                    nameof(apiRepository),
                                    "Api Repository parameter can't be null");
            _idRepository = idRepository ??
                                throw new ArgumentNullException(
                                    nameof(idRepository),
                                    "Id Repository parameter can't be null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isDebugLoggingEnabled = logger.IsEnabled(LogLevel.Debug);
            _isErrorLoggingEnabled = logger.IsEnabled(LogLevel.Error);
        }
        public async Task<Models.ApiResource> FindApiResourceAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(
                        "name can't be null or whitespace in FindApiResourceAsync");
                }

                var ncacheApiResource =
                    await _apiRepository.GetSingleAsync($"{name}");

                var model = ncacheApiResource.ToModel();

                if (_isDebugLoggingEnabled)
                {
                    if (model == null)
                    {
                        _logger.LogDebug(
                            "Did not find {api} API resource in NCache", name);
                    }
                    else
                    {
                        _logger.LogDebug(
                            "Found {api} API resource in NCache", name);
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with FindApiResourceAsync " +
                        $"for {name}");
                }

                throw;
            }
        }

        public async Task<IEnumerable<Models.ApiResource>>
                FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            try
            {
                if (scopeNames == null)
                {
                    throw new ArgumentNullException(nameof(scopeNames));
                }

                var scopeNameList = new List<string>();

                foreach (var scopeName in scopeNames)
                {
                    scopeNameList.Add($"{scopeName}");
                }
                var ncacheApiResources =
                    new List<Entities.ApiResource>(
                        await _apiRepository.GetMultipleByTagsAsync(
                                                            scopeNameList));

                if (ncacheApiResources.Count == 0)
                {
                    return new List<Models.ApiResource>();
                }

                var apiResources = ncacheApiResources.Select(x => x.ToModel());

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                                "Found {scopes} api resources in NCache",
                                apiResources.Select(x => x.Name));
                }

                return apiResources;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with " +
                        $"FindApiResourcesByScopeAsync");
                }

                throw;
            }
        }

        public async Task<IEnumerable<Models.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            try
            {
                if (scopeNames == null)
                {
                    throw new ArgumentNullException(nameof(scopeNames));
                }

                var keys = scopeNames.ToArray().Select(scopeName => $"{scopeName}");

                var ncacheIdentityResources =
                    new List<Entities.IdentityResource>(
                        await _idRepository.GetMultipleAsync(keys));

                if (ncacheIdentityResources.Count == 0)
                {
                    return new List<Models.IdentityResource>();
                }

                var identityResources =
                    ncacheIdentityResources.Select(x => x.ToModel());

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                             "Found {scopes} identity scopes in NCache",
                             identityResources.Select(x => x.Name));
                }

                return identityResources;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with" +
                        $" FindIdentityResourcesByScopeAsync");
                }

                throw;

            }
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            try
            {
                List<Models.IdentityResource> identityResources =
                    new List<Models.IdentityResource>();
                List<Models.ApiResource> apiResources =
                    new List<Models.ApiResource>();

                var ncacheIdentityResources =
                    await _idRepository.GetMultipleByTagsAsync(
                        new string[] { "IdentityResource" }
                    );

                var ncacheApiResources =
                    await _apiRepository.GetMultipleByTagsAsync(
                        new string[] { "ApiResource" });

                if (ncacheIdentityResources.Count(x => true) > 0)
                {
                    identityResources =
                        ncacheIdentityResources
                        .ToArray()
                        .Select(x => x.ToModel()).ToList();
                }

                if (ncacheApiResources.Count(x => true) > 0)
                {
                    apiResources =
                        ncacheApiResources
                        .ToArray()
                        .Select(x => x.ToModel()).ToList();
                }

                var result = new Resources(
                                    identityResources,
                                    apiResources);

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug(
                        "Found {scopes} as all scopes in NCache",
                            result.IdentityResources.Select(x => x.Name)
                                .Union(
                                    result.ApiResources
                                        .SelectMany(x => x.Scopes)
                                        .Select(x => x.Name)));
                }

                return result;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with GetAllResourcesAsync");
                }
                throw;
            }
        }
    }
}
