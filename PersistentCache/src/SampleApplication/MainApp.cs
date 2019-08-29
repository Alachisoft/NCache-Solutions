using System;
using System.Collections.Generic;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Alachisoft.NCache.PersistenceStore;

namespace Test
{
    class MainApp
    {
        /// <summary>
        /// instance of cache on which operations are performed
        /// </summary>
        private ICache _cache;
        private string _cacheId;
        private JSonSerializer _serializer = new JSonSerializer();

        #region properties being used to fetch and store user data
        public Customer Customer{ get; set; }
        public int AbsoluteExpirationSeconds { get; set; }

        #endregion

        #region global provider names
        private string ReadThruProviderName { get; set; }
        private string WriteThruProviderName { get; set; }
        #endregion

        #region Display Functions

        /// <summary>
        /// Displays customer data
        /// </summary>
        /// <param name="customer">object containing customer details</param>
        public void DisplayCustomerData(Customer customer)
        {
            Console.WriteLine("Customer ID: " + (Customer.CustomerID = customer.CustomerID));
            Console.WriteLine("Contact Name: " + (Customer.ContactName = customer.ContactName));
            Console.WriteLine("Contact Number: " + (Customer.ContactNo = customer.ContactNo));
            Console.WriteLine("Fax: " + (Customer.Fax = customer.Fax));
            Console.WriteLine("Company: " + (Customer.CompanyName = customer.CompanyName));
            Console.WriteLine("Address: " + (Customer.Address = customer.Address));
            Console.WriteLine("City: " + (Customer.City = customer.City));
            Console.WriteLine("Postal Code: " + (Customer.PostalCode = customer.PostalCode));
            Console.WriteLine("Country: " + (Customer.Country = customer.Country));

        }


        #endregion

        #region Input Methods

        /// <summary>
        /// Take main menu input from user
        /// </summary>
        /// <returns></returns>
        public MainMenu InputMainMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine();
            Console.WriteLine("1 - Get Customer Detail");
            Console.WriteLine("2 - Add New Customer");
            Console.WriteLine("3 - Add New Customer with expiration");
            Console.WriteLine("4 - Add 100 dummy customers");
            Console.WriteLine("5 - Restart Cache for Startuploader");
            Console.WriteLine("6 - End Execution");
            Console.WriteLine();
            Console.Write("Enter Option: ");

            try
            {
                // convert input to integer
                int choice = Convert.ToInt32(Console.ReadLine());

                // validate input; ensure the input is between required numbers
                if (choice >= 1 && choice < 7)
                    return (MainMenu) choice;
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// take customer data input from user
        /// </summary>
        public void InputDataFromUser(ExpirationType expirationType=ExpirationType.None)
        {
            Console.WriteLine("Enter the following information");

            string input;
            Console.Write("Customer ID: ");
            Customer.CustomerID = ((input = Console.ReadLine()) != "") ? input : Customer.CustomerID;
            Console.Write("Contact Name: ");
            Customer.ContactName = ((input = Console.ReadLine()) != "") ? input : Customer.ContactName;
            Console.Write("Company Name: ");
            Customer.CompanyName = ((input = Console.ReadLine()) != "") ? input : Customer.CompanyName;
            Console.Write("Address: ");
            Customer.Address = ((input = Console.ReadLine()) != "") ? input : Customer.Address;
            Console.Write("City: ");
            Customer.City = ((input = Console.ReadLine()) != "") ? input : Customer.City;
            Console.Write("Country: ");
            Customer.Country = ((input = Console.ReadLine()) != "") ? input : Customer.Country;
            Console.Write("Postal Code: ");
            Customer.PostalCode = ((input = Console.ReadLine()) != "") ? input : Customer.PostalCode;
            Console.Write("Phone Number: ");
            Customer.ContactNo = ((input = Console.ReadLine()) != "") ? input : Customer.ContactNo;
            Console.Write("Fax Number: ");
            Customer.Fax = ((input = Console.ReadLine()) != "") ? input : Customer.Fax;
            if(expirationType.Equals(ExpirationType.Absolute))
                Console.Write("Enter seconds for Absolute expiration: ");
            try
            {
                AbsoluteExpirationSeconds = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Absolute expiration must be set in seconds in int value");
            }

        }
        /// <summary>
        /// Add dummy customers to cache
        /// </summary>
        public void AddDummyCustomerToCache()
        {
            Console.WriteLine("Enter the following information");
            for(int i=0;i<100;i++)
            {
                Customer=DummyData.GetDummyCustomer("Customer:" + i);
                StoreNewCustomerData();
            }
        }

        #endregion

        /// <summary>
        /// Insert customer data to cache
        /// </summary>
        public void StoreNewCustomerData(ExpirationType expirationType=ExpirationType.None)
        {
            try
            {               
                var cacheItem=new CacheItem(_serializer.Serialize(Customer));
                if(expirationType.Equals(ExpirationType.Absolute))
                    cacheItem.Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 0, 10));
                _cache.Insert(Customer.CustomerID.ToUpper(), cacheItem, new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName));
                Console.WriteLine("Customer information Inserted successfuly");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + "Error: " + e.Message);
            }
        }

        /// <summary>
        /// method for functioning
        /// </summary>
        public void Run()
        {
            // initialize cache before any operations
            InitializeCache();

            ReadThruProviderName = "PersistenceReadThruProvider";
            WriteThruProviderName = "PersistenceWritethruProvider";

            // initialize customer object
            Customer customer = null;

            bool isRunning = true;
            // clearing cache to see readthru/writethru changes
            _cache.Clear();
            while (isRunning)
            {
                try
                {
                    // take input according to main menu
                    switch (InputMainMenu())
                    {
                        // 1 - Get Customer Detail
                        case MainMenu.GetCacheItem:

                            // take customer ID from user
                            Console.Write("Enter Customer ID: ");
                            Customer.CustomerID = Convert.ToString(Console.ReadLine());


                            var jValue = _cache.Get<string>(Customer.CustomerID, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));
                            if (jValue == null)
                            {
                                throw new Exception("Item does not exists in persistent cache");
                            }
                            customer = JsonConvert.DeserializeObject<Customer>(jValue);

                            if (customer != null)
                            {
                                // display details of entered customer ID
                                Console.WriteLine("\n" + "Fetched Customer Details [readthru]: ");

                                DisplayCustomerData(customer);
                            }
                            else
                            {
                                Console.WriteLine("\nCustomer with provided ID does not exist.");
                            }

                            break;
                        // 2 - Add Customer Detail
                        case MainMenu.AddCacheItem:

                            InputDataFromUser();
                            StoreNewCustomerData();
                            break;

                        // 3 - Add dummy Customer Detail
                        case MainMenu.AddDummyItems:
                            AddDummyCustomerToCache();
                            break;

                        // 4 - Add with Expiration
                        case MainMenu.AddWithExpiration:

                            InputDataFromUser(ExpirationType.Absolute);
                            StoreNewCustomerData(ExpirationType.Absolute);
                            break;

                        // 5 - Restart Cache
                        case MainMenu.RestartCache:
                            // take customer ID from user
                            RestartCache();
                            break;

                        // 6 - End execution
                        case MainMenu.EndExecution:
                            Console.WriteLine("Finished Execution");
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }                
            }
        }

        /// <summary>
        /// initialize cache
        /// </summary>
        private void InitializeCache()
        {
            //Name of cache to connect with
            // Providers must be deployed with that cache
            //Name of provider to be deployed
            //PersistenceReadThru , PersistenceWriteThru and PersistenceStartupLoadewr
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string cacheId = configuration["NCache:cacheName"];
            if (String.IsNullOrEmpty(cacheId))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }
            _cacheId=cacheId;
            // Initialize an instance of the cache to begin performing operations:
            _cache = CacheManager.GetCache(cacheId);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cacheId));
        }

        /// <summary>
        /// Restart cache and wait for startuploader
        /// </summary>
        private void RestartCache()
        {
            Console.WriteLine("Restarting Cache");
            CacheManager.StopCache(_cacheId);
            Console.WriteLine("Cache stopped");

            CacheManager.StartCache(_cacheId);
            Console.WriteLine("Cache started again");

            Console.WriteLine("Waiting for 20 seconds for startuploader");
            Thread.Sleep(20 * 1000);

            _cache =CacheManager.GetCache(_cacheId);
        }

        #region Helper Functions

        public enum MainMenu
        {
            GetCacheItem=1,
            AddCacheItem,
            AddWithExpiration,
            AddDummyItems,
            RestartCache,
            EndExecution
        }

        #endregion
    }
}