using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tutorial.DataAccess.MSSQL.Extensions;
using Tutorial.Services;

namespace Tutorial
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            //var builder = new HostBuilder()
            //    .ConfigureAppConfiguration(x =>
            //    {
            //        var configuration = new ConfigurationBuilder()
            //            .SetBasePath(Directory.GetCurrentDirectory())
            //            .AddJsonFile("appsettings.json", false, true)
            //            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            //            .Build();

            //        x.AddConfiguration(configuration);
            //    })
            //    .ConfigureLogging(x =>
            //    {
            //        x.AddConsole();
            //        x.SetMinimumLevel(LogLevel.Debug); // Defines what kind of information should be logged (e.g. Debug, Information, Warning, Critical) adjust this to your liking
            //    })
            //    .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
            //    {
            //        config.SocketConfig = new DiscordSocketConfig
            //        {
            //            LogLevel = LogSeverity.Verbose, // Defines what kind of information should be logged from the API (e.g. Verbose, Info, Warning, Critical) adjust this to your liking
            //            AlwaysDownloadUsers = true,
            //            MessageCacheSize = 200,
            //        };

            //        config.Token = context.Configuration["token"];
            //    })
            //    .UseCommandService((context, config) =>
            //    {
            //        config.CaseSensitiveCommands = false;
            //        config.LogLevel = LogSeverity.Verbose;
            //        config.DefaultRunMode = RunMode.Async;
            //    })
            //    .ConfigureServices((context, services) =>
            //    {
            //        services.AddHostedService<CommandHandler>();
            //        services.RegisterDataServices(context.Configuration);
            //    })
            //    .UseConsoleLifetime();

            var host = CreateHostBuilder(args).Build();
            using (host)
            {
                await host.RunAsync();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(x =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .Build();

                x.AddConfiguration(configuration);
            })
            .ConfigureLogging(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Debug); // Defines what kind of information should be logged (e.g. Debug, Information, Warning, Critical) adjust this to your liking
            })
            .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
            {
                config.SocketConfig = new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose, // Defines what kind of information should be logged from the API (e.g. Verbose, Info, Warning, Critical) adjust this to your liking
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 200,
                };

                config.Token = context.Configuration["token"];
            })
            .UseCommandService((context, config) =>
            {
                config.CaseSensitiveCommands = false;
                config.LogLevel = LogSeverity.Verbose;
                config.DefaultRunMode = RunMode.Async;
            })
            .ConfigureServices((context, services) =>
            {
                services.RegisterDataServices(context.Configuration);
                //services.AddTransient<CommandHandler>();
                services.AddHostedService<CommandHandler>();
            })
            .UseConsoleLifetime();
    }
}

