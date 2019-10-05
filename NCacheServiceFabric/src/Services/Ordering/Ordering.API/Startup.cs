namespace Microsoft.eShopOnContainers.Services.Ordering.API
{
    using AspNetCore.Http;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using global::Ordering.API.Application.IntegrationEvents;
    using global::Ordering.API.Application.IntegrationEvents.Events;
    using global::Ordering.API.Infrastructure.Filters;
    using global::Ordering.API.Infrastructure.Middlewares;
    using Infrastructure.AutofacModules;
    using Infrastructure.Filters;
    using Infrastructure.Services;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.ServiceFabric;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
    using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
    using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
    using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Ordering.Infrastructure;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.IdentityModel.Tokens.Jwt;
    using System.Reflection;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Alachisoft.NCache.Client;
    using Microsoft.Extensions.Options;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsights(Configuration)
                .AddCustomMvc()
                .AddHealthChecks(Configuration)
                .AddCustomDbContext(Configuration)
                .AddCustomSwagger(Configuration)
                .AddCustomIntegrations(Configuration)
                .AddCustomConfiguration(Configuration)
                .AddEventBus(Configuration)
                .AddCustomAuthentication(Configuration);

            

            //configure autofac

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());

            container.RegisterModule(new ApplicationModule(Configuration));

            return new AutofacServiceProvider(container.Build());
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddAzureWebAppDiagnostics();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseCors("CorsPolicy");

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            ConfigureAuth(app);

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Ordering.API V1");
                   c.OAuthClientId("orderingswaggerui");
                   c.OAuthAppName("Ordering Swagger UI");
               });

            ConfigureEventBus(app);
        }


        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<BuildingBlocks.EventBus.Abstractions.IEventBus>();

            eventBus.Subscribe<UserCheckoutAcceptedIntegrationEvent, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>>();
            eventBus.Subscribe<GracePeriodConfirmedIntegrationEvent, IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>>();
            eventBus.Subscribe<OrderStockConfirmedIntegrationEvent, IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>>();
            eventBus.Subscribe<OrderStockRejectedIntegrationEvent, IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
            eventBus.Subscribe<OrderPaymentSuccededIntegrationEvent, IIntegrationEventHandler<OrderPaymentSuccededIntegrationEvent>>();
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            // Enable SF telemetry initializer
            services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                new FabricTelemetryInitializer());

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();  //Injecting Controllers themselves thru DI
                                              //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

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

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddSqlServer(
                    configuration["ConnectionString"],
                    name: "OrderingDB-check",
                    tags: new string[] { "orderingdb" });


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
                name: "ordering-ncacheeventbus-check",
                failureStatus: HealthStatus.Unhealthy,
                tags: new string[] { "ncacheeventbus" },
                cacheConnectionOptions: new CacheConnectionOptions
                {
                    ServerList = servers
                });


            if (configuration.GetValue<bool>("CachingEnabled"))
            {
                cacheID = "OrderingCache";
                if (!string.IsNullOrEmpty(configuration["CacheID"]))
                {
                    cacheID = configuration["CacheID"];
                }

                ipAddresses = configuration.GetValue<string>("OrderingCacheIPAddresses", "").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for Ordering cache");
                }

                servers = new List<ServerInfo>();

                foreach (var ipAddress in ipAddresses)
                {
                    servers.Add(new ServerInfo(ipAddress.Trim()));
                }

                hcBuilder
                 .AddNCacheHealthCheck(
                    cacheID: cacheID,
                    name: "ordering-cache-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncache" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = servers
                    });
            }


            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEntityFrameworkSqlServer()
                   .AddDbContext<OrderingContext>(options =>
                   {
                       options.UseSqlServer(configuration["ConnectionString"],
                           sqlServerOptionsAction: sqlOptions =>
                           {
                               sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                               sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                           });
                   },
                       ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                   );

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

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Ordering HTTP API",
                    Version = "v1",
                    Description = "The Ordering Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize",
                    TokenUrl = $"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "orders", "Ordering API" }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();


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

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<OrderingSettings>(configuration);
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

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];


            services.AddSingleton<IEventBus, EventBusNCache>(sp =>
            {
                var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                string ncacheSubscriptionName = "OrderingSubscription";

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

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "orders";
            });

            return services;
        }
    }
}
