using Alachisoft.NCache.Client;
using Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;

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
