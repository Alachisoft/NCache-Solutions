using System;

namespace IdentityServer4.NCache.Entities
{
    [Serializable]
    public abstract class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}