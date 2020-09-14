using System;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public abstract class UserClaim
    {
        public string Type { get; set; }
    }
}