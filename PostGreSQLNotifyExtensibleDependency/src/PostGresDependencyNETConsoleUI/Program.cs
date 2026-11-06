using Alachisoft.NCache.Client;
using Entities;
using Npgsql;
using System;
using System.Configuration;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency.NETConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            NpgsqlConnection connection = null;
            try

            {
                // Change the connection string to your PostGreSQL Database
                const string connectionString = "Host={server};Port=5432;Username={user};Password={password};Database={database}";

                var customer = new Customer() { customerid = "ALFKI", address = "USA", city = "Newyork", country = "USA"};

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
