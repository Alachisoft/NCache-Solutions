namespace Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure
{
    using Microsoft.eShopOnContainers.Services.Locations.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System;

    public class LocationsContext
    {
        private readonly IMongoDatabase _database = null;

        public LocationsContext(IOptions<LocationSettings> settings)
        {
            if (!settings.Value.CosmosDBDeployment)
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                if (client != null)
                    _database = client.GetDatabase(settings.Value.Database); 
            }
            else
            {
                MongoClientSettings mongoSettings = MongoClientSettings
                                                        .FromUrl(new MongoUrl(settings.Value.ConnectionString));
                mongoSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
                

                var client = new MongoClient(mongoSettings);

                if (client != null)
                {
                    _database = client.GetDatabase(settings.Value.Database);
                }
            }
        }

        public IMongoCollection<UserLocation> UserLocation
        {
            get
            {
                return _database.GetCollection<UserLocation>("UserLocation");
            }
        }

        public IMongoCollection<Locations> Locations
        {
            get
            {
                return _database.GetCollection<Locations>("Locations");
            }
        }       
    }
}
