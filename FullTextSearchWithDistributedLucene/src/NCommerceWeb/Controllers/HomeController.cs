// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Microsoft.AspNetCore.Mvc;
using NCommerceWeb.Models;
using System.Diagnostics;

namespace NCommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new FormModel();
            model.Category = "All";

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(FormModel model)
        {
            ViewData["searchItem"] = model;

            return View(model);
        }

        public IActionResult Info(string id)
        {
            ViewData.Add("id", id);
            var model = new FormModel();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
