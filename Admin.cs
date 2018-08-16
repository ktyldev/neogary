using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace neogary
{
    public class Admin : ModuleBase
    {
        private ILogService _log;

        public Admin(ILogService log)
        {
            _log = log;
        }

        [Command("ping")]
        [Remarks("Replies with pong")]
        public async Task Ping()
        {
            _log.Log("ping");
            await ReplyAsync("Pong");
        }

        [Command("help")]
        [Remarks("Provides names and descriptions of loaded commands")]
        public async Task Help()
        {
            var user = Context.User;
            var dmChannel = await user.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync("sup loser");
        }
    }
}
