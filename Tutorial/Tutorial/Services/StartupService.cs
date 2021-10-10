using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Tutorial.Services
{
    public class StartupService
    {
        public static IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService command, IConfigurationRoot config)
        {
            _provider = provider;
            _discord = discord;
            _commands = command;
            _config = config;
        }

        public async Task StartAsync()
        {
            string token = _config["tokens:Discord"];
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please provide your Discord token in _config.yml");
                return;
            }

            // Start the bot
            await _discord.LoginAsync(Discord.TokenType.Bot, token);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

    }
}
