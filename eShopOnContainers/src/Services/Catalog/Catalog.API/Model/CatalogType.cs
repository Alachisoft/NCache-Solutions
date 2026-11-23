// Copyright (c) .NET Foundation and Contributors.
using System;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Model
{
    [Serializable]
    public class CatalogType
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
