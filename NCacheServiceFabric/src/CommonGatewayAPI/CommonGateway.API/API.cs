using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Serilog;
using Microsoft.AspNetCore;

namespace OcelotApiGw
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class API : StatelessService
    {
        public API(StatelessServiceContext context)
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

                        return WebHost.CreateDefaultBuilder()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .ConfigureAppConfiguration(ic => ic.AddJsonFile(Path.Combine("configuration", "configurationSF.json")))
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseStartup<Startup>()
                                    .ConfigureLogging((hostingContext, loggingbuilder) =>
                                    {
                                        loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                        loggingbuilder.AddConsole();
                                        loggingbuilder.AddDebug();
                                    })
                                    .UseSerilog((builderContext, config) =>
                                    {
                                        config
                                            .MinimumLevel.Information()
                                            .Enrich.FromLogContext()
                                            .WriteTo.Console();
                                    })
                                    .UseUrls(url)
                                    .Build();
                    }))
            };
        }
    }
}
