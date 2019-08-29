// Copyright (c) Alachisoft. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.
// You may obtain a copy of the License from here http://www.apache.org/licenses/LICENSE-2.0

using Lucene.Net.Documents;
using Lucene.Net.Index;
using NCommerce.Common;
using NCommerce.Common.DataProviders;
using NCommerce.Common.Models;
using System.Collections.Generic;

namespace NCommerce.FTSEngine
{
    public class IndexManager : FTSBase
    {
        readonly IDataProvider dataProvider;

        public IndexManager(AppSettings appSettings) : base(appSettings)
        {
            dataProvider = Util.GetDataProvider(false);
        }

        public int CreateIndex()
        {
            int count = 0;
            //--- Fetch data from data source
            using (var enumerator = dataProvider.GetProductFTSEnumerator())
            {
                ushort commitCount = 0;
                while (enumerator.MoveNext())
                {
                    var doc = enumerator.Current.GetLuceneDocument();

                    indexWriter.AddDocument(doc);

                    count++;
                    commitCount++;
                    //--- Instead of performing commit at the end of all records (which might be in thousands) do it after a chunk of documents
                    if (commitCount == BULK_SIZE)
                    {
                        indexWriter.Commit();//Flush and make it ready for search
                        commitCount = 0;
                    }
                }
                //--- Write the last chunk of the documents
                if (commitCount > 0)
                {
                    indexWriter.Commit();//Flush and make it ready for search
                }
            }

            return count;
        }

        public int CreateIndexInBulk()
        {
            int count = 0;
            IList<Document> docs = new List<Document>();
            //--- Fetch data from data source
            using (var enumerator = dataProvider.GetProductFTSEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    docs.Add(enumerator.Current.GetLuceneDocument());

                    count++;
                    //--- Instead of performing commit at the end of all records (which might be in thousands) do it after a chunk of documents
                    if (docs.Count == BULK_SIZE)
                    {
                        indexWriter.AddDocuments(docs);
                        //Flush and make it ready for search
                        indexWriter.Commit();

                        docs.Clear();//Remove the added documents...
                    }
                }
                //--- Write the last chunk of the documents
                if (docs.Count > 0)
                {
                    indexWriter.AddDocuments(docs);
                    //Flush and make it ready for search
                    indexWriter.Commit();
                }
            }

            return count;
        }

        public void DeleteAll()
        {
            if (indexWriter == null)
                indexWriter = new IndexWriter(indexDirectory, indexWriterConfig);

            indexWriter.DeleteAll();
            indexWriter.Commit();//Flush and make it ready for search
        }

        public void Update(string productId)
        {
            var prod = dataProvider.GetProduct(productId);

            indexWriter.UpdateDocument(new Term("discountedPrice"), prod.GetLuceneDocument());
            indexWriter.Commit();//Flush and make it ready for search
        }
    }
}
