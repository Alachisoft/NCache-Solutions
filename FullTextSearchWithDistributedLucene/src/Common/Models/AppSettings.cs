// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.IO;

namespace NCommerce.Common.Models
{
    public class AppSettings
    {
        private string _conString;
        private string _luceneCacheName = "luceneCache";
        private string _dataCacheName = "dataCache";
        private string _indexPath = @"lucene-index";

        public AppSettings()
        {
        }

        public string LuceneCacheName
        {
            get { return _luceneCacheName; }
            internal set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("'LuceneCacheName' IsNullOrEmpty or IsNullWhiteSpace!");

                _luceneCacheName = value;
            }
        }

        public string DataCacheName
        {
            get { return _dataCacheName; }
            internal set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("'DataCacheName' IsNullOrEmpty or IsNullOrWhiteSpace!");

                _dataCacheName = value;
            }
        }

        public string IndexPath
        {
            get
            {
                if (UseCustomIndexPath)
                {
                    string fullPath = Path.GetFullPath(_indexPath).TrimEnd(Path.DirectorySeparatorChar);
                    return Path.GetFileName(fullPath);
                }
                else
                {
                    return _indexPath;
                }
            }

            internal set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("'IndexPath' IsNullOrEmpty or IsNullOrWhiteSpace!");

                _indexPath = value;
            }
        }

        public bool UseCustomIndexPath { get; set; } = true;

        public string ConnectionString
        {
            get { return _conString; }
            internal set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("'ConnectionString' IsNullOrEmpty or IsNullWhiteSpace!");

                _conString = value;
            }
        }

    }
}
