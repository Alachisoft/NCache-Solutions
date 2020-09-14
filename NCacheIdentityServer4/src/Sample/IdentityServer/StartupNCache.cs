// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in 
// the project root for license information.


using System.Linq;
using IdentityServer4;
using IdentityServer4.NCache.Entities;
using IdentityServer4.NCache.Mappers;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Stores.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                    options.CacheId = _configuration["CacheId"];

                    var serverList = _configuration["Servers"].Split(',')
                                            .Select(x => x.Trim())
                                            .ToList()
                                            .Select(y => 
                                                new NCacheServerInfo(y, 9800))
                                            .ToList();
                    options.ConnectionOptions = new NCacheConnectionOptions
                    {
                        ServerList = serverList,
                        EnableClientLogs = true,
                        LogLevel = NCacheLogLevel.Debug
                    };
                })
                .AddNCachePersistedGrantStore(options => 
                {
                    options.CacheId = _configuration["CacheId"];

                    var serverList = _configuration["Servers"].Split(',')
                                            .Select(x => x.Trim())
                                            .ToList()
                                            .Select(y => 
                                                new NCacheServerInfo(y, 9800))
                                            .ToList();
                    options.ConnectionOptions = new NCacheConnectionOptions
                    {
                        ServerList = serverList,
                        EnableClientLogs = true,
                        LogLevel = NCacheLogLevel.Debug
                    };
                })
                .AddNCacheDeviceCodeStore(options =>
                {
                    options.CacheId = _configuration["CacheId"];

                    var serverList = _configuration["Servers"].Split(',')
                                            .Select(x => x.Trim())
                                            .ToList()
                                            .Select(y =>
                                                new NCacheServerInfo(y, 9800))
                                            .ToList();
                    options.ConnectionOptions = new NCacheConnectionOptions
                    {
                        ServerList = serverList,
                        EnableClientLogs = true,
                        LogLevel = NCacheLogLevel.Debug
                    };
                });
                

            builder.AddDeveloperSigningCredential();

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