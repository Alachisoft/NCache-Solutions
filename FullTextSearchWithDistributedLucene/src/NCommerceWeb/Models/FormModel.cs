// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Microsoft.AspNetCore.Mvc.Rendering;
using NCommerce.Common;
using System.Collections.Generic;

namespace NCommerceWeb.Models
{
    public class FormModel
    {
        private static IList<string> categoryList = null;

        public string Category { get; set; }
        public string SearchText { get; set; }

        public List<SelectListItem> Categories
        {
            get
            {
                if (categoryList == null)
                    categoryList = Util.GetDataProvider().GetCategories();

                var selectedList = new List<SelectListItem>();
                selectedList.Add(new SelectListItem { Value = "All", Text = "All" });

                foreach (var category in categoryList)
                {
                    selectedList.Add(new SelectListItem { Value = category, Text = category });
                }

                return selectedList;
            }
        }

        public override string ToString()
        {
            return "Category: " + Category + ", SearchText: " + SearchText;
        }
    }
}
