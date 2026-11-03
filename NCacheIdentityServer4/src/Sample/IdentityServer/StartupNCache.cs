// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in 
// the project root for license information.


using System.Linq;
using IdentityServer4;
using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Stores.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public class StartupNCache
    {
        public StartupNCache(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer()
                .AddTestUsers(TestUsers.Users)
                .AddNCacheConfigurationStore(options =>
                {
                    SetNCacheConnection(options);
                })
                .AddNCachePersistedGrantStore(options =>
                {
                    SetNCacheConnection(options);
                })
                .AddNCacheDeviceCodeStore(options =>
                {
                    SetNCacheConnection(options);
                });


            builder.AddDeveloperSigningCredential();

            // IdentityServer4 3.1.x configures its authentication cookies with SameSite=None.
            // Modern browsers reject SameSite=None cookies that are not Secure, and this
            // sample runs over plain HTTP on localhost, so use Lax instead.
            services.PostConfigure<CookieAuthenticationOptions>(
                IdentityServerConstants.DefaultCookieAuthenticationScheme,
                options => options.Cookie.SameSite = SameSiteMode.Lax);
            services.PostConfigure<CookieAuthenticationOptions>(
                IdentityServerConstants.ExternalCookieAuthenticationScheme,
                options => options.Cookie.SameSite = SameSiteMode.Lax);

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = 
                        IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                })
                .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
                {
                    options.SignInScheme = 
                        IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.SaveTokens = true;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "native.code";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                InitializeNCacheStore(app);
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        // NCache thin client connection: cache name, server and port come from
        // appsettings.json ("CacheId", "CacheServer", "CachePort"); no client.ncconf is used.
        private void SetNCacheConnection(OptionsBase options)
        {
            options.CacheId = _configuration["CacheId"];
            options.CacheServer = _configuration["CacheServer"];
            options.CachePort = _configuration.GetValue<int?>("CachePort") ?? 9800;
        }

        private void InitializeNCacheStore(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                                            .GetService<IServiceScopeFactory>()
                                                .CreateScope())
            {
                var clientContext = serviceScope.ServiceProvider
                    .GetRequiredService<IConfigurationStoreRepository<Client>>();

                var identityContext = serviceScope.ServiceProvider
                    .GetRequiredService<IConfigurationStoreRepository<
                                                        IdentityResource>>();
                var apiContext = serviceScope.ServiceProvider
                    .GetRequiredService<IConfigurationStoreRepository<
                                                        ApiResource>>();

                var clientCount = clientContext
                                    .GetMultipleByTagsAsync(
                                        new string[] { "Client" })
                                    .GetAwaiter()
                                    .GetResult()
                                    .ToList()
                                    .Count;

                var identityCount = identityContext
                                    .GetMultipleByTagsAsync(
                                        new string[] { "IdentityResource" })
                                    .GetAwaiter()
                                    .GetResult()
                                    .ToList()
                                    .Count;

                var apiCount = apiContext
                                    .GetMultipleByTagsAsync(
                                        new string[] { "ApiResource" })
                                    .GetAwaiter()
                                    .GetResult()
                                    .ToList()
                                    .Count;

                if (clientCount == 0)
                {
                    var clients = Config.Clients.ToList().Select(x => x.ToEntity());
                    clientContext.AddAsync(clients).Wait();
                }

                if (identityCount == 0)
                {
                    var resources = Config.Ids.ToList().Select(x => x.ToEntity());
                    identityContext.AddAsync(resources).Wait();
                }

                if (apiCount == 0)
                {
                    var resources = Config.Apis.ToList().Select(x => x.ToEntity());
                    apiContext.AddAsync(resources).Wait();
                }
            }
        }
    }
}