using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public class IdentityResource:IConfigurationStoreEntity
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<IdentityClaim> UserClaims { get; set; }
        public List<IdentityResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }

        public string GetKey()
        {
            return Name;
        }

        public IEnumerable<string> GetTags()
        {
            List<string> tags = new List<string>();

            tags.Add("IdentityResource");

            return tags;
        }
    }

    [Serializable]
    public class IdentityResourceProperty:Property
    {
    }

    [Serializable]
    public class IdentityClaim:UserClaim
    {
    }
}
