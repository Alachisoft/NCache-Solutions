using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.Extensions.Configuration;
using Ordering.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly OrderingContext _context;
        private readonly ICache _cache;
        private readonly IConfiguration _configuration;

        public RequestManager(OrderingContext context, ICache cache, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = cache;
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            ClientRequest request = null;

            if (!_configuration.GetValue<bool>("CachingEnabled"))
            {
                request = await _context.
                        FindAsync<ClientRequest>(id);
            }
            else
            {
                var key = $"ClientRequest:Guid:{id.ToString()}";
                request = await Task.Run(() => _cache.Get<ClientRequest>(key));

                if (request == null)
                {
                    request = await _context.
                        FindAsync<ClientRequest>(id);

                    if (request != null)
                    {
                        await _cache.InsertAsync(key, new CacheItem(request)
                        {
                            Expiration = new Expiration(ExpirationType.Sliding, TimeSpan.FromSeconds(15))
                        }); 
                    }
                }
            }

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new OrderingDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();

            await _cache.InsertAsync($"ClientRequest:Guid:{id.ToString()}", new CacheItem(request)
            {
                Expiration = new Expiration(ExpirationType.Sliding, TimeSpan.FromSeconds(15))
            });
        }
    }
}
