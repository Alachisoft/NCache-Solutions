namespace Microsoft.eShopOnContainers.Services.Marketing.API
{
    using AspNetCore.Builder;
    using AspNetCore.Hosting;
    using AspNetCore.Http;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Azure.ServiceBus;
    using BuildingBlocks.EventBus;
    using BuildingBlocks.EventBus.Abstractions;
    using BuildingBlocks.EventBusNCache;
    using BuildingBlocks.EventBusRabbitMQ;
    using BuildingBlocks.EventBusServiceBus;
    using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
    using EntityFrameworkCore;
    using Extensions.Configuration;
    using Extensions.DependencyInjection;
    using Extensions.Logging;
    using HealthChecks.UI.Client;
    using Infrastructure;
    using Infrastructure.Filters;
    using Infrastructure.Repositories;
    using Infrastructure.Services;
    using IntegrationEvents.Events;
    using Marketing.API.IntegrationEvents.Handlers;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.ServiceFabric;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure.Middlewares;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using RabbitMQ.Client;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Reflection;
    using Alachisoft.NCache.Client;
    using Alachisoft.NCache.EntityFrameworkCore;

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
            services
                .AddCustomHealthCheck(Configuration)
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();  //Injecting Controllers themselves thru DIFor further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            services.Configure<MarketingSettings>(Configuration);

            ConfigureAuthService(services);

            services.AddDbContext<MarketingContext>(options =>
            {
                if (Configuration.GetValue<bool>("CachingEnabled"))
                {
                    var cacheID = Configuration["MarketingCacheID"];

                    if (string.IsNullOrEmpty(cacheID))
                    {
                        cacheID = "MarketingCache";
                    }

                    var ipAddresses = Configuration.GetValue<string>("MarketingCacheIPAddresses", "").Split('-');

                    if (ipAddresses.Length == 0)
                    {
                        throw new ArgumentNullException("No IP addresses given for the Marketing EF Cache");
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
                            EnableClientLogs = Configuration.GetValue("EnableEFClientLogs", false),
                            LogLevel = Alachisoft.NCache.Client.LogLevel.Debug,
                            EnableKeepAlive = true,
                            KeepAliveInterval = TimeSpan.FromSeconds(60)
                        });
                }

                options.UseSqlServer(Configuration["ConnectionString"],
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

            if (Configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    var serviceBusConnectionString = Configuration["EventBusConnection"];
                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
            }
            else if (Configuration.GetValue<bool>("RabbitMQBusEnabled"))
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = Configuration["EventBusConnection"],
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                    {
                        factory.UserName = Configuration["EventBusUserName"];
                    }

                    if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                    {
                        factory.Password = Configuration["EventBusPassword"];
                    }

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                    }

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                });
            }
            else
            {
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
            }

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Marketing HTTP API",
                    Version = "v1",
                    Description = "The Marketing Service HTTP API",
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
                        { "marketing", "Marketing API" }
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

            RegisterEventBus(services);

            services.AddTransient<IMarketingDataRepository, MarketingDataRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddOptions();

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

            ConfigureAuth(app);

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Marketing.API V1");
                   c.OAuthClientId("marketingswaggerui");
                   c.OAuthAppName("Marketing Swagger UI");
               });

            ConfigureEventBus(app);
        }

        private void RegisterAppInsights(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            var orchestratorType = Configuration.GetValue<string>("OrchestratorType");

            if (orchestratorType?.ToUpper() == "K8S")
            {
                // Enable K8s telemetry initializer
                services.AddApplicationInsightsKubernetesEnricher();
            }
            if (orchestratorType?.ToUpper() == "SF")
            {
                // Enable SF telemetry initializer
                services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                    new FabricTelemetryInitializer());
            }
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("IdentityUrl");
                    options.Audience = "marketing";
                    options.RequireHttpsMetadata = false;
                });
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            if (Configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });
            }
            else if (Configuration.GetValue<bool>("RabbitMQBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                });
            }
            else
            {
                services.AddSingleton<IEventBus, EventBusNCache>(sp =>
                {
                    var ncachePersistentConnection = sp.GetRequiredService<INCachePersistantConnection>();
                    var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusNCache>>();
                    var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    string ncacheSubscriptionName = "MarketingSubscription";

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
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<UserLocationUpdatedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UserLocationUpdatedIntegrationEvent, UserLocationUpdatedIntegrationEventHandler>();
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

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddSqlServer(
                    configuration["ConnectionString"],
                    name: "MarketingDB-check",
                    tags: new string[] { "marketingdb" })
                .AddMongoDb(
                    configuration["MongoConnectionString"],
                    name: "MarketingDB-mongodb-check",
                    tags: new string[] { "mongodb" });

            var accountName = configuration.GetValue<string>("AzureStorageAccountName");
            var accountKey = configuration.GetValue<string>("AzureStorageAccountKey");
            if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(accountKey))
            {
                hcBuilder
                    .AddAzureBlobStorage(
                        $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey};EndpointSuffix=core.windows.net",
                        name: "marketing-storage-check",
                        tags: new string[] { "marketingstorage" });
            }

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                hcBuilder
                    .AddAzureServiceBusTopic(
                        configuration["EventBusConnection"],
                        topicName: "eshop_event_bus",
                        name: "marketing-servicebus-check",
                        tags: new string[] { "servicebus" });
            }
            else if (configuration.GetValue<bool>("RabbitMQBusEnabled"))
            {
                hcBuilder
                    .AddRabbitMQ(
                        $"amqp://{configuration["EventBusConnection"]}",
                        name: "marketing-rabbitmqbus-check",
                        tags: new string[] { "rabbitmqbus" });
            }
            else
            {
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
                    name: "marketing-ncacheeventbus-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncacheeventbus" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = servers
                    });
            }

            if (configuration.GetValue<bool>("CachingEnabled"))
            {
                var cacheID = configuration["MarketingCacheID"];

                if (string.IsNullOrEmpty(cacheID))
                {
                    cacheID = "MarketingCache";
                }

                var ipAddresses = configuration.GetValue<string>("MarketingCacheIPAddresses", "").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for the Marketing EF Cache");
                }

                List<ServerInfo> servers = new List<ServerInfo>();

                foreach (var ipAddress in ipAddresses)
                {
                    servers.Add(new ServerInfo(ipAddress.Trim()));
                }

                hcBuilder
                 .AddNCacheHealthCheck(
                    cacheID: cacheID,
                    name: "marketing-ncach-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncache" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = servers
                    });
            }

            return services;
        }
    }
}
