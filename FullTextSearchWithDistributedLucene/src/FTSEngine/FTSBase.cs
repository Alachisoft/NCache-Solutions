// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using NCommerce.Common;
using NCommerce.Common.Models;
using System;
using System.Collections.Generic;

namespace NCommerce.FTSEngine
{
    public abstract class FTSBase : IDisposable
    {
        protected readonly BaseDirectory indexDirectory;

        protected static IndexWriter indexWriter;
        protected readonly IndexWriterConfig indexWriterConfig;
        protected readonly Analyzer analyzer;

        //protected readonly string[] Fields = new[] { "description" };
        protected readonly string[] Fields = new[] { "name", "description" };

        protected const ushort BULK_SIZE = 1000;
        public LuceneVersion Version { get { return LuceneVersion.LUCENE_48; } }
        public IndexWriter IndexWriter { get { return indexWriter; } internal set { indexWriter = value; } }

        public FTSBase(AppSettings appSettings)
        {
            analyzer = new StandardAnalyzer(this.Version);
            //analyzer = new Lucene.Net.Analysis.En.EnglishAnalyzer(this.Version);

            //indexDirectory = FSDirectory.Open(new DirectoryInfo(appSettings.IndexPath));
            indexDirectory = NCacheDirectory.Open(appSettings.LuceneCacheName, appSettings.IndexPath);

            //--- Create the configuration for IndexWriter
            indexWriterConfig = new IndexWriterConfig(this.Version, analyzer);
            if (indexWriter == null)
                indexWriter = new IndexWriter(indexDirectory, indexWriterConfig);
        }

        protected List<ProductFTS> GetSearchedDocs(IndexSearcher searcher, TopDocs topDocs)
        {
            var prodList = new List<ProductFTS>();

            //ScoreDocs return top hits for the current search
            foreach (var result in topDocs.ScoreDocs)
            {
                //Get the actual document by providing its id
                var doc = searcher.Doc(result.Doc);

                prodList.Add(doc.GetProductFTS());
            }

            return prodList;
        }

        public void Dispose()
        {
            indexWriter?.Dispose();
        }
    }
}
