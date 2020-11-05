using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder
                .AddAzureAppConfiguration(Environment
                    .GetEnvironmentVariable("ConnectionString"));

            var config = builder.Build();
            Console
                .WriteLine(config["TestConsoleApp:Settings:Message"]
                    ?? "Hello world!");
        }
    }
}
