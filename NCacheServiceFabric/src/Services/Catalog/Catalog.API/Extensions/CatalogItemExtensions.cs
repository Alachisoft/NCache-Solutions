using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Model
{
    public static class CatalogItemExtensions
    {
        public static void FillProductUrl(this CatalogItem item, string picBaseUrl)
        {
            if (item != null)
            {
                item.PictureUri = picBaseUrl.Replace("[0]", item.Id.ToString());
            }
        }
    }
}
