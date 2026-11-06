using Alachisoft.NCache.Client;
using Entities;
using Microsoft.Extensions.Configuration;
using System;

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
        /// Connects to the cache named by CacheName in appsettings.json. NCache reads the
        /// server and port of the cache from the "ncache" section of the same file.
        /// </summary>
        private static ICache InitializeCache()
        {
            IConfiguration settings = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var cacheName = settings["CacheName"];
            if (string.IsNullOrWhiteSpace(cacheName))
            {
                throw new InvalidOperationException("CacheName cannot be null or empty in appsettings.json.");
            }

            return CacheManager.GetCache(cacheName, new ConnectionOptions());
        }
    }
}
