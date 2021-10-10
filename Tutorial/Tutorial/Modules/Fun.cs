using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tutorial.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Fun> _logger;

        public Fun(ILogger<Fun> logger)
            => _logger = logger;

        /// <summary>
        /// Pull back a meme from an api
        /// </summary>
        /// <returns>Memes</returns>
        [Command("meme")]
        [Alias("reddit")]
        public async Task Meme(string subReddit = null)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync($"https://reddit.com/r/{subReddit ?? "memes"}/random.json?limit=1&obey_over18=true" +
                $"");

            if (!result.StartsWith("["))
            {
                await Context.Channel.SendMessageAsync($"The subreddit {subReddit} doesn't exist!");
                return;
            }

            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());

            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(245, 212, 66))
                .WithTitle(post["title"].ToString())
                .WithUrl("https:/reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨 {post["num_comments"]} ⬆️ {post["ups"]}");

            var embed = builder.Build();

            await ReplyAsync(null, false, embed);
            _logger.LogInformation($"{Context.User.Username} executed the meme/reddit command!");
        }
    }
}
