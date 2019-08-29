// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using NCommerce.Common.Models;
using System;
using System.Collections.Generic;

namespace NCommerce.FTSEngine
{
    public class SearchEngine : FTSBase
    {
        const int TOP_LIMIT = 100;
        const string CATEGORY_FIELD = "category";

        public SearchEngine(AppSettings appSettings) : base(appSettings)
        {
            // Note: Use the same anaylyzer which you have used for indexing
        }

        public Tuple<long, List<ProductFTS>> Search(string searchTerm, int top = TOP_LIMIT, string category = null)
        {
            long totalHits = 0;
            var prodList = new List<ProductFTS>();

            try
            {
                TopDocs topDocs = null;
                var queryParser = new MultiFieldQueryParser(Version, Fields, analyzer);

                var query = new BooleanQuery();
                //--- For exact match use the queryParser.Parse(searchTerm)
                query.Add(queryParser.Parse(searchTerm), Occur.MUST);

                //--- Split the search term into multiple search words to make fuzzy query ...
                string[] terms = searchTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string term in terms)
                    query.Add(queryParser.Parse(term.Replace("~", "") + "~"), Occur.MUST);// remove the duplicate ~, if already exists

                //--- Get the new instance of IndexSearcher, to make sure that query is being performed on the updated index
                var searcher = GetIndexSearcher();
                if (!string.IsNullOrEmpty(category))
                {
                    var filter = new QueryWrapperFilter(new TermQuery(new Term(CATEGORY_FIELD, category)));

                    topDocs = searcher.Search(query, filter, top, Sort.RELEVANCE);
                }
                else
                {
                    topDocs = searcher.Search(query, top, sort: Sort.RELEVANCE);
                }

                totalHits = topDocs.TotalHits;

                prodList = GetSearchedDocs(searcher, topDocs);
            }
            catch (Exception exc)
            {
                throw exc;
            }

            return new Tuple<long, List<ProductFTS>>(totalHits, prodList);
        }

        public Tuple<long, List<ProductFTS>> GetAllDocuments()
        {
            //--- Get the new instance of IndexSearcher, to make sure that query is being performed on the updated index
            var searcher = GetIndexSearcher();

            var directoryReader = DirectoryReader.Open(indexWriter.Directory);
            //Console.WriteLine($"MaxDoc: {directoryReader.MaxDoc} > NumDocs: {directoryReader.NumDocs} > RefCount: {directoryReader.RefCount}");

            var topDocs = searcher.Search(new MatchAllDocsQuery(), (int)directoryReader.MaxDoc);
            //Console.WriteLine("Total Hits: " + topDocs.TotalHits);

            var products = GetSearchedDocs(searcher, topDocs);

            return new Tuple<long, List<ProductFTS>>(topDocs.TotalHits, products);
        }

        private IndexSearcher GetIndexSearcher()
        {
            //var directoryReader = DirectoryReader.Open(indexDirectory);
            var directoryReader = DirectoryReader.Open(indexWriter.Directory);
            //var directoryReader = DirectoryReader.Open(indexWriter, true);

            //Create the IndexSearcher instance to use it for searching ...
            return new IndexSearcher(directoryReader);
        }
    }
}
