using System.Collections.Generic;

namespace IdentityServer4.NCache.Stores.Interfaces
{
    public interface IConfigurationStoreEntity
    {
        string GetKey();
        IEnumerable<string> GetTags();
    }
}
