using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace neogary
{
    public class Admin : ModuleBase
    {
        private ILogService _log;
        private IDataService _data;
        private PermissionService _perms;

        public Admin(ILogService log, IDataService data, PermissionService perms)
        {
            _log = log;
            _data = data;
            _perms = perms;
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
            var user = (SocketGuildUser)Context.User;
            _log.Log(user.Roles.ToString());
            var result = "";

            _data.Find(
                "botcommand",
                "1=1",
                r => 
                {
                    string name = r.GetString(1);
                    if (!_perms.CheckPermission(user, name))
                        return;

                    result += String.Format(
                        "`{0}` | {1}\n",
                        name,
                        r.GetString(2));
                });
            
            var builder = new EmbedBuilder()
                .WithTitle("Help")
                .WithDescription(result)
                .WithCurrentTimestamp();

            var dmChannel = await user.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync("", false, builder);
        }
    }
}
