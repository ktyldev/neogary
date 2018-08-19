using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace neogary
{
    public class Admin : ModuleBase
    {
        private DiscordSocketClient _client;
        private ILogService _log;
        private IDataService _data;

        public Admin(ILogService log, DiscordSocketClient client, IDataService data)
        {
            _log = log;
            _client = client;
            _data = data;
        }

        [Command("ping")]
        [Remarks("Replies with pong")]
        public async Task Ping()
        {
            _log.Log("ping");
            await ReplyAsync(String.Format("Pong! Latency is {0}ms", _client.Latency));
        }

        [Command("help")]
        [Remarks("Provides names and descriptions of loaded commands")]
        public async Task Help()
        {
            var result = "";
            _data.Find(
                "botcommand",
                "1=1",
                r => result += String.Format(
                    "`{0}`\n{1}\n",
                    r.GetString(1),
                    r.GetString(2)));

            var user = Context.User;
            var dmChannel = await user.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync(result);
        }
    }
}
