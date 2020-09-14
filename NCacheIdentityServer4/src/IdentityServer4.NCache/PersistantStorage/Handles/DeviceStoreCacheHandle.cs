using Alachisoft.NCache.Client;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Utils;

namespace IdentityServer4.NCache.Stores.Handles
{
    public class DeviceStoreCacheHandle
    {
        internal readonly DeviceStoreOptions options;
        internal readonly ICache cache;

        public DeviceStoreCacheHandle(
            DeviceStoreOptions options)
        {
            this.options = options;
            cache = Utilities.CreateCache(options);
        }
    }
}
