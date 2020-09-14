using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Stores.Handles;
using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores.Repositories
{
    public class PersistedGrantStoreRepository : IPersistedGrantStoreRepository
    {
        private readonly PersistedGrantStoreCacheHandle _handle;
        private static readonly string _keyPrefix = 
            $"{typeof(PersistedGrant).FullName}-";
        private static readonly string _tagPrefix = 
            $"{typeof(PersistedGrant).FullName}-Tag-";

        public PersistedGrantStoreRepository(PersistedGrantStoreCacheHandle handle)
        {
            _handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        public async Task AddAsync(PersistedGrant item)
        {
            var item1 = CreateCacheItem(item);
            var key = GetKey(item);
            await _handle.cache.InsertAsync(key, item1);
        }

        public async Task AddAsync(IEnumerable<PersistedGrant> items)
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

        public async Task DeleteAsync(string key)
        {
            await _handle.cache.RemoveAsync<PersistedGrant>(GenerateKey(key));
        }

        public async Task DeleteAsync(IEnumerable<string> keys)
        {
            keys = keys.ToList().Select(x => GenerateKey(x));

            await Task.Run(() => 
                _handle.cache.RemoveBulk<PersistedGrant>(
                    keys, 
                    out IDictionary<string, PersistedGrant> items));
        }

        public Task DeleteByTagsAsync(IEnumerable<string> tags)
        {
            _handle.cache.SearchService.RemoveByTags(
                GenerateTags(tags),
                TagSearchOptions.ByAllTags);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<PersistedGrant>> GetMultipleAsync(
            IEnumerable<string> keys)
        {
            keys = keys.ToList().Select(x => GenerateKey(x));
            var dictionary = 
                await Task.Run(() => _handle.cache.GetBulk<PersistedGrant>(keys));

            return new List<PersistedGrant>(dictionary.Values)
                            .Where(x => x != null);
        }

        public Task<IEnumerable<PersistedGrant>> GetMultipleByTagsAsync(
            IEnumerable<string> tags)
        {
            var items = _handle.cache.SearchService.GetByTags<PersistedGrant>(
                GenerateTags(tags),
                TagSearchOptions.ByAllTags);

            if (items == null || items.Count == 0)
            {
                return Task.FromResult(new List<PersistedGrant>().AsEnumerable());
            }

            return Task.FromResult(items.Values.AsEnumerable());
        }

        public async Task<PersistedGrant> GetSingleAsync(string key)
        {
            var item = await Task.Run(
                () => _handle.cache.Get<PersistedGrant>(GenerateKey(key)));
            return item;
        }

        private static string GenerateKey(string key)
        {
            return $"{_keyPrefix}-{key.Trim()}";
        }

        private static Tag[] GenerateTags(IEnumerable<string> tags)
        {
            return tags.Select(x => new Tag($"{_tagPrefix}-{x.Trim()}")).ToArray();
        }

        private static string GetKey(PersistedGrant item)
        {
            return GenerateKey(item.GetKey());
        }

        private static Tag[] GetTags(PersistedGrant item)
        {
            return GenerateTags(item.GetTags());
        }

        private static CacheItem CreateCacheItem(
            PersistedGrant entity)
        {
            CacheItem item = new CacheItem(entity);
            item.Tags = GetTags(entity);

            if (entity.GetExpiration().HasValue)
            {
                TimeSpan span = entity.GetExpiration().Value - DateTime.UtcNow;
                item.Expiration = new Expiration(ExpirationType.Absolute, span);
            }

            return item;
        }
    }
}
