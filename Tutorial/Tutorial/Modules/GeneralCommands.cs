using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Modules
{
    public class GeneralCommands : ModuleBase
    {
        /// <summary>
        /// Standard command example
        /// </summary>
        /// <returns>Pong</returns>
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }

        /// <summary>
        /// Embeded message example
        /// </summary>
        /// <returns>Embeded message</returns>
        [Command("info")]
        public async Task Info(SocketGuildUser user = null)
        {
            SocketGuildUser currentUser;

            currentUser = user ?? Context.User as SocketGuildUser;

            // Build the message
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(currentUser.GetAvatarUrl() ?? currentUser.GetDefaultAvatarUrl())
                .WithDescription("With this message you can see some information about yourself!")
                .WithColor(new Color(245, 212, 66))
                .AddField("User ID", currentUser.Id, true)
                .AddField("Discriminator", currentUser.Discriminator, true)
                .AddField("Created at", currentUser.CreatedAt.ToString("MM/dd/yyyy"), true)
                .AddField("Joined Server at", currentUser.JoinedAt.Value.ToString("MM/dd/yyyy"), true)
                .AddField("Roles", string.Join(separator: ", ", values: currentUser.Roles.Select(x => x.Mention)))
                .WithCurrentTimestamp();

            var embed = builder.Build();

            // Send message to channel
            await Context.Channel.SendMessageAsync(null, false, embed);
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
            var message = await Context.Channel.SendMessageAsync($"{messages.Count()} messages deleted successfully!");
            await Task.Delay(2500);
            await message.DeleteAsync();
        }

        [Command("server")]
        public async Task Server()
        {
            var socketGuild = Context.Guild as SocketGuild;

            // Build the message
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithDescription("In this message you can find some information about the server.")
                .WithTitle($"{Context.Guild.Name} Information")
                .WithColor(new Color(245, 212, 66))
                .AddField("Created at", Context.Guild.CreatedAt.ToString("MM/dd/yyyy"), true)
                .AddField("Member Count", socketGuild.MemberCount + " members", true)
                .AddField("Boost Level", socketGuild.PremiumTier, true);

            var embed = builder.Build();

            // Send the message to the channel
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
