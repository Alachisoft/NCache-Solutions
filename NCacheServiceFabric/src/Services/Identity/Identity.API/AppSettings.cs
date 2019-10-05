namespace Microsoft.eShopOnContainers.Services.Identity.API
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public string IsClusterEnv { get; set; }

        public string CacheID { get; set; }

        public string MvcClient { get; set; }

        public string LocationApiClient { get; set; }

        public string MarketingApiClient { get; set; }

        public string BasketApiClient { get; set; }

        public string OrderingApiClient { get; set; }

        public string WebShoppingAggClient { get; set; }

        public string WebStatusClient { get; set; }

        public bool UseCustomizationData { get; set; }
    }
}
