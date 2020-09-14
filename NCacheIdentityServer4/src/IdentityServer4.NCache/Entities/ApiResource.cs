using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public class ApiResource:IConfigurationStoreEntity
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<ApiSecret> Secrets { get; set; }
        public List<ApiScope> Scopes { get; set; }
        public List<ApiResourceClaim> UserClaims { get; set; }
        public List<ApiResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }


        public string GetKey()
        {
            return Name; 
        }

        public IEnumerable<string> GetTags()
        {
            List<string> tags = new List<string>();

            tags.Add("ApiResource");

            tags.AddRange(Scopes.Select(scope => $"{scope.Name.Trim()}"));

            return tags;
        }
    }

    [Serializable]
    public class ApiResourceProperty:Property
    {
    }

    [Serializable]
    public class ApiResourceClaim:UserClaim
    {
    }

    [Serializable]
    public class ApiScope
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiScopeClaim> UserClaims { get; set; }
    }

    [Serializable]
    public class ApiScopeClaim:UserClaim
    {
    }

    [Serializable]
    public class ApiSecret:Secret
    {
    }
}
