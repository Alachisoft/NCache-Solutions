// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Microsoft.Extensions.Configuration;
using NCommerce.Common.DataProviders;
using NCommerce.Common.Models;
using System;

namespace NCommerce.Common
{
    public static class Util
    {
        static AppSettings appSettings;
        static IDataProvider dataProvider;
        static bool lastProviderWasCacheable = false;

        static Util()
        {
            GetAppSettings();
        }

        public static AppSettings GetAppSettings()
        {
            if (appSettings == null)
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddJsonFile("appsettings.json", false, true);

                var configuration = configBuilder.Build();

                //appSettings = configRoot.GetSection("NCommerce").Get<AppSettings>();
                appSettings = new AppSettings();
                //var nc = configuration.GetSection("NCommerce");//.Bind(appSettings);

                appSettings.LuceneCacheName = configuration.GetSection("NCommerce:LuceneCacheName").Value;
                appSettings.DataCacheName = configuration.GetSection("NCommerce:DataCacheName").Value;
                appSettings.IndexPath = configuration.GetSection("NCommerce:IndexPath").Value;
                appSettings.UseCustomIndexPath = bool.Parse(configuration.GetSection("NCommerce:UseCustomIndexPath").Value);
                appSettings.ConnectionString = configuration.GetSection("NCommerce:ConnectionString").Value;

                if (appSettings == null)
                    throw new Exception("Unable to load 'NCommerce' section from appsettings.json!");
            }

            return appSettings;
        }

        public static IDataProvider GetDataProvider(bool cacheableProvider = true)
        {
            if (dataProvider == null || lastProviderWasCacheable != cacheableProvider)
            {
                dataProvider = new SQLProvider(appSettings.ConnectionString);
                if (cacheableProvider)
                    dataProvider = new CacheableDataProvider(appSettings.DataCacheName, dataProvider);
            }

            return dataProvider;
        }
    }
}
