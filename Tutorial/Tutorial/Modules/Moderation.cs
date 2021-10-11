using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.DataAccess.MSSQL.Interfaces;

namespace Tutorial.Modules
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Moderation> _logger;
        private readonly IServerRepository _servers;
        private readonly IConfiguration _config;

        public Moderation(ILogger<Moderation> logger, IServerRepository servers, IConfiguration config)
        {
            _logger = logger;
            _servers = servers;
            _config = config;
        }

        /// <summary>
        /// Delete X number of the latest messages in the current channel
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Success/Fail message</returns>
        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amount)
        {
            var socketTextChannel = Context.Channel as SocketTextChannel;

            // Send message to channel
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();

            // Delete messages
            await socketTextChannel.DeleteMessagesAsync(messages);

            // Send message to channel
            var message = await ReplyAsync($"{messages.Count()} messages deleted successfully!");
            await Task.Delay(2500);
            await message.DeleteAsync();
            _logger.LogInformation($"{Context.User.Username} executed the purge command!");
        }

        [Command("prefix")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Purge(string prefix = null)
        {
            if (prefix == null)
            {
                var guildPrefix = await _servers.GetGuildPrefix(Context.Guild.Id) ?? _config["prefix"];
                await ReplyAsync($"The current prefix of this bot is `{guildPrefix}`");
                return;
            }

            if (prefix.Length > 8)
            {
                await ReplyAsync("The length of the new prefix is to long! 8 please use characters max.");
            }
            await _servers.ModifyGuildPrefix(Context.Guild.Id, prefix, Context.Guild.Name);
            await ReplyAsync($"The prefix of this bot has been changed to `{prefix}`");
        }
    }
}
