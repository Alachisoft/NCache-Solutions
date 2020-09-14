using System;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;

namespace IdentityServer4.NCache.Options
{
    public class ProfileServiceCachingOptions<T>
        where T : class, IProfileService
    {
        public Func<IsActiveContext, string> KeySelector { get; set; } = 
            (context) => context.Subject.Claims.First(_ => _.Type == "sub").Value;

        public Func<IsActiveContext, bool> ShouldCache { get; set; } = 
            (context) => true;

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(10);

        private string _keyPrefix = string.Empty;

        public string KeyPrefix
        {
            get
            {
                return string.IsNullOrEmpty(this._keyPrefix) ? this._keyPrefix : $"{_keyPrefix}:";
            }
            set
            {
                this._keyPrefix = value;
            }
        }
    }
}
