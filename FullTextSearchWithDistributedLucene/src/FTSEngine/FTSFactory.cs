// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using NCommerce.Common;

namespace NCommerce.FTSEngine
{
    public class FTSFactory
    {
        public static IndexManager GetIndexManager()
        {
            return new IndexManager(Util.GetAppSettings());
        }
        public static SearchEngine GetSearchEngine()
        {
            return new SearchEngine(Util.GetAppSettings());
        }
    }
}
