using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace StreamToNCache
{
    class Program
    {
        static ICache _cache;

        static private void InitializeParameters()
        {
            var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json",
                                     false,
                                     true)
                                    .Build();

            IDictionary parameters = new Dictionary<string, string>();
            parameters.Add("cacheId", configuration["settings:cacheId"]);

            _cache = CacheManager.GetCache(parameters["cacheId"].ToString());
            Console.WriteLine("Cache Initialized");
        }

        static private void PopulateCache()
        {
            ICounter customerId = null;
            customerId = _cache.DataTypeManager.GetCounter("customerId");
            if(customerId == null)
                customerId = _cache.DataTypeManager.CreateCounter("customerId", 0);

            while (true)
            {
                var fakeOrders = new Bogus.Faker<Order>()
                    .RuleFor(o => o.OrderDate, (fake) => fake.Date.Past())
                    .RuleFor(o => o.ShipAddress, (fake) => fake.Address.StreetAddress())
                    .RuleFor(o => o.ShipCity, (fake) => fake.Address.City())
                    .RuleFor(o => o.ShipCountry, (fake) => fake.Address.Country())
                    .RuleFor(o => o.OrderID, (fake) => fake.UniqueIndex)
                    .RuleFor(o => o.ShipName, (fake) => fake.Lorem.Word());

                Random r = new Random();
                int randomOrdersCount = r.Next(1, 15);

                var customers = new Bogus.Faker<Customer>()
                    .RuleFor(i => i.Id, (fake) => $"{customerId.Increment()}")
                    .RuleFor(i => i.Contact, (fake) => fake.Person.FullName)
                    .RuleFor(i => i.Address, (fake) => fake.Address.StreetAddress())
                    .RuleFor(i => i.City, (fake) => "Ridgemond")
                    .RuleFor(i => i.Country, (fake) => "US")
                    .RuleFor(i => i.Company, (fake) => "Microsoft")
                    .RuleFor(i => i.Orders, (fake) => fakeOrders.Generate(randomOrdersCount))
                    .RuleFor(i => i.OrdersCount, (fake) => randomOrdersCount)
                    .Generate(1);

                var cacheItem = new CacheItem(customers[0]);
                cacheItem.Expiration = new Expiration(ExpirationType.Sliding, new TimeSpan(0, 0, 15));
                _cache.Insert("CustomerID:" + customers[0].Id, cacheItem);


                Thread.Sleep(r.Next(200, 300));
            }
        }
        static void Main(string[] args)
        {
            InitializeParameters();
            PopulateCache();
        }
    }
}
