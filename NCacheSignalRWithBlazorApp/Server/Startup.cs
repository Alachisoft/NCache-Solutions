using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorSignalRApp.Server.Hubs;
using Alachisoft.NCache.AspNetCore.SignalR;

namespace BlazorSignalRApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region snippet_ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            var ncacheSection = Configuration.GetSection("NCacheConfiguration");

            string cacheName = ncacheSection["CacheName"];
            if (string.IsNullOrWhiteSpace(cacheName))
            {
                throw new InvalidOperationException(
                    "'NCacheConfiguration:CacheName' is missing in appsettings.json.");
            }

            string applicationId = ncacheSection["ApplicationID"];
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new InvalidOperationException(
                    "'NCacheConfiguration:ApplicationID' is missing in appsettings.json.");
            }

            // Server and port are read by NCache from the "ncache" section of appsettings.json
            services.AddSignalR().AddNCache(ncacheOptions =>
            {
                ncacheOptions.CacheName = cacheName;
                ncacheOptions.ApplicationID = applicationId;
                ncacheOptions.EventKey = applicationId;
            });
            services.AddControllersWithViews();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }
        #endregion

        #region snippet_Configure
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
        #endregion
    }
}
