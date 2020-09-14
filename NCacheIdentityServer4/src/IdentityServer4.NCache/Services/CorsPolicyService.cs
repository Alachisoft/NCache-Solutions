using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Stores.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<CorsPolicyService> _logger;
        private readonly bool _isDebugLoggingEnabled;
        private readonly bool _isErrorLoggingEnabled;

        public CorsPolicyService(
            IHttpContextAccessor context,
            ILogger<CorsPolicyService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isDebugLoggingEnabled = logger.IsEnabled(LogLevel.Debug);
            _isErrorLoggingEnabled = logger.IsEnabled(LogLevel.Error);
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(origin))
                {
                    throw new ArgumentNullException("origin can not be null");
                }

                origin = origin.Trim().ToLowerInvariant();
                var repository =
                    _context
                        .HttpContext
                        .RequestServices
                        .GetRequiredService<IConfigurationStoreRepository<Client>>();

                var origins =
                    await repository.GetMultipleByTagsAsync(
                        new string[] { $"{origin}" });

                var result = origins.ToList().Count != 0;

                if (_isDebugLoggingEnabled)
                {
                    _logger.LogDebug($"Origin {origin} is allowed: {result}");
                }
                return result;
            }
            catch (Exception ex)
            {
                if (_isErrorLoggingEnabled)
                {
                    _logger.LogError(
                        ex,
                        $"Something went wrong with IsOriginAllowedAsync " +
                        $"for {origin}");
                }
                throw;
            }
        }
    }
}
