using System;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Model
{
    [Serializable]
    public class CatalogBrand
    {
        public int Id { get; set; }

        public string Brand { get; set; }
    }
}
