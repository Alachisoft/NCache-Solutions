using System;
using Alachisoft.NCache.Client;
using NCache.StackExchange.Redis;

namespace BasicUsageStackExchangeRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectionMultiplexer ncache = ConnectionMultiplexer.Connect("cacheName");
                ICache cache = ncache.GetNCacheInterface("cacheName");
                cache.Clear();
                var db = ncache.GetDatabase();
                db.StringSet("key1", "Value1");
                var value = db.StringGet("key1");
                var value2 = db.StringGet("key2");

                var listCount = db.ListRightPush("Customers", "Mark");
                listCount = db.ListRightPush("Customers", "Steve");
                Console.WriteLine($"Number of customers in list: {listCount}");
                listCount = db.ListRemove("Customers", "Mark");
                Console.WriteLine($"Number of customers in list: {listCount}");
                var customerName = db.ListLeftPop("Customers");
                Console.WriteLine($"The name of fetched customer from list is {customerName}");




                Console.WriteLine($"The value fetched against key: key1 is {value}");
                Console.WriteLine($"The value fetched against key: key2 is {value2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured with description \n" + ex.Message);
            }
        }

        private static void MessageCallBack(ChannelMessage obj)
        {
            Console.WriteLine("Message received from sender" + obj.Message.Box());
        }
    }
}
