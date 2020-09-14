using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using IdentityServer4.NCache.Stores.Handles;
using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores.Repositories
{
    public class ConfigurationStoreRepository<T> : IConfigurationStoreRepository<T>
        where T:IConfigurationStoreEntity
    {
        private readonly ConfigurationStoreCacheHandle _handle;
        private static readonly string _keyPrefix = $"{typeof(T).FullName}-";
        private static readonly string _tagPrefix = $"{typeof(T).FullName}-Tag-";

        public ConfigurationStoreRepository(ConfigurationStoreCacheHandle handle)
        {
            _handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        public async Task AddAsync(T item)
        {
            var item1 = CreateCacheItem(item);
            var key = GetKey(item);
            await _handle.cache.InsertAsync(key, item1);
        }

        public async Task AddAsync(IEnumerable<T> items) 
        {
            Dictionary<string, CacheItem> dictionary =
                new Dictionary<string, CacheItem>();

            string key;
            CacheItem item1;
            items.ToList().ForEach(x =>
            {
                key = GetKey(x);
                item1 = CreateCacheItem(x);
                dictionary.Add(key, item1);
            });

            await Task.Run(() => _handle.cache.InsertBulk(dictionary));
        }

        public async Task<IEnumerable<T>> GetMultipleAsync(IEnumerable<string> keys) 
        {
            keys = keys.ToList().Select(x => GenerateKey(x));
            var dictionary = await Task.Run(() => _handle.cache.GetBulk<T>(keys));

            return new List<T>(dictionary.Values).Where(x => x != null);
        }

        public Task<IEnumerable<T>> GetMultipleByTagsAsync(
            IEnumerable<string> tags) 
        {
            var items = _handle.cache.SearchService.GetByTags<T>(
                GenerateTags(tags),
                TagSearchOptions.ByAnyTag);

            if (items == null || items.Count == 0)
            {
                return Task.FromResult(new List<T>().AsEnumerable());
            }

            return Task.FromResult(items.Values.AsEnumerable());
        }

        public async Task<T> GetSingleAsync(string key)
        {
            return await Task.Run(() => _handle.cache.Get<T>(GenerateKey(key)));
        }

        private static string GenerateKey(string key)
        {
            return $"{_keyPrefix}-{key.Trim()}";
        }

        private static Tag[] GenerateTags(IEnumerable<string> tags)
        {
            return tags.Select(x => new Tag($"{_tagPrefix}-{x.Trim()}")).ToArray();
        }

        private static string GetKey(T item)
        {
            return GenerateKey(item.GetKey());
        }

        private static Tag[] GetTags(T item)
        {
            return GenerateTags(item.GetTags());
        }

        private static CacheItem CreateCacheItem(T entity)
        {
            CacheItem item = new CacheItem(entity);
            item.Tags = GetTags(entity);
            return item;
        }
    }
}
