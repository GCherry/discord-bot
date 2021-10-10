using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Modules
{
    public class GeneralCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<GeneralCommands> _logger;

        public GeneralCommands(ILogger<GeneralCommands> logger)
            => _logger = logger;

        /// <summary>
        /// Standard command example
        /// </summary>
        /// <returns>Pong</returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("I'm alive!");
            _logger.LogInformation($"{Context.User.Username} executed the ping command!");
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
                .WithDescription($"With this message you can see some information about {currentUser.Username}!")
                .WithColor(new Color(245, 212, 66))
                .AddField("User ID", currentUser.Id, true)
                .AddField("Discriminator", currentUser.Discriminator, true)
                .AddField("Created at", currentUser.CreatedAt.ToString("MM/dd/yyyy"), true)
                .AddField("Joined Server at", currentUser.JoinedAt.Value.ToString("MM/dd/yyyy"), true)
                .AddField("Roles", string.Join(separator: ", ", values: currentUser.Roles.Select(x => x.Mention)))
                .WithCurrentTimestamp();

            var embed = builder.Build();

            // Send message to channel
            await ReplyAsync(null, false, embed);
            _logger.LogInformation($"{Context.User.Username} executed the info command!");
        }



        /// <summary>
        /// Get server information
        /// </summary>
        /// <returns>Server info</returns>
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
                .AddField("Online Users", socketGuild.Users.Where(x=> x.Status != UserStatus.Offline).Count() + " members", true)
                .AddField("Boost Level", socketGuild.PremiumTier, true);

            var embed = builder.Build();

            // Send the message to the channel
            await ReplyAsync(null, false, embed);
            _logger.LogInformation($"{Context.User.Username} executed the server command!");
        }


    }
}
