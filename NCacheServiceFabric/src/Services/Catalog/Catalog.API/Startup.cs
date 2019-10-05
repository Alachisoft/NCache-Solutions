using Autofac;
using Autofac.Extensions.DependencyInjection;
using global::Catalog.API.Infrastructure.Filters;
using global::Catalog.API.IntegrationEvents;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Alachisoft.NCache.EntityFrameworkCore;
using Alachisoft.NCache.Client;
using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.Catalog.API
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
            services.AddAppInsight(Configuration)
                .AddCustomMVC(Configuration)
                .AddCustomDbContext(Configuration)
                .AddCustomOptions(Configuration)
                .AddIntegrationServices(Configuration)
                .AddEventBus(Configuration)
                .AddSwagger()
                .AddCustomHealthCheck(Configuration);

            var builder = new ContainerBuilder();

            builder.Populate(services);
            

            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Configure logs

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

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.API V1");
              });

            ConfigureEventBus(app);
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddAppInsight(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            var orchestratorType = configuration.GetValue<string>("OrchestratorType");


            // Enable SF telemetry initializer
            services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                new FabricTelemetryInitializer());


            return services;
        }

        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {

            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    configuration["ConnectionString"],
                    name: "CatalogDB-check",
                    tags: new string[] { "catalogdb" });



            if (configuration.GetValue<bool>("EFCoreCachingEnabled"))
            {
                var ipAddressesEFCore = configuration.GetValue<string>("CatalogEFCacheIPAddresses", "").Split('-');

                if (ipAddressesEFCore.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for the Catalog EF Cache");
                }

                List<ServerInfo> serversEFCore = new List<ServerInfo>();

                foreach (var ipAddress in ipAddressesEFCore)
                {
                    serversEFCore.Add(new ServerInfo(ipAddress.Trim()));
                }

                hcBuilder
                    .AddNCacheHealthCheck(
                        cacheID: configuration.GetValue("EFCacheID", "CatalogEFCache"),
                        name: "catalog-efcache",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "ncache" },
                        cacheConnectionOptions: new CacheConnectionOptions
                        {
                            ServerList = serversEFCore
                        }
                    );
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
                name: "catalog-ncacheeventbus-check",
                failureStatus: HealthStatus.Unhealthy,
                tags: new string[] { "ncacheeventbus" },
                cacheConnectionOptions: new CacheConnectionOptions
                {
                    ServerList = servers
                });


            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogContext>(options =>
            {
                if (configuration.GetValue<bool>("EFCoreCachingEnabled"))
                {
                    var cacheID = configuration["EFCacheID"];

                    if (string.IsNullOrEmpty(cacheID))
                    {
                        cacheID = "CatalogEFCache";
                    }

                    var ipAddresses = configuration.GetValue<string>("CatalogEFCacheIPAddresses", "").Split('-');

                    if (ipAddresses.Length == 0)
                    {
                        throw new ArgumentNullException("No IP addresses given for the Catalog EF Cache");
                    }

                    List<ServerInfo> servers = new List<ServerInfo>();

                    foreach (var ipAddress in ipAddresses)
                    {
                        servers.Add(new ServerInfo(ipAddress.Trim()));
                    }

                    NCacheConfiguration.Configure(
                        cacheID,
                        DependencyType.Other,
                        new CacheConnectionOptions
                        {
                            ServerList = servers,
                            CommandRetryInterval = TimeSpan.FromSeconds(configuration.GetValue("EFCacheCommandRetryInterval", 1)),
                            CommandRetries = configuration.GetValue("EFCommandRetries", 5),
                            EnableClientLogs = configuration.GetValue("EnableEFClientLogs", false),
                            LogLevel = Alachisoft.NCache.Client.LogLevel.Debug,
                            EnableKeepAlive = true,
                            KeepAliveInterval = TimeSpan.FromSeconds(60)
                        });

                    if (configuration.GetValue("EnableEFClientLogs", false))
                    {
                        NCacheConfiguration.ConfigureLogger();
                    }
                }

                var connectionString = configuration["ConnectionString"];
                options.UseSqlServer(configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });
            });

            return services;
        }

        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CatalogSettings>(configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "eShopOnContainers - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service"
                });
            });

            return services;

        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            services.AddSingleton<INCachePersistantConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultNCachePersistentConnection>>();

                var cacheID = "PubSubCache";
                if (!string.IsNullOrEmpty(configuration["EventBusNCachID"]))
                {
                    cacheID = configuration["EventBusNCachID"];
                }

                var topicID = "IntegrationEvents";
                if (!string.IsNullOrEmpty(configuration["EventBusNCachTopicID"]))
                {
                    topicID = configuration["EventBusNCachTopicID"];
                }

                var ipAddresses = configuration.GetValue<string>("PubSubCacheIPAddresses").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for Pub Sub Cache");
                }

                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    ipAddresses[i] = ipAddresses[i].Trim();
                }

                var enableNCacheClientLogs = configuration.GetValue<bool>("EnableNCacheClientLogs", false);
                return new DefaultNCachePersistentConnection(cacheID, topicID, logger, ipAddresses, enableNCacheClientLogs);
            });
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IEventBus, EventBusNCache>(sp =>
            {
                var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                string ncacheSubscriptionName = "CatalogSubscription";

                if (!string.IsNullOrEmpty(configuration["EventBusNCacheSubscriptionName"]))
                {
                    ncacheSubscriptionName = configuration["EventBusNCacheSubscriptionName"];
                }

                return new EventBusNCache(
                    ncachePersistentConnection,
                    eventBusSubscriptionManager,
                    logger,
                    iLifeTimeScope,
                    ncacheSubscriptionName);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

            return services;
        }
    }
}
