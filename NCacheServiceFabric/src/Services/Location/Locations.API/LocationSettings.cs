using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Locations.API
{
    public class LocationSettings
    {
        public string ExternalCatalogBaseUrl { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool LocationCachingEnabled { get; set; }
        public string CacheID { get; set; }
        public string LocationCacheIPAddresses { get; set; }
        public double LocationCacheExpirationTimeInMinutes { get; set; } = 60;

        public bool CosmosDBDeployment { get; set; } = false;
    }
}
