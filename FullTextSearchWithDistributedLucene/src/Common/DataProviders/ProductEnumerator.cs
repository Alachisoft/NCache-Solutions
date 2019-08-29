// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using NCommerce.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NCommerce.Common.DataProviders
{
    public class ProductEnumerator : IEnumerator<Product>
    {
        SqlDataReader _reader;

        public ProductEnumerator(SqlDataReader sqlDataRader)
        {
            _reader = sqlDataRader ?? throw new ArgumentNullException(nameof(sqlDataRader));
        }

        public Product Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _reader.Close();
        }

        public bool MoveNext()
        {
            if (_reader.Read())
            {
                Current = _reader.GetProduct();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Current = null;
        }
    }
}
