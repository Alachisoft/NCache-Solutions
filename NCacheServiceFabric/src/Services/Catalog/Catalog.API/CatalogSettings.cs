namespace Microsoft.eShopOnContainers.Services.Catalog.API
{
    public class CatalogSettings
    {
        public string PicBaseUrl { get; set; }

        public string ConnectionString { get; set; }

        public bool UseCustomizationData { get; set; }

        public double NCacheAbsoluteExpirationTime { get; set; } = 60;

        public bool EFCoreCachingEnabled { get; set; }
    }
}
