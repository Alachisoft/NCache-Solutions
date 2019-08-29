// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using NCommerce.Common.Models;

namespace NCommerce.Common.DataProviders
{
    public interface IDataProvider : IDisposable
    {
        string ConnectionString { get; }
        bool IsDisposed { get; }

        IList<string> GetCategories();
        Product GetProduct(string prodId);
        ProductEnumerator GetProductEnumerator();
        ProductFTSEnumerator GetProductFTSEnumerator();
    }
}