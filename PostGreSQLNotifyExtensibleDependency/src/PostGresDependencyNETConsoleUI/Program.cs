using Alachisoft.NCache.Client;
using Entities;
using Npgsql;
using System;

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
                const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=database";

                var customer = new Customer() { customerid = "ALFKI", address = "USA", city = "Newyork", country = "USA"};

                var cacheItem = new CacheItem(customer)
                {
                    Dependency = new PostGreSQLDependency(connectionString, customer.customerid, "public", "customers", "customer_channel")
                };

                var key = customer.customerid;

                var cache = CacheManager.GetCache("democache");

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


    }
}
