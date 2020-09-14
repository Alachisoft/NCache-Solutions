using IdentityServer4.NCache.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public class DeviceFlowCodes:IOperationalStoreEntity
    {
        public string DeviceCode { get; set; }
        public string UserCode { get; set; }
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
            return DeviceCode;
        }

        public IEnumerable<string> GetTags()
        {
            string userCode = string.IsNullOrWhiteSpace(UserCode) ?
                                    " " : UserCode.Trim();
            return new List<string> 
            { 
                $"{userCode}"
            };
        }
    }
}
