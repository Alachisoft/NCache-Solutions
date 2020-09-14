using System;
using System.Collections.Generic;

namespace IdentityServer4.NCache.Stores.Interfaces
{
    public interface IOperationalStoreEntity
    {
        string GetKey();
        IEnumerable<string> GetTags();
        DateTime? GetExpiration();
    }
}
