// Copyright (c) 2017 Microsoft Corp.
using System;
using Xamarin.Forms;

namespace eShopOnContainers.Core.Services.OpenUrl
{
    public class OpenUrlService : IOpenUrlService
    {
        public void OpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}
