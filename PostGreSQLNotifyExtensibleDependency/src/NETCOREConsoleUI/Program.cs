using Alachisoft.NCache.Client;
using Npgsql;
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

                var cacheItem = new CacheItem("dummy")
                {
                    Dependency = new PostGreSQLDependency(connectionString, "a", "public", "customers", "customer_channel")
                };

                var key = "dummy";

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
