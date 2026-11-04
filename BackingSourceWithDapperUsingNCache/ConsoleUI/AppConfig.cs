using Microsoft.Extensions.Configuration;
using System;

namespace Alachisoft.NCache.Samples.Dapper.ConsoleUI
{
    internal static class AppConfig
    {
        public static IConfiguration Settings { get; } = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
