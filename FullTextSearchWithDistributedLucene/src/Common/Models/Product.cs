// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;

namespace NCommerce.Common.Models
{
    [Serializable]
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string CategroryTree { get; set; }
        public int RetailPrice { get; set; }
        public int DiscountedPrice { get; set; }
        public IList<string> ImageUrls { get; set; }
        public string Description { get; set; }
        public string ProductRating { get; set; }
        public string OverallRating { get; set; }
        public string Brand { get; set; }
        public string Sepecification { get; set; }
    }
}
