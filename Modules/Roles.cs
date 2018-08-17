using Discord.WebSocket;
using Discord.Commands;

namespace neogary
{
    public class Roles : ModuleBase
    {
        private IDataService _data;
        private DiscordSocketClient _client;
        private ILogService _log;

        public Roles(ILogService log, IDataService data, DiscordSocketClient client) 
        {
            _log = log;
            _data = data;
            _client = client;

            _log.Log("ayy lmao");
            
            SyncRoles();
        }

        private void SyncRoles()
        {
            foreach (var g in _client.Guilds)
            {
                _log.Log(g.Name);
            }
        }
    }
}
