using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Modules
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Moderation> _logger;

        public Moderation(ILogger<Moderation> logger)
            => _logger = logger;

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
    }
}
