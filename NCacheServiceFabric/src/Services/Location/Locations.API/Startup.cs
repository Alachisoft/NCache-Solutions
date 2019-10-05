using Alachisoft.NCache.Client;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
using Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure.Filters;
using Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure.Middlewares;
using Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure.Repositories;
using Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.eShopOnContainers.Services.Locations.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterAppInsights(services);

            services
                .AddCustomHealthCheck(Configuration)
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            ConfigureAuthService(services);

            services.Configure<LocationSettings>(Configuration);


            services.AddSingleton<INCachePersistantConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultNCachePersistentConnection>>();

                var cacheID = "PubSubCache";
                if (!string.IsNullOrEmpty(Configuration["EventBusNCachID"]))
                {
                    cacheID = Configuration["EventBusNCachID"];
                }

                var topicID = "IntegrationEvents";
                if (!string.IsNullOrEmpty(Configuration["EventBusNCachTopicID"]))
                {
                    topicID = Configuration["EventBusNCachTopicID"];
                }

                var ipAddresses = Configuration.GetValue<string>("PubSubCacheIPAddresses").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for Pub Sub Cache");
                }

                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    ipAddresses[i] = ipAddresses[i].Trim();
                }

                var enableNCacheClientLogs = Configuration.GetValue<bool>("EnableNCacheClientLogs", false);
                return new DefaultNCachePersistentConnection(cacheID, topicID, logger, ipAddresses, enableNCacheClientLogs);
            });


            RegisterEventBus(services);

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "eShopOnContainers - Location HTTP API",
                    Version = "v1",
                    Description = "The Location Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service"
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize",
                    TokenUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "locations", "Locations API" }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ILocationsService, LocationsService>();
            services.AddTransient<ILocationsRepository, LocationsRepository>();

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddAzureWebAppDiagnostics();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseCors("CorsPolicy");

            ConfigureAuth(app);

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Locations.API V1");
                  c.OAuthClientId("locationsswaggerui");
                  c.OAuthAppName("Locations Swagger UI");
              });

            LocationsContextSeed.SeedAsync(app, loggerFactory)
                .Wait();
        }

        private void RegisterAppInsights(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            // Enable SF telemetry initializer
            services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                new FabricTelemetryInitializer());

        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityUrl");
                options.Audience = "locations";
                options.RequireHttpsMetadata = false;
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
        }

        private void RegisterEventBus(IServiceCollection services)
        {

            services.AddSingleton<IEventBus, EventBusNCache>(sp =>
            {
                var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                string ncacheSubscriptionName = "LocationSubscription";

                if (!string.IsNullOrEmpty(Configuration["EventBusNCacheSubscriptionName"]))
                {
                    ncacheSubscriptionName = Configuration["EventBusNCacheSubscriptionName"];
                }

                return new EventBusNCache(
                    ncachePersistentConnection,
                    eventBusSubscriptionManager,
                    logger,
                    iLifeTimeScope,
                    ncacheSubscriptionName);
            });


            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddMongoDb(
                    configuration["ConnectionString"],
                    name: "locations-mongodb-check",
                    tags: new string[] { "mongodb" });

            if (configuration.GetValue<bool>("LocationCachingEnabled"))
            {
                var ipAddressesLocation = configuration.GetValue<string>("LocationCacheIPAddresses", "").Split('-');

                if (ipAddressesLocation.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for Locations cache");
                }

                List<ServerInfo> serversLocation = new List<ServerInfo>();

                foreach (var ipAddress in ipAddressesLocation)
                {
                    serversLocation.Add(new ServerInfo(ipAddress.Trim()));
                }

                hcBuilder
                    .AddNCacheHealthCheck(
                    configuration.GetValue<string>("CacheID", "LocationsCache"),
                    name: "locations-repo-ncache",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncache" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = serversLocation
                    });
            }


            var cacheID = "PubSubCache";
            if (!string.IsNullOrEmpty(configuration["EventBusNCachID"]))
            {
                cacheID = configuration["EventBusNCachID"];
            }

            var ipAddresses = configuration.GetValue<string>("PubSubCacheIPAddresses", "").Split('-');

            if (ipAddresses.Length == 0)
            {
                throw new ArgumentNullException("No IP addresses given for Pub Sub cache");
            }

            List<ServerInfo> servers = new List<ServerInfo>();

            foreach (var ipAddress in ipAddresses)
            {
                servers.Add(new ServerInfo(ipAddress.Trim()));
            }

            hcBuilder
             .AddNCacheHealthCheck(
                cacheID: cacheID,
                name: "location-ncacheeventbus-check",
                failureStatus: HealthStatus.Unhealthy,
                tags: new string[] { "ncacheeventbus" },
                cacheConnectionOptions: new CacheConnectionOptions
                {
                    ServerList = servers
                });


            return services;
        }
    }
}
