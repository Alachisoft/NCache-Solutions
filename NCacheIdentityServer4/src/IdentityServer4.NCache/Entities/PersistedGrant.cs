using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public class PersistedGrant:IOperationalStoreEntity
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }

        public DateTime? GetExpiration()
        {
            return Expiration;
        }

        public string GetKey()
        {
            return Key;
        }

        public IEnumerable<string> GetTags()
        {
            var subjectId = string.IsNullOrWhiteSpace(SubjectId) ?
                                " " : SubjectId.Trim();
            var clientId = string.IsNullOrWhiteSpace(ClientId) ?
                                " " : ClientId.Trim();
            var type = string.IsNullOrWhiteSpace(Type) ?
                                " " : Type.Trim();
            return new List<string>
            {
                $"{subjectId}",
                $"{clientId}",
                $"{type}",
                "PersistedGrant"
            }.AsEnumerable();
        }
    }
}
