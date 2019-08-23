using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusNCache;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.NCacheHealthCheck;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.BackgroundTasks.Configuration;
using Ordering.BackgroundTasks.Tasks;
using RabbitMQ.Client;
using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using Alachisoft.NCache.Client;

namespace Ordering.BackgroundTasks
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
            //add health check for this service
            services.AddCustomHealthCheck(Configuration);

            //configure settings

            services.Configure<BackgroundTaskSettings>(Configuration);
            services.AddOptions();

            //configure background task

            services.AddSingleton<IHostedService, GracePeriodManagerService>();

            //configure event bus related services

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

            RegisterEventBus(services);

            //create autofac based service provider
            var container = new ContainerBuilder();
            container.Populate(services);


            return new AutofacServiceProvider(container.Build());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
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

                    string ncacheSubscriptionName = "OrderingTaskSubscription";

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
                    name: "OrderingTaskDB-check",
                    tags: new string[] { "orderingtaskdb" });

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                hcBuilder
                    .AddAzureServiceBusTopic(
                        configuration["EventBusConnection"],
                        topicName: "eshop_event_bus",
                        name: "orderingtask-servicebus-check",
                        tags: new string[] { "servicebus" });
            }
            else if (configuration.GetValue<bool>("RabbitMQBusEnabled"))
            {
                hcBuilder
                    .AddRabbitMQ(
                        $"amqp://{configuration["EventBusConnection"]}",
                        name: "orderingtask-rabbitmqbus-check",
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
                    name: "orderingtask-ncacheeventbus-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "ncacheeventbus" },
                    cacheConnectionOptions: new CacheConnectionOptions
                    {
                        ServerList = servers
                    });
            }

            return services;
        }
    }
}
