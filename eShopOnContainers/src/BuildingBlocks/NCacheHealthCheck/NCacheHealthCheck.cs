// Copyright (c) 2019 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using Alachisoft.NCache.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck
{
    public class NCacheHealthCheck : IHealthCheck
    {
        private readonly string _cacheID;
        private readonly CacheConnectionOptions _cacheConnectionOptions;

        public NCacheHealthCheck(string cacheID, CacheConnectionOptions cacheConnectionOptions = null)
        {
            _cacheID = cacheID ?? throw new ArgumentNullException(nameof(cacheID));
            _cacheConnectionOptions = cacheConnectionOptions;
        }

        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var cache = CacheManager.GetCache(_cacheID, _cacheConnectionOptions))
                {
                    return Task.FromResult(HealthCheckResult.Healthy($"Cache {_cacheID} is running"));
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Cache {_cacheID} connection ran into problems", e));
            }
        }

        
    }

    public static class NCacheHealthCheckBuilderExtensions
    {
        const string NAME = "NCache-Health-Check";

        public static IHealthChecksBuilder AddNCacheHealthCheck(
            this IHealthChecksBuilder builder,
            string cacheID,
            string name,
            HealthStatus? failureStatus = HealthStatus.Unhealthy,
            IEnumerable<string> tags = null,
            CacheConnectionOptions cacheConnectionOptions = null)
        {
            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new NCacheHealthCheck(cacheID, cacheConnectionOptions),
                failureStatus,
                tags));
        }
    }
}
