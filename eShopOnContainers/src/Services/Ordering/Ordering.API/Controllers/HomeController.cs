// Copyright (c) .NET Foundation and Contributors.
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
