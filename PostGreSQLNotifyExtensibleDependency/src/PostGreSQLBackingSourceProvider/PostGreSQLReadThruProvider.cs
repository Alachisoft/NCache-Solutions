using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Samples.PostGreSQLNotificationDependency;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Entities;

namespace PostGreSQLBackingSourceProvider
{
    public class PostGreSQLReadThruProvider : IReadThruProvider
    {
        private string _connectionString;
        private string _dependencyKey;
        private string _schema;
        private string _table;
        private string _channel;


        private IDbConnection _connection;
        public void Init(IDictionary parameters, string cacheId)
        {
            _connectionString = parameters["connectionString"] as string;
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            throw new NotImplementedException();
        }

        public ProviderCacheItem LoadFromSource(string key)
        {
            // Define a query returning a single row result set (key is passed as a parameter, not concatenated)
            const string query = "SELECT customerid, address, country, city FROM customers WHERE customerid = @customerid";
            using (var command = new NpgsqlCommand(query, _connection as NpgsqlConnection))
            {
                command.Parameters.AddWithValue("customerid", key);
                ProviderCacheItem providerCacheItem = null;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (providerCacheItem == null)
                        {
                            var customer = new Customer()
                            {
                                customerid = reader[0] as string,
                                address = reader[1] as string,
                                country = reader[2] as string,
                                city = reader[3] as string,
                            };
                            providerCacheItem = new ProviderCacheItem(customer)
                            {
                                Dependency = new PostGreSQLDependency(_connectionString, customer.customerid, "public", "customers", "customer_channel"),
                                ResyncOptions = new ResyncOptions(true)
                            };
                        }
                    }
                }

                return providerCacheItem;
            }
        }

        public IDictionary<string, ProviderCacheItem> LoadFromSource(ICollection<string> keys)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
