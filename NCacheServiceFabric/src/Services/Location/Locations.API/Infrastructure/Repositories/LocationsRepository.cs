namespace Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure.Repositories
{
    using Alachisoft.NCache.Client;
    using Alachisoft.NCache.Runtime.Caching;
    using Microsoft.eShopOnContainers.Services.Locations.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.GeoJsonObjectModel;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModel;

    public class LocationsRepository
        : ILocationsRepository
    {
        private readonly LocationsContext _context;
        private ICache _cache;
        private readonly bool _cachingEnabled;
        private readonly double _expirationTime;

        public LocationsRepository(IOptions<LocationSettings> settings)
        {
            _context = new LocationsContext(settings);
            _cachingEnabled = settings.Value.LocationCachingEnabled;
            _expirationTime = settings.Value.LocationCacheExpirationTimeInMinutes;
            CreateCache(settings);
        }

        private void CreateCache(IOptions<LocationSettings> settings)
        {
            if (settings.Value.LocationCachingEnabled)
            {
                var cacheID = settings.Value.CacheID;
                if (cacheID == null)
                {
                    throw new ArgumentNullException("Cache ID not given for locations cache");
                }

                var ipAddresseString = settings.Value.LocationCacheIPAddresses;
                if (ipAddresseString == null)
                {
                    throw new ArgumentNullException("IP addresses not given for locations cache");
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

        public async Task<Locations> GetAsync(int locationId)
        {
            if (!_cachingEnabled)
            {
                var filter = Builders<Locations>.Filter.Eq("LocationId", locationId);
                return await _context.Locations
                                     .Find(filter)
                                     .FirstOrDefaultAsync();
            }
            else
            {
                string key = $"Location:LocationID:{locationId}";

                var item = await Task.Run(() => _cache.GetCacheItem(key));

                if (item == null)
                {
                    var filter = Builders<Locations>.Filter.Eq("LocationId", locationId);
                    var location = await _context.Locations
                                         .Find(filter)
                                         .FirstOrDefaultAsync();

                    if (location != null)
                    {
                        await _cache.InsertAsync(key, new CacheItem(JsonConvert.SerializeObject(location))
                        {
                            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                        });
                    }

                    return location;
                }
                else
                {
                    var locationString = item.GetValue<string>();
                    return JsonConvert.DeserializeObject<Locations>(locationString);
                }
            }

        }

        public async Task<UserLocation> GetUserLocationAsync(string userId)
        {
            if (!_cachingEnabled)
            {
                var filter = Builders<UserLocation>.Filter.Eq("UserId", userId);
                return await _context.UserLocation
                                     .Find(filter)
                                     .FirstOrDefaultAsync(); 
            }
            else
            {
                string key = $"UserLocation:UserID:{userId}";
                var item = await Task.Run(() => _cache.GetCacheItem(key));

                if (item == null)
                {
                    var filter = Builders<UserLocation>.Filter.Eq("UserId", userId);
                    var userLocation =  await _context.UserLocation
                                         .Find(filter)
                                         .FirstOrDefaultAsync();

                    if (userLocation != null)
                    {
                        await _cache.InsertAsync(key, new CacheItem(JsonConvert.SerializeObject(userLocation))
                        {
                            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                        });
                    }

                    return userLocation;
                }
                else
                {
                    var userLocationString = item.GetValue<string>();
                    return JsonConvert.DeserializeObject<UserLocation>(userLocationString);
                }
            }
        }

        public async Task<List<Locations>> GetLocationListAsync()
        {
            if (!_cachingEnabled)
            {
                return await _context.Locations.Find(new BsonDocument()).ToListAsync(); 
            }
            else
            {
                var key = $"GetAllLocations";
                var item = await Task.Run(() => _cache.GetCacheItem(key));

                if (item == null)
                {
                    var locations = await _context.Locations.Find(new BsonDocument()).ToListAsync();

                    if (locations != null && locations.Count > 0)
                    {
                        var cacheList = new List<string>();

                        foreach (var location in locations)
                        {
                            cacheList.Add(JsonConvert.SerializeObject(location));
                        }

                        await _cache.InsertAsync(key, new CacheItem(cacheList)
                        {
                            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                        });
                    }

                    return locations;
                }
                else
                {
                    var cacheList = item.GetValue<List<string>>();

                    var locations = new List<Locations>();

                    foreach(var cacheListItem in cacheList)
                    {
                        locations.Add(JsonConvert.DeserializeObject<Locations>(cacheListItem));
                    }

                    return locations;
                }
            }
        }

        public async Task<List<Locations>> GetCurrentUserRegionsListAsync(LocationRequest currentPosition)
        {
            var point = GeoJson.Point(GeoJson.Geographic(currentPosition.Longitude, currentPosition.Latitude));
            var orderByDistanceQuery = new FilterDefinitionBuilder<Locations>().Near(x => x.Location, point);
            var withinAreaQuery = new FilterDefinitionBuilder<Locations>().GeoIntersects("Polygon", point);
            var filter = Builders<Locations>.Filter.And(orderByDistanceQuery, withinAreaQuery);
            return await _context.Locations.Find(filter).ToListAsync();
        }

        public async Task AddUserLocationAsync(UserLocation location)
        {
            await _context.UserLocation.InsertOneAsync(location);

            if (_cachingEnabled)
            {
                await _cache.InsertAsync($"UserLocation:UserID:{location.UserId}", new CacheItem(JsonConvert.SerializeObject(location))
                {
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                });
            }
        }

        public async Task UpdateUserLocationAsync(UserLocation userLocation)
        {
            await _context.UserLocation.ReplaceOneAsync(
                doc => doc.UserId == userLocation.UserId,
                userLocation,
                new UpdateOptions { IsUpsert = true });

            if (_cachingEnabled)
            {
                await _cache.InsertAsync($"UserLocation:UserID:{userLocation.UserId}", new CacheItem(JsonConvert.SerializeObject(userLocation))
                {
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(_expirationTime))
                });
            }
        }
    }
}
