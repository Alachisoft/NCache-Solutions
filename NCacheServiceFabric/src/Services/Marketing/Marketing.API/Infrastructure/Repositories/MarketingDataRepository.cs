using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.eShopOnContainers.Services.Marketing.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure.Repositories
{
    public class MarketingDataRepository
        : IMarketingDataRepository
    {
        private readonly MarketingReadDataContext _context;
        private ICache _cache;
        private readonly bool _cachingEnabled;
        private readonly double _expirationTime;

        public MarketingDataRepository(IOptions<MarketingSettings> settings)
        {
            _context = new MarketingReadDataContext(settings);
            _cachingEnabled = settings.Value.CachingEnabled;
            _expirationTime = settings.Value.MarketingCacheExpirationTimeInMinutes;
            CreateCache(settings);
        }

        private void CreateCache(IOptions<MarketingSettings> settings)
        {
            if (settings.Value.CachingEnabled)
            {
                var cacheID = settings.Value.MarketingCacheID;
                if (cacheID == null)
                {
                    throw new ArgumentNullException("Cache ID not given for marketing cache");
                }

                var ipAddresseString = settings.Value.MarketingCacheIPAddresses;
                if (ipAddresseString == null)
                {
                    throw new ArgumentNullException("IP addresses not given for marketing cache");
                }

                var ipAddresses = ipAddresseString.Split('-');
                List<ServerInfo> servers = new List<ServerInfo>();

                foreach (var ipAddress in ipAddresses)
                {
                    servers.Add(new ServerInfo(ipAddress.Trim()));
                }

                _cache = CacheManager.GetCache(cacheID, new CacheConnectionOptions
                {
                    ServerList = servers,
                    ConnectionRetries = 5,
                    RetryConnectionDelay = TimeSpan.FromSeconds(2),
                    EnableKeepAlive = true,
                    KeepAliveInterval = TimeSpan.FromMinutes(1)
                });

            }
        }

        public async Task<MarketingData> GetAsync(string userId)
        {
            if (!_cachingEnabled)
            {
                var filter = Builders<MarketingData>.Filter.Eq("UserId", userId);
                return await _context.MarketingData
                                     .Find(filter)
                                     .FirstOrDefaultAsync(); 
            }
            else
            {
                var key = $"MarketingData:UserID:{userId}";
                var item = await Task.Run(() => _cache.GetCacheItem(key));

                if (item == null)
                {
                    var filter = Builders<MarketingData>.Filter.Eq("UserId", userId);
                    var marketingData =  await _context.MarketingData
                                         .Find(filter)
                                         .FirstOrDefaultAsync();

                    if (marketingData != null)
                    {
                        item = new CacheItem(JsonConvert.SerializeObject(marketingData))
                        {
                            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                        };

                        await _cache.InsertAsync(key, item);
                    }

                    return marketingData;
                }
                else
                {
                    var marketingDataString = item.GetValue<string>();
                    return JsonConvert.DeserializeObject<MarketingData>(marketingDataString);
                }
            }
        }

        public async Task UpdateLocationAsync(MarketingData marketingData)
        {
            var filter = Builders<MarketingData>.Filter.Eq("UserId", marketingData.UserId);
            var update = Builders<MarketingData>.Update
                .Set("Locations", marketingData.Locations)
                .CurrentDate("UpdateDate");

            await _context.MarketingData
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            if (_cachingEnabled)
            {
                var item = new CacheItem(JsonConvert.SerializeObject(marketingData))
                {
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                };

                await _cache.InsertAsync($"MarketingData:UserID:{marketingData.UserId}", item);
            }
        }
    }
}
