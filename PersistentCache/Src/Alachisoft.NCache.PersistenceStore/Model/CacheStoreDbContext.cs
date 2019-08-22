// Copyright (c) 2019 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos.Storage.Internal;
using PersistenceProviders.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersistentNCache.Model
{
    public class CacheStoreDbContext:DbContext
    {        
        public DbSet<StoreItem> CacheItem { get; set; }
        public DbSet<DataTypeItem> dataTypeItem{ get; set; }

        string _connectionString = "";
        DbType dbType = DbType.SqlServer;

        string _serviceEndPoint;
        string _authKey;
        string _databaseName;

        public CacheStoreDbContext()
        {
            _connectionString = "Data Source =.; Initial Catalog = NCacheDb; Integrated Security = false; User ID = sa; password=4Islamabad";
        }
        public CacheStoreDbContext(string connectionString= "Data Source =.; Initial Catalog = PersistentStore; Integrated Security = false; User ID = sa; password=4Islamabad")
        {
            dbType = DbType.SqlServer;
            _connectionString = connectionString;
        }
        //"https://localhost:8081~C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==~NCacheDb"
        public CacheStoreDbContext(string serviceEndPoint, string authKey, string databaseName, DbContextOptions<CacheStoreDbContext> options) : base(options)
        {
            dbType = DbType.Cosmos;

            _serviceEndPoint = serviceEndPoint;
            _authKey = authKey;
            _databaseName = databaseName;
        }       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = _connectionString;
            if (dbType.Equals(DbType.Cosmos))
            {
                
                optionsBuilder.UseCosmos(_serviceEndPoint,_authKey,_databaseName,
                    options=>
                    {
                        options.ExecutionStrategy(d => new CosmosExecutionStrategy(d));
                    }
                    );
            }
            else
            {
                optionsBuilder.UseSqlServer(connString);
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<StoreItem>().Property(p => p.Key).HasValueGenerator<>();
        //    modelBuilder.Entity<Car>().Property(p => p.CarId).HasValueGenerator<GuidValueGenerator>();
        //}
    }
}
