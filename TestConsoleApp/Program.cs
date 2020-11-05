using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace TestConsoleApp
{
    class Program
    {
        private static IConfiguration _configuration = null;

        private static IConfigurationRefresher _refresher = null;

        private static string settingKey = "TestConsoleApp:Settings:Message";

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder
                .AddAzureAppConfiguration(options =>
                {
                    options
                        .Connect(Environment
                            .GetEnvironmentVariable("ConnectionString"))
                        .ConfigureRefresh(refresh =>
                        {
                            refresh
                                .Register(settingKey)
                                .SetCacheExpiration(TimeSpan.FromSeconds(10));
                        });

                    _refresher = options.GetRefresher();
                });

            _configuration = builder.Build();
            PrintMessage().Wait();
        }

        private static async Task PrintMessage()
        {
            Console
                .WriteLine(_configuration[settingKey]
                    ?? "Hello world!");

            // Wait for the user to press Enter
            Console.ReadLine();

            await _refresher.TryRefreshAsync();
            Console
                .WriteLine(_configuration[settingKey]
                    ?? "Hello world!");
        }
    }
}
