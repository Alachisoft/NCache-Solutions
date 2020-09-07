using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Samples.Dapper.ConsoleUI.Repository;
using Alachisoft.NCache.Samples.Dapper.ConsoleUI.SeedData;
using Alachisoft.NCache.Samples.Dapper.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Alachisoft.NCache.Samples.Dapper.ConsoleUI
{
    public static class DapperDemo
    {
        static readonly ICache cache;
        static readonly string connectionString;
        static readonly bool useCache;
        static DapperDemo()
        {
            var cacheId = ConfigurationManager.AppSettings["cacheId"];

            cache = CacheManager.GetCache(cacheId);

            connectionString =
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

            useCache = bool.Parse(ConfigurationManager.AppSettings["UseCache"]);

        }

        public static void Run()
        {
            var done = false;

            while (!done)
            {
                switch (GetUserChoice())
                {
                    case 1:
                        {
                            IEnumerable<string> customerIds;

                            var stopWatch = new Stopwatch();

                            if (useCache)
                            {
                                stopWatch.Start();
                                customerIds =
                                    cache.DataTypeManager.GetList<string>(
                                                    "Customer:CustomerID:All",
                                                    new ReadThruOptions
                                                    {
                                                        Mode = ReadMode.ReadThru
                                                    });
                                stopWatch.Stop();
                            }
                            else
                            {
                                stopWatch.Start();
                                customerIds =
                                    CustomerRepository.GetCustomerIDs();
                                stopWatch.Stop();
                            }

                            var ms = stopWatch.ElapsedMilliseconds;
                            Console.WriteLine($"Total operation time: {ms} ms");
                            if (customerIds.Count() == 0)
                            {
                                Console.WriteLine("No customers in database");
                            }
                            else
                            {
                                PrintCustomerIDs(customerIds);
                            }
                        }
                        break;
                    case 2:
                        {
                            var customerID = GetCustomerID();
                            var stopWatch = new Stopwatch();
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                Customer customer;
                                if (useCache)
                                {
                                    stopWatch.Start();
                                    customer =
                                        cache.Get<Customer>(
                                                  $"Customer:CustomerID:{customerID}",
                                                  new ReadThruOptions(
                                                            ReadMode.ReadThru));
                                    stopWatch.Stop();
                                }
                                else
                                {
                                    stopWatch.Start();
                                    customer =
                                        CustomerRepository.GetCustomer(customerID);
                                    stopWatch.Stop();
                                }

                                var ms = stopWatch.ElapsedMilliseconds;
                                Console.WriteLine($"Total operation time: {ms} ms");
                                PrintCustomerDetails(customer);
                            }
                        }
                        break;
                    case 3:
                        {
                            var customerID = GetCustomerID();
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                if (useCache)
                                {
                                    cache.Remove(
                                            $"Customer:CustomerID:{customerID}",
                                            null,
                                            null,
                                            new WriteThruOptions(
                                                        WriteMode.WriteThru)); 
                                }
                                else
                                {
                                    CustomerRepository.DeleteCustomer(customerID);
                                }
                            }
                        }
                        break;
                    case 4:
                        {
                            var customer = CustomerSeed.SeedCustomers()
                                .Generate();

                            Console.WriteLine("New customer:\n");
                            PrintCustomerDetails(customer);

                            if (useCache)
                            {
                                cache.Add(
                                      $"Customer:CustomerID:{customer.CustomerID}",
                                      new CacheItem(customer)
                                      {
                                          Dependency = GetCustomerSqlDependency(
                                                                    customer.CustomerID),
                                          ResyncOptions = new ResyncOptions(true)
                                      },
                                      new WriteThruOptions
                                      {
                                          Mode = WriteMode.WriteThru
                                      }); 
                            }
                            else
                            {
                                CustomerRepository.SaveCustomer(customer);
                            }
                        }
                        break;
                    case 5:
                        {
                            var customerID = GetCustomerID();
                            Customer oldCustomer = null;
                            
                            if (!string.IsNullOrWhiteSpace(customerID))
                            {
                                if (useCache)
                                {
                                    oldCustomer =
                                         cache.Get<Customer>(
                                                    $"Customer:CustomerID:{customerID}",
                                                    new ReadThruOptions
                                                    {
                                                        Mode = ReadMode.ReadThru
                                                    }); 
                                }
                                else
                                {
                                    oldCustomer =
                                        CustomerRepository.GetCustomer(customerID);
                                }
                            }

                            if (oldCustomer == null)
                            {
                                Console.WriteLine(
                                    $"No customer with ID {customerID} exists " +
                                    $"in database");
                            }
                            else
                            {
                                var newCustomer = CustomerSeed.SeedCustomers()
                                                                    .Generate();

                                newCustomer.CustomerID = customerID;

                                Console.WriteLine("Updated customer:\n");
                                PrintCustomerDetails(newCustomer);

                                if (useCache)
                                {
                                    cache.Insert(
                                            $"Customer:CustomerID:{customerID}",
                                            new CacheItem(newCustomer)
                                            {
                                                Dependency = GetCustomerSqlDependency(
                                                                  customerID),
                                                ResyncOptions = new ResyncOptions(true)
                                            },
                                            new WriteThruOptions
                                            {
                                                Mode = WriteMode.WriteThru
                                            }); 
                                }
                                else
                                {
                                    CustomerRepository.UpdateCustomer(newCustomer);
                                }
                            }
                        }
                        break;
                    case 6:
                        {
                            var country = GetCustomerCountry();

                            if (!string.IsNullOrWhiteSpace(country))
                            {
                                IEnumerable<string> customerIds;

                                var stopWatch = new Stopwatch();
                                if (useCache)
                                {
                                    stopWatch.Start();
                                    customerIds =
                                        cache.DataTypeManager.GetList<string>(
                                                    $"Customer:Country:{country}",
                                                    new ReadThruOptions
                                                    {
                                                        Mode = ReadMode.ReadThru
                                                    });
                                    stopWatch.Stop();
                                }
                                else
                                {
                                    stopWatch.Start();
                                    customerIds =
                                        CustomerRepository.GetCustomerIDsByCountry(
                                                                            country);
                                    stopWatch.Stop();
                                }

                                var ms = stopWatch.ElapsedMilliseconds;
                                Console.WriteLine($"Total operation time: {ms} ms");

                                if (customerIds.Count() == 0)
                                {
                                    Console.WriteLine(
                                        $"No customers in database with country " +
                                        $" {country}");
                                }
                                else
                                {
                                    PrintCustomerIDs(customerIds);
                                }
                            }
                        }
                        break;
                    case 7:
                        {
                            done = true;
                        }
                        break;

                }
            }
        }
        private static int GetUserChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(" 1- View customers list");
            Console.WriteLine(" 2- View customer details");
            Console.WriteLine(" 3- Delete customer");
            Console.WriteLine(" 4- Add customer");
            Console.WriteLine(" 5- Update customer");
            Console.WriteLine(" 6- View customers by country");
            Console.WriteLine(" 7- Exit");
            Console.WriteLine("");

            Console.Write("Enter your choice (1 - 7): ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice >= 1 && choice <= 7)
                    return choice;
            }
            catch (Exception)
            {
            }
            Console.WriteLine("Please enter a valid choice (1 - 7)");
            return GetUserChoice();
        }

        private static string GetCustomerID()
        {
            Console.Write("Please enter the customer Id: ");
            string result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine("customer id can not be empty.");
                return null;
            }

            return result.ToUpperInvariant();
        }

        private static string GetCustomerCountry()
        {
            Console.Write("Please enter the country: ");
            string result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine("customer country can not be empty.");
                return null;
            }

            return result;
        }

        private static void PrintCustomerIDs(IEnumerable<string> customerIds)
        {
            int i = 1;
            foreach (var customerId in customerIds)
            {
                Console.WriteLine($"{i++}-{customerId}");
            }

        }

        private static void PrintCustomerDetails(Customer customer)
        {
            if (customer != null)
            {
                Console.WriteLine("Customer's Details");
                Console.WriteLine("------------------");

                Console.WriteLine("Customer ID : " + customer.CustomerID);
                Console.WriteLine("Name        : " + customer.ContactName);
                Console.WriteLine("Company     : " + customer.CompanyName);
                Console.WriteLine("Address     : " + customer.Address);
                Console.WriteLine("Country     : " + customer.Country);
            }
            else
            {
                Console.WriteLine("No such customer exists.");
            }
        }


        private static SqlCacheDependency GetCustomerSqlDependency(
                                                            string customerId)
        {
            var sqlQuery =  $"SELECT CustomerID, " +
                            $"       CompanyName, " +
                            $"       ContactName, " +
                            $"       ContactTitle, " +
                            $"       City, " +
                            $"       Country, " +
                            $"       Region, " +
                            $"       PostalCode, " +
                            $"       Phone, " +
                            $"       Fax, " +
                            $"       Address " +
                            $"FROM dbo.Customers " +
                            $"WHERE CustomerID = @cid";

            var cmdParam = new SqlCmdParams
            {
                Value = customerId,
                Size = 255,
                Type = CmdParamsType.NVarChar
            };

            var sqlCmdParams = new Dictionary<string, SqlCmdParams>
            {
                {"@cid", cmdParam }
            };

            return new SqlCacheDependency(
                                    connectionString,
                                    sqlQuery,
                                    SqlCommandType.Text,
                                    sqlCmdParams);
        }

    }
}
