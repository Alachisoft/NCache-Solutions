// Copyright notices for this sample are in LICENSE-IdentityServer4 and LICENSE-Alachisoft at the sample root.
using Microsoft.AspNetCore.Builder;

namespace JavaScriptClient
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
