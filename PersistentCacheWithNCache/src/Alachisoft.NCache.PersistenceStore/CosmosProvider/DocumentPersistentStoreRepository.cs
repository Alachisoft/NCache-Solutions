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

namespace Alachisoft.NCache.PersistenceStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public class DocumentPersistentStoreRepository<T> where T : class
    {
        #region private fields
        private string _endpoint = default(string);
        private string _key = default(string);
        private string _databaseId = default(string);
        private string _collectionId = default(string);
        private DocumentClient client = default(DocumentClient);
        #endregion

        #region constructur and initialization
        public DocumentPersistentStoreRepository()
        {
        }
        /// <summary>
        /// Initialize DocumentDb
        ///complete: https://localhost:8081~C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==~NCacheDB~CacheItem
        /// Endpoint = "https://localhost:8081";
        /// Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        /// DatabaseId = "NCacheDB";
        /// CollectionId = "CacheItem";
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="key"></param>
        /// <param name="databaseId"></param>
        /// <param name="CollectionId"></param>
        public void Initialize(string endPoints, string key, string databaseId, string collectionId)
        {
            try
            {
                _endpoint = endPoints;
                _key = key;
                _databaseId= databaseId;
                _collectionId = collectionId;

                client = new DocumentClient(new Uri(_endpoint), _key);
                CreateDatabaseIfNotExistsAsync().Wait();
                CreateCollectionIfNotExistsAsync().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 
        
        #region Basic Operations
        /// <summary>
        /// Get Item Async
        /// </summary>
        /// <param name="id">item id</param>
        /// <returns></returns>
        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var option = new RequestOptions
                {
                    PartitionKey = new PartitionKey(id)
                };

                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),option);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get Items Async for bulk
        /// </summary>
        /// <param name="predicate">function to compare and get item</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {           
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 ,EnableCrossPartitionQuery=true})
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns></returns>
        public async Task<Document> CreateItemAsync(T item)
        {
            var option=new RequestOptions
            {
                PartitionKey = new PartitionKey((item as StoreItem).Key)
            };

            return await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), 
                item,
                options: option,
                disableAutomaticIdGeneration:true
                );
           
        }
        /// <summary>
        /// Update Item
        /// </summary>
        /// <param name="id">item id to be updated</param>
        /// <param name="item">item to be updated</param>
        /// <returns></returns>
        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);
        }

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <param name="id">item id to be deleted</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        }
        #endregion

        #region private methods
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = _databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection { Id = _collectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion
    }
}