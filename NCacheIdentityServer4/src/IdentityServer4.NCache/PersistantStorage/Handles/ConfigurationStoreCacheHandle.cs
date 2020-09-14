using Alachisoft.NCache.Client;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Utils;

namespace IdentityServer4.NCache.Stores.Handles
{
    public class ConfigurationStoreCacheHandle
    {
        internal readonly ConfigurationStoreOptions options;
        internal readonly ICache cache;

        public ConfigurationStoreCacheHandle(
            ConfigurationStoreOptions options)
        {
            this.options = options;
            cache = Utilities.CreateCache(options);
        }
    }
}
