using Alachisoft.NCache.Client;
using Autofac;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Idempotency;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.AutofacModules
{

    public class ApplicationModule
        :Autofac.Module
    {

        public IConfiguration Configuration { get; }

        public ApplicationModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private ICache GetCache()
        {
            ICache cache = null;

            if (Configuration.GetValue<bool>("CachingEnabled"))
            {
                var cacheID = Configuration["CacheID"];

                if (string.IsNullOrEmpty(cacheID))
                {
                    cacheID = "OrderingCache";
                }

                var ipAddresses = Configuration.GetValue<string>("OrderingCacheIPAddresses", "").Split('-');

                if (ipAddresses.Length == 0)
                {
                    throw new ArgumentNullException("No IP addresses given for Ordering cache");
                }

                List<ServerInfo> servers = new List<ServerInfo>();

                foreach (var ipAddress in ipAddresses)
                {
                    servers.Add(new ServerInfo(ipAddress.Trim()));
                }

                cache = CacheManager.GetCache(cacheID, new CacheConnectionOptions
                {
                    ServerList = servers,
                    CommandRetryInterval = TimeSpan.FromSeconds(Configuration.GetValue("OrderingCacheCommandRetryInterval", 1)),
                    CommandRetries = Configuration.GetValue("OrderingCacheCommandRetries", 5),
                    EnableClientLogs = Configuration.GetValue("EnableOrderingCacheClientLogs", false),
                    LogLevel = LogLevel.Debug,
                    EnableKeepAlive = true,
                    KeepAliveInterval = TimeSpan.FromSeconds(60)
                });

            }

            return cache;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => GetCache()).As<ICache>().SingleInstance();
            builder.Register(ctx => Configuration).As<IConfiguration>().SingleInstance();

            builder.RegisterType<OrderQueries>()
                .As<IOrderQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BuyerRepository>()
                .As<IBuyerRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
