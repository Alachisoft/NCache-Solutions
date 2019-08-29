// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using NCommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NCommerce.Common.DataProviders
{
    public class SQLProvider : IDataProvider
    {
        readonly SqlConnection _sqlConnection;

        public string ConnectionString { get; }
        public bool IsDisposed { get; private set; }

        public SQLProvider(string connString)
        {
            ConnectionString = connString ?? throw new ArgumentNullException(nameof(connString));
            _sqlConnection = new SqlConnection(ConnectionString);
            _sqlConnection.Open();
        }

        public IList<string> GetCategories()
        {
            ThrowErrorIfDisposed();
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText = "SELECT DISTINCT(category) FROM products ORDER BY category",
                Connection = _sqlConnection,
            };

            IList<string> catList = new List<string>();
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    catList.Add(reader.GetString(0));
                }
            }

            return catList;
        }
        public Product GetProduct(string prodId)
        {
            ThrowErrorIfDisposed();
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText = "SELECT * FROM products WHERE id = '" + prodId + "'",
                Connection = _sqlConnection,
            };

            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader.GetProduct();
                }
            }

            return null;
        }

        public ProductEnumerator GetProductEnumerator()
        {
            ThrowErrorIfDisposed();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM products");
            sqlCommand.Connection = new SqlConnection(ConnectionString);
            sqlCommand.Connection.Open();

            return new ProductEnumerator(sqlCommand.ExecuteReader());
        }

        public ProductFTSEnumerator GetProductFTSEnumerator()
        {
            ThrowErrorIfDisposed();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM products");
            sqlCommand.Connection = new SqlConnection(ConnectionString);
            sqlCommand.Connection.Open();

            return new ProductFTSEnumerator(sqlCommand.ExecuteReader());
        }

        public void Dispose()
        {
            IsDisposed = true;
            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }

        private void ThrowErrorIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Unable to perform the operation. This provider has been disposed!");
        }
    }
}
