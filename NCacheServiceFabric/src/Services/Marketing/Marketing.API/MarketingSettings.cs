using System.Security;

namespace Microsoft.eShopOnContainers.Services.Marketing.API
{
    public class MarketingSettings
    {
        public string ConnectionString { get; set; }
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
        public string ExternalCatalogBaseUrl { get; set; }
        public string PicBaseUrl { get; set; }
        public bool CachingEnabled { get; set; }
        public string MarketingCacheID { get; set; }
        public string MarketingCacheIPAddresses { get; set; }
        public double MarketingCacheExpirationTimeInMinutes { get; set; } = 60;

        public bool CosmosDBDeployment { get; set; } = false;
    }
}
