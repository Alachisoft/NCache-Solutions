using Alachisoft.NCache.Client;
using Microsoft.Extensions.Configuration;
using Alachisoft.NCache.Runtime.Caching;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Runtime.Exceptions;

namespace Alachisoft.StreamProcessingUI
{
    public class MainApp
    {
        ICache _cache;
        private void InitializeParameters()
        {
            var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json",
                                     false,
                                     true)
                                    .Build();

            IDictionary parameters = new Dictionary<string, string>();
            parameters.Add("cacheId", configuration["settings:cacheId"]);

            _cache = CacheManager.GetCache(parameters["cacheId"].ToString());
            _cache.Clear();
            Console.WriteLine("Cache Initialized");

        }
        public void RegisterContinousQueryForGoldCustomers()
        {
            try
            {
                // Precondition: Cache is already connected
                // Query for required operation
                string query = "SELECT $VALUE$ FROM Models.Customer WHERE OrdersCount >= ?";

                var queryCommand = new QueryCommand(query);
                queryCommand.Parameters.Add("OrdersCount", 10);

                // Create Continuous Query
                var cQuery = new ContinuousQuery(queryCommand);

                // Item add notification 
                // EventDataFilter.None returns the cache keys added
                cQuery.RegisterNotification(new QueryDataNotificationCallback(QueryItemCallBackForGoldCustomers), EventType.ItemAdded, EventDataFilter.None);

                // Register continuousQuery on server 
                _cache.MessagingService.RegisterCQ(cQuery);
                Console.WriteLine("Continous query registered");
            }
            catch (OperationFailedException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.INCORRECT_FORMAT)
                {
                    // Make sure that the query format is correct
                }
                else
                {
                    // Exception can occur due to:
                    // Connection Failures
                    // Operation Timeout
                    // Operation performed during state transfer
                }
            }
            catch (Exception ex)
            {
                // Any generic exception like ArgumentException, ArgumentNullException
            }

        }

        private void QueryItemCallBackForGoldCustomers(string key, CQEventArg arg)
        {
            var cacheItem = _cache.GetCacheItem(key);
            cacheItem.Expiration = new Expiration(ExpirationType.None);
            Tag[] tags = new Tag[1];
            tags[0] = new Tag("GoldCustomers");
            cacheItem.Tags = tags;
            _cache.Insert(key, cacheItem);

            ICollection<string> retrievedKeys = _cache.SearchService.GetKeysByTag(tags[0]);
        }

        public void RegisterContinousQueryForSilverCustomers()
        {
            try
            {
                // Precondition: Cache is already connected

                // Query for required operation
                string query = "SELECT $VALUE$ FROM Models.Customer WHERE OrdersCount >= 7 AND OrdersCount < 10";

                var queryCommand = new QueryCommand(query);

                // Create Continuous Query
                var cQuery = new ContinuousQuery(queryCommand);

                // Item add notification 
                // EventDataFilter.None returns the cache keys added
                cQuery.RegisterNotification(new QueryDataNotificationCallback(QueryItemCallBackForSilverCustomers), EventType.ItemAdded, EventDataFilter.None);

                // Register continuousQuery on server 
                _cache.MessagingService.RegisterCQ(cQuery);
                Console.WriteLine("Continous query registered");
            }
            catch (OperationFailedException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.INCORRECT_FORMAT)
                {
                    // Make sure that the query format is correct
                }
                else
                {
                    // Exception can occur due to:
                    // Connection Failures
                    // Operation Timeout
                    // Operation performed during state transfer
                }
            }
            catch (Exception ex)
            {
                // Any generic exception like ArgumentException, ArgumentNullException
            }

        }

        private void QueryItemCallBackForSilverCustomers(string key, CQEventArg arg)
        {
            var cacheItem = _cache.GetCacheItem(key);
            cacheItem.Expiration = new Expiration(ExpirationType.None);
            Tag[] tags = new Tag[1];
            tags[0] = new Tag("SilverCustomers");
            cacheItem.Tags = tags;
            _cache.Insert(key, cacheItem);

            ICollection<string> retrievedKeys = _cache.SearchService.GetKeysByTag(tags[0]);
        }

        public void RegisterContinousQueryForBronzeCustomers()
        {
            try
            {
                // Precondition: Cache is already connected

                // Query for required operation
                string query = "SELECT $VALUE$ FROM Models.Customer WHERE OrdersCount >= 4 AND OrdersCount < 7";

                var queryCommand = new QueryCommand(query);

                // Create Continuous Query
                var cQuery = new ContinuousQuery(queryCommand);

                // Item add notification 
                // EventDataFilter.None returns the cache keys added
                cQuery.RegisterNotification(new QueryDataNotificationCallback(QueryItemCallBackForBronzeCustomers), EventType.ItemAdded, EventDataFilter.None);

                // Register continuousQuery on server 
                _cache.MessagingService.RegisterCQ(cQuery);
                Console.WriteLine("Continous query registered");
            }
            catch (OperationFailedException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.INCORRECT_FORMAT)
                {
                    // Make sure that the query format is correct
                }
                else
                {
                    // Exception can occur due to:
                    // Connection Failures
                    // Operation Timeout
                    // Operation performed during state transfer
                }
            }
            catch (Exception ex)
            {
                // Any generic exception like ArgumentException, ArgumentNullException
            }

        }

        private void QueryItemCallBackForBronzeCustomers(string key, CQEventArg arg)
        {
            var cacheItem = _cache.GetCacheItem(key);
            cacheItem.Expiration = new Expiration(ExpirationType.None);
            Tag[] tags = new Tag[1];
            tags[0] = new Tag("BronzeCustomers");
            cacheItem.Tags = tags;
            _cache.Insert(key, cacheItem);

            ICollection<string> retrievedKeys = _cache.SearchService.GetKeysByTag(tags[0]);
        }

        private void PrintValuableCustomersCount()
        {
            while (true)
            {
                Console.Clear();
                ICollection<string> retrievedKeys = _cache.SearchService.GetKeysByTag(new Tag("GoldCustomers"));
                Console.WriteLine("Platinum Customers Count:{0}", retrievedKeys.Count);
                retrievedKeys = _cache.SearchService.GetKeysByTag(new Tag("SilverCustomers"));
                Console.WriteLine("Diamond Customers Count:{0}", retrievedKeys.Count);
                retrievedKeys = _cache.SearchService.GetKeysByTag(new Tag("BronzeCustomers"));
                Console.WriteLine("Gold Customers Count:{0}", retrievedKeys.Count);

                Thread.Sleep(3000);
            }
        }

        public void Run()
        {
            InitializeParameters();
            RegisterContinousQueryForGoldCustomers();
            RegisterContinousQueryForSilverCustomers();
            RegisterContinousQueryForBronzeCustomers();

            Thread printThread = new Thread(PrintValuableCustomersCount);
            printThread.Start();

            Console.ReadLine();
        }
    }
}
