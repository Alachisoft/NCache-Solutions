using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Samples.Dapper.Models;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Alachisoft.NCache.Samples.Dapper.BackingSources
{
    public class ReadThruProvider : IReadThruProvider
    {
        string ConnectionString;
        public SqlConnection Connection { get; set; }
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public void Init(
                        IDictionary parameters,
                        string cacheId)
        {
            ConnectionString = parameters["connectionString"] as string;
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(
                                                        string key, 
                                                        DistributedDataType dataType)
        {
            if (key.StartsWith("Customer:Country:"))
            {
                string country = key.Replace("Customer:Country:", "");

                var commandDefinition = new CommandDefinition(
                                                    $@" SELECT CustomerID 
                                                        FROM dbo.Customers 
                                                        WHERE Country = @cntry",
                                                    new { cntry = country }, 
                                                    flags: CommandFlags.NoCache);
                var customerIds = Connection.Query<string>(
                                                commandDefinition);

                return new ProviderDataTypeItem<IEnumerable>(customerIds)
                {
                    Dependency = GetCustomerIDsByCountrySqlDependency(country),
                    ResyncOptions = new ResyncOptions(true),
                    Expiration = new Expiration(
                                            ExpirationType.Absolute,
                                            TimeSpan.FromMinutes(10))
                };

            }
            else if (key == "Customer:CustomerID:All")
            {
                var commandDefinition = new CommandDefinition(
                                                    $@" SELECT CustomerID 
                                                        FROM dbo.Customers",
                                                    flags: CommandFlags.NoCache);

                var customerIds = Connection.Query<string>(
                                                commandDefinition);

                return new ProviderDataTypeItem<IEnumerable>(customerIds)
                {
                    Dependency = GetCustomerIDsSqlDependency(),
                    ResyncOptions = new ResyncOptions(true),
                    Expiration = new Expiration(
                                            ExpirationType.Absolute,
                                            TimeSpan.FromMinutes(10))
                };
            }

            return null;
        }

        public ProviderCacheItem LoadFromSource(
                                            string key)
        {
            if (key.StartsWith("Customer:CustomerID:"))
            {
                var customerId = key.Replace("Customer:CustomerID:", "").Trim();

                var commandDefinition = new CommandDefinition(
                                                    $@" SELECT * 
                                                        FROM dbo.Customers 
                                                        WHERE CustomerID = @cId",
                                                    new { cid = customerId },
                                                    flags: CommandFlags.NoCache);

                var customer = Connection.Query<Customer>(commandDefinition)
                                            .FirstOrDefault();

                if (customer == null)
                {
                    return null;
                }
                else
                {
                    var providerCacheItem = new ProviderCacheItem(customer)
                    {
                        Dependency = GetCustomerSqlDependency(customerId),
                        ResyncOptions = new ResyncOptions(true)
                    };

                    return providerCacheItem;
                }
            }
            else
            {
                return null;
            }
        }

        public IDictionary<string, ProviderCacheItem> LoadFromSource(
                                                            ICollection<string> keys)
        {
            Dictionary<string, ProviderCacheItem> cacheDict =
                new Dictionary<string, ProviderCacheItem>(keys.Count);


            foreach (var key in keys)
            {
                cacheDict.Add(key, LoadFromSource(key));
            }

            return cacheDict;
        }

        private SqlCacheDependency GetCustomerIDsSqlDependency()
        {
            var sqlQuery = $"SELECT CustomerID " +
                            $"FROM dbo.Customers ";

            return new SqlCacheDependency(
                                    ConnectionString,
                                    sqlQuery);
        }

        private SqlCacheDependency GetCustomerSqlDependency(
                                                            string customerId)
        {
            var sqlQuery = $"SELECT CustomerID, " +
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
                                    ConnectionString,
                                    sqlQuery,
                                    SqlCommandType.Text,
                                    sqlCmdParams);
        }

        private SqlCacheDependency GetCustomerIDsByCountrySqlDependency(
                                                                    string country)
        {
            var sqlQuery = $"SELECT CustomerID " +
                            $"FROM dbo.Customers " +
                            $"WHERE Country = @country";

            var cmdParam = new SqlCmdParams
            {
                Value = country,
                Size = 255,
                Type = CmdParamsType.NVarChar
            };

            var sqlCmdParams = new Dictionary<string, SqlCmdParams>
            {
                {"@country", cmdParam }
            };

            return new SqlCacheDependency(
                                    ConnectionString,
                                    sqlQuery,
                                    SqlCommandType.Text,
                                    sqlCmdParams);
        }
    }
}
