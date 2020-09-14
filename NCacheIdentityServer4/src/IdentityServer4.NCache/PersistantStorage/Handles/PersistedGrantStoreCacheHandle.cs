using Alachisoft.NCache.Client;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Utils;

namespace IdentityServer4.NCache.Stores.Handles
{
    public class PersistedGrantStoreCacheHandle
    {
        internal readonly PersistedStoreOptions options;
        internal readonly ICache cache;

        public PersistedGrantStoreCacheHandle(
            PersistedStoreOptions options)
        {
            this.options = options;
            cache = Utilities.CreateCache(options);
        }
    }
}
