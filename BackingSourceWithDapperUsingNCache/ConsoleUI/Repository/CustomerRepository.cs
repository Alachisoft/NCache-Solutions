using Alachisoft.NCache.Samples.Dapper.Models;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Alachisoft.NCache.Samples.Dapper.ConsoleUI.Repository
{
    internal static class CustomerRepository
    {
        static readonly string connectionString;
        static CustomerRepository()
        {
            connectionString =
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
        }

        public static IEnumerable<string> GetCustomerIDs()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return connection.Query<string>(
                            "SELECT CustomerID FROM dbo.Customers");
            }
        }

        public static Customer GetCustomer(string customerId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return connection.Query<Customer>(
                            "SELECT * " +
                            "FROM dbo.Customers " +
                            "WHERE CustomerID = @cId",
                            new { cId = customerId})
                    .FirstOrDefault();
            }
        }

        public static bool DeleteCustomer(string customerId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.Execute(
                     "DELETE FROM dbo.Customers " +
                     "WHERE CustomerID = @cId",
                     new { cId = customerId });

                return true;
            }
        }

        public static bool SaveCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int rowsAffected = connection.Execute(
                    @"INSERT INTO dbo.Customers (
                                CustomerID, 
                                CompanyName, 
                                ContactName, 
                                ContactTitle, 
                                Address, 
                                City, 
                                Region, 
                                PostalCode, 
                                Country, 
                                Phone, 
                                Fax) 
                        VALUES (
                                @CustomerID, 
                                @CompanyName, 
                                @ContactName, 
                                @ContactTitle, 
                                @Address, 
                                @City, 
                                @Region, 
                                @PostalCode, 
                                @Country, 
                                @Phone, 
                                @Fax);",
                    customer);

                return rowsAffected > 0;
            }
        }

        public static bool UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int rowsAffected = connection.Execute(
                    @"UPDATE dbo.Customers 
                      SET    CompanyName = @CompanyName, 
                             ContactName = @ContactName, 
                             ContactTitle = @ContactTitle, 
                             Address = @Address, 
                             City = @City, 
                             Region = @Region, 
                             PostalCode = @PostalCode, 
                             Country = @Country, 
                             Phone = @Phone, 
                             Fax = @Fax
                      WHERE CustomerID = @CustomerID;",
                    customer);

                return rowsAffected > 0;
            }
        }

        public static IEnumerable<string> GetCustomerIDsByCountry(
                                                            string country)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return connection.Query<string>(
                            "SELECT CustomerID " +
                            "FROM dbo.Customers " +
                            "WHERE Country = @cntry",
                            new { cntry = country });
            }
        }
    }
}
