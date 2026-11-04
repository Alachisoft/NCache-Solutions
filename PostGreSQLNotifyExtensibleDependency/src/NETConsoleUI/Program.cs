using Alachisoft.NCache.Client;
using Entities;
using System;
using System.Configuration;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency.NETConsoleUI
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
        /// Connects to the cache named by the CacheName appSetting in App.config. NCache reads the
        /// server and port of the cache from the &lt;ncache&gt; section of the same file.
        /// </summary>
        private static ICache InitializeCache()
        {
            var cacheName = ConfigurationManager.AppSettings["CacheName"];
            if (string.IsNullOrWhiteSpace(cacheName))
            {
                throw new ConfigurationErrorsException("The 'CacheName' setting is missing from the appSettings section of App.config.");
            }

            return CacheManager.GetCache(cacheName, new ConnectionOptions());
        }
    }
}
