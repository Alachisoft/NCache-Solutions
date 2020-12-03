using Alachisoft.NCache.Client;
using Entities;
using System;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency.NETCOREConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cache = CacheManager.GetCache("democache");
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


    }
}
