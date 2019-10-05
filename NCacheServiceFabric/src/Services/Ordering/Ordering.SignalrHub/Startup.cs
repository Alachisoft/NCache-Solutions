using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.SignalrHub.AutofacModules;
using Ordering.SignalrHub.IntegrationEvents;
using Ordering.SignalrHub.IntegrationEvents.EventHandling;
using Ordering.SignalrHub.IntegrationEvents.Events;
using System;
using System.IdentityModel.Tokens.Jwt;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
using System.Collections.Generic;
using Alachisoft.NCache.Client;

namespace Ordering.SignalrHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomHealthCheck(Configuration)
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
                });


            services.AddSignalR();



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


            ConfigureAuthService(services);

            RegisterEventBus(services);

            services.AddOptions();

            //configure autofac
            var container = new ContainerBuilder();
            container.RegisterModule(new ApplicationModule());
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddAzureWebAppDiagnostics();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
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

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsHub>("/notificationhub", options =>
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransports.All);
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToStockConfirmedIntegrationEvent, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToCancelledIntegrationEvent, OrderStatusChangedToCancelledIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToSubmittedIntegrationEvent, OrderStatusChangedToSubmittedIntegrationEventHandler>();
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "orders.signalrhub";
            });
        }

        private void RegisterEventBus(IServiceCollection services)
        {

            services.AddSingleton<IEventBus, EventBusNCache>(sp =>
            {
                var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                string ncacheSubscriptionName = "OrderingSignalRSubscription";

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
                name: "orderingsignalr-ncacheeventbus-check",
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
