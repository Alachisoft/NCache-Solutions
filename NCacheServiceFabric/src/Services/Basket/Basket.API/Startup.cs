using Alachisoft.NCache.Client;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Basket.API.Infrastructure.Filters;
using Basket.API.Infrastructure.Middlewares;
using Basket.API.IntegrationEvents.EventHandling;
using Basket.API.IntegrationEvents.Events;
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
using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
using Microsoft.eShopOnContainers.Services.Basket.API.Infrastructure.Repositories;
using Microsoft.eShopOnContainers.Services.Basket.API.IntegrationEvents.EventHandling;
using Microsoft.eShopOnContainers.Services.Basket.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Basket.API.Model;
using Microsoft.eShopOnContainers.Services.Basket.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.eShopOnContainers.Services.Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterAppInsights(services);

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));

            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            ConfigureAuthService(services);

            services.AddCustomHealthCheck(Configuration);

            services.Configure<BasketSettings>(Configuration);

            services.AddSingleton<ICache>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;



                var ipAddresses = Configuration.GetValue<string>("BasketCacheIPAddresses", "").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for basket repo cache");
                }

                List<ServerInfo> servers = new List<ServerInfo>();

                foreach (var ipAddress in ipAddresses)
                {
                    servers.Add(new ServerInfo(ipAddress.Trim()));
                }

                return CacheManager.GetCache(settings.CacheID, new CacheConnectionOptions
                {
                    ServerList = servers,
                    EnableClientLogs = Configuration.GetValue<bool>("EnableNCacheClientLogs", false),
                    LogLevel = Alachisoft.NCache.Client.LogLevel.Debug,
                    EnableKeepAlive = true,
                    KeepAliveInterval = TimeSpan.FromSeconds(60)
                });
            });

            services.AddTransient<IBasketRepository, NCacheBasketRepository>();




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

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Basket HTTP API",
                    Version = "v1",
                    Description = "The Basket Service HTTP API",
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
                        { "basket", "Basket API" }
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

            services.AddOptions();

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

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            ConfigureAuth(app);

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Basket.API V1");
                   c.OAuthClientId("basketswaggerui");
                   c.OAuthAppName("Basket Swagger UI");
               });

            ConfigureEventBus(app);

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

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "basket";
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
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusNCache>(sp =>
            {
                var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                string ncacheSubscriptionName = "BasketSubscription";

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

            services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
            services.AddTransient<OrderStartedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());


            var ipAddressesBasket = configuration.GetValue<string>("BasketCacheIPAddresses", "").Split('-');

            if (ipAddressesBasket.Length == 0)
            {
                throw new ArgumentNullException("No IP addresses given for basket repo cache");
            }

            List<ServerInfo> serversBasket = new List<ServerInfo>();

            foreach (var ipAddress in ipAddressesBasket)
            {
                serversBasket.Add(new ServerInfo(ipAddress.Trim()));
            }

            hcBuilder
                .AddNCacheHealthCheck
                (
                    cacheID: configuration["CacheID"],
                    name: "ncache-basket-repository-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncache" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = serversBasket
                    }
                );

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
                name: "basket-ncacheeventbus-check",
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
