// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Alachisoft.NCache.Client;
using NCommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NCommerce.Common.DataProviders
{
    public class CacheableDataProvider : IDataProvider
    {
        readonly ICache _cache;
        readonly IDataProvider _provider;
        const string CATEGORIES_KEY = "Product:Categories";

        public string CacheName { get; }
        public string ConnectionString { get { return _provider?.ConnectionString; } }
        public bool IsDisposed { get; private set; }

        public CacheableDataProvider(string cacheName, IDataProvider provider)
        {
            CacheName = cacheName ?? throw new ArgumentNullException(nameof(cacheName));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));

            //--- Connect with cache ...
            _cache = CacheManager.GetCache(cacheName);
        }

        public IList<string> GetCategories()
        {
            ThrowErrorIfDisposed();

            var catList = _cache.Get<IList<string>>(CATEGORIES_KEY);
            if (catList?.Count > 0)
            {
                return catList;
            }
            else//--- Fetch it from database
            {
                catList = _provider.GetCategories();
                //--- Store it in NCache for future use
                _cache.Add(CATEGORIES_KEY, catList);

                return catList;
            }
        }

        public Product GetProduct(string prodId)
        {
            ThrowErrorIfDisposed();
            var prodKey = $"Product:Id:{prodId}";

            Product product = _cache.Get<Product>(prodKey);
            if (product != null)
            {
                return product;
            }
            else//--- Fetch it from database
            {
                product = _provider.GetProduct(prodId);
                if (product != null)
                {
                    //--- Store it in NCache for future use
                    _cache.Insert(prodKey, product);
                }

                return product;
            }
        }

        public ProductEnumerator GetProductEnumerator()
        {
            ThrowErrorIfDisposed();

            return _provider.GetProductEnumerator();
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
            _cache?.Dispose();
            _provider?.Dispose();
        }

        private void ThrowErrorIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Unable to perform the operation. This provider has been disposed!");
        }
    }
}
