using Alachisoft.NCache.Client;
using Entities;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency.NETCOREConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            NpgsqlConnection connection = null;
            try
            {
                // Change the connection string to your PostGreSQL Database
                const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=database";

                var customer = new Customer() { customerid = "ALFKI", address = "USA", city = "Newyork", country = "USA" };

                var cacheItem = new CacheItem(customer)
                {
                    Dependency = new PostGreSQLDependency(connectionString, customer.customerid, "public", "customers", "customer_channel")
                };

                var key = customer.customerid;

                var cache = InitializeCache();

                cache.Insert(key, cacheItem);

                Console.ReadLine();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
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
