using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Microsoft.eShopOnContainers.Web.Shopping.HttpAggregator
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class ShoppingAggregator : StatelessService
    {
        public ShoppingAggregator(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var host = new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureAppConfiguration((builderContext, config) =>
                                    {
                                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                                        config.AddEnvironmentVariables();
                                    })
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .CaptureStartupErrors(true)
                                    .UseSetting("detailedErrors","true")
                                    .UseStartup<Startup>()
                                    .UseSerilog((builderContext, config) =>
                                    {
                                        config
                                            .MinimumLevel.Information()
                                            .Enrich.FromLogContext()
                                            .WriteTo.Console();
                                    })
                                    .UseUrls(url)
                                    .Build();

                        return host;
                    }))
            };
        }
    }
}
