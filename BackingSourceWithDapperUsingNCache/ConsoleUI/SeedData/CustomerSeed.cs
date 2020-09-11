using Alachisoft.NCache.Samples.Dapper.Models;
using Bogus;
using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Alachisoft.NCache.Samples.Dapper.ConsoleUI.SeedData
{
    public static class CustomerSeed
    {
        static readonly string connectionString;
        static readonly int noOfInstances;
        static CustomerSeed()
        {
            Randomizer.Seed = new Random(8675309);

            connectionString = 
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

            noOfInstances =
                int.Parse(ConfigurationManager.AppSettings["noOfInstances"]);
        }

        public static void Initialize()
        {
            var customers = SeedCustomers().Generate(noOfInstances).ToList();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var database = connection.Database;

                connection.Execute($@"
                USE [{database}]
                ALTER DATABASE {database} SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;
                ALTER DATABASE {database} SET ANSI_NULLS ON;
                ALTER DATABASE {database} SET ANSI_PADDING ON;
                ALTER DATABASE {database} SET ANSI_WARNINGS ON;
                ALTER DATABASE {database} SET CONCAT_NULL_YIELDS_NULL ON;
                ALTER DATABASE {database} SET QUOTED_IDENTIFIER ON;
                ALTER DATABASE {database} SET NUMERIC_ROUNDABORT OFF;
                ALTER DATABASE {database} SET ARITHABORT ON;
                if OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
                CREATE TABLE dbo.Customers(
                     CustomerID nvarchar(255) NOT NULL,
                     CompanyName nvarchar(255) NOT NULL,
                     ContactName nvarchar(255),
                     ContactTitle nvarchar(255),
                     Address nvarchar(255),
                     City nvarchar(255),
                     Region nvarchar(255),
                     PostalCode nvarchar(255),
                     Country nvarchar(255),
                     Phone nvarchar(255),
                     Fax nvarchar(255),
                     PRIMARY KEY(CustomerID)); ");

                connection.Execute(
                    @"INSERT INTO Customers (
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
                    customers);

            }
        }

        public static Faker<Customer> SeedCustomers()
        {
            return new Faker<Customer>()
                .RuleFor(
                        x => x.Address, 
                        faker => faker.Address.StreetAddress())
                .RuleFor(
                        x => x.City, 
                        faker => faker.Address.City())
                .RuleFor(
                        x => x.ContactName, 
                        faker => faker.Name.FullName())
                .RuleFor(
                        x => x.ContactTitle, 
                        faker => faker.Name.JobTitle())
                .RuleFor(
                        x => x.CompanyName, 
                        faker => faker.Company.CompanyName())
                .RuleFor(
                        x => x.Country, 
                        faker => faker.PickRandom<string>(new[] 
                                                            { 
                                                                "USA",
                                                                "UK",
                                                                "France"
                                                            }))
                .RuleFor(
                        x => x.Region, 
                        faker => faker.Address.State())
                .RuleFor(
                        x => x.Phone, 
                        faker => faker.Phone.PhoneNumber())
                .RuleFor(
                        x => x.Postalcode, 
                        faker => faker.Address.ZipCode())
                .RuleFor(
                        x => x.Fax, 
                        faker => faker.Phone.PhoneNumber())
                .RuleFor(
                        x => x.CustomerID, 
                        faker => faker.Random.String2(5,5).ToUpperInvariant());
        }
    }
}
