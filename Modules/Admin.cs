using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace neogary
{
    public class Admin : ModuleBase
    {
        private DiscordSocketClient _client;
        private ILogService _log;
        private IDataService _data;
        private PermissionService _perms;

        public Admin(ILogService log, DiscordSocketClient client, 
            IDataService data, PermissionService perms)
        {
            _log = log;
            _client = client;
            _data = data;
            _perms = perms;
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
            var user = (SocketGuildUser)Context.User;
            _log.Log(user.Roles.ToString());
            var result = "";

            List<string> modules = new List<string>();
            List<string> results = new List<string>();

            _data.Find(
                "botcommand",
                "1=1",
                r => 
                {
                    string name = r.GetString(1);
                    if (!_perms.CheckPermission(user, name))
                        return;

                    string module = r.GetString(4);

                    var index = modules.FindIndex(str => str == module);

                    if (index == -1)
                    {
                        index = modules.Count;
                        modules.Add(module);
                        results.Add("");
                    }

                    results[index] += String.Format(
                        "`{0}` | {1}\n",
                        name,
                        r.GetString(2));
                });
            
            var builder = new EmbedBuilder()
                .WithCurrentTimestamp();

            for (int i = 0; i < results.Count; i++)
            {
                builder.AddField(modules[i], results[i]);
            }

            var dmChannel = await user.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync("", false, builder);
        }

        [Command("stop")]
        [Remarks("shutdown bot")]
        public Task Stop()
        {
            _log.Log("Stopping...");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
