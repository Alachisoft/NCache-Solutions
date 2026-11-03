using Alachisoft.NCache.Client;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency.NETCOREConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cache = InitializeCache();
                //Key with which customer will be fected from database on the basis of CustomerID
                var customerId = "ALFKI";
                var customer = cache.Get<Customer>(customerId, new Runtime.Caching.ReadThruOptions(Runtime.Caching.ReadMode.ReadThru, "PostGreSqlReadThruProvider"));
                Console.WriteLine($"Customer details are \n{customer.customerid}\n{customer.address}\n{customer.city}\n{customer.country}");

                Console.ReadLine();

                customer = cache.Get<Customer>(customerId, new Runtime.Caching.ReadThruOptions(Runtime.Caching.ReadMode.ReadThru, "PostGreSqlReadThruProvider"));
                Console.WriteLine($"Customer details are \n{customer.customerid}\n{customer.address}\n{customer.city}\n{customer.country}");


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Connects to the cache as a thin client using the cacheName, cacheServer and cachePort
        /// values from the appSettings section of App.config.
        /// </summary>
        private static ICache InitializeCache()
        {
            var cacheName = GetRequiredSetting("cacheName");
            var server = GetRequiredSetting("cacheServer");
            var portValue = GetRequiredSetting("cachePort");

            if (!int.TryParse(portValue, out int port))
            {
                throw new ConfigurationErrorsException($"The 'cachePort' value '{portValue}' in App.config is not a valid port number.");
            }

            var options = new ConnectionOptions
            {
                Servers = new List<ServerInfo> { new ServerInfo(server, port) }
            };

            return CacheManager.GetCache(cacheName, options);
        }

        private static string GetRequiredSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException($"The '{key}' setting is missing from the appSettings section of App.config.");
            }
            return value.Trim();
        }
    }
}
