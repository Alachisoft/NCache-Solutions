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
using System;
using System.Collections.Generic;
using System.Text;

namespace Alachisoft.NCache.PersistenceStore
{
    /// <summary>
    /// Db context class against the database
    /// </summary>
    public class PersistentStoreContext:DbContext
    {        
        public DbSet<StoreItem> CacheItem { get; set; }

        string _connectionString = default(string);
        PersistentStoreType _dbType = PersistentStoreType.SqlServer;

        string _serviceEndPoint;
        string _authKey;
        string _databaseName;

        public PersistentStoreContext(string connectionString)
        {
            _dbType = PersistentStoreType.SqlServer;
            _connectionString = connectionString;
        }
        //"https://localhost:8081~C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==~NCacheDb"
        public PersistentStoreContext(string serviceEndPoint, string authKey, string databaseName, DbContextOptions<PersistentStoreContext> options) : base(options)
        {
            _dbType = PersistentStoreType.Cosmos;

            _serviceEndPoint = serviceEndPoint;
            _authKey = authKey;
            _databaseName = databaseName;
        }       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = _connectionString;
            if (_dbType.Equals(PersistentStoreType.Cosmos))
            {
                optionsBuilder.UseCosmos(_serviceEndPoint, _authKey, _databaseName);
            }
            else
            {
                optionsBuilder.UseSqlServer(connString);
            }
        }

    }
}
