// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Text;

namespace NCommerce.Common.Models
{
    public class ProductFTS
    {
        private string _description;

        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int RetailPrice { get; set; }
        public int DiscountedPrice { get; set; }
        public string ImageUrl { get; set; }
        public string Description
        {
            get
            {
                // Return limited description, some products have very lengthy descripitons! It should be handeled on UI
                return _description.Length < 500 ?_description : _description.Substring(0, 500) + "...";
            }
            set { _description = value; }
        }
    }
}
