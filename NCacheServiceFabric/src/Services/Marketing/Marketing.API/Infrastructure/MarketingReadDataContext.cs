namespace Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure
{
    using Microsoft.eShopOnContainers.Services.Marketing.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System;

    public class MarketingReadDataContext
    {
        private readonly IMongoDatabase _database = null;

        public MarketingReadDataContext(IOptions<MarketingSettings> settings)
        {
            if (!settings.Value.CosmosDBDeployment)
            {
                var client = new MongoClient(settings.Value.MongoConnectionString);
                if (client != null)
                {
                    _database = client.GetDatabase(settings.Value.MongoDatabase);
                }
            }
            else
            {
                MongoClientSettings mongoSettings = MongoClientSettings
                                                        .FromUrl(new MongoUrl(settings.Value.MongoConnectionString));
                mongoSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };

                var client = new MongoClient(mongoSettings);

                if (client != null)
                {
                    _database = client.GetDatabase(settings.Value.MongoDatabase);
                }
            }

            
        }

        public IMongoCollection<MarketingData> MarketingData
        {
            get
            {
                return _database.GetCollection<MarketingData>("MarketingReadDataModel");
            }
        }        
    }
}
