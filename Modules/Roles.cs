using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace neogary
{
    public class Roles : ModuleBase
    {
        private IDataService _data;
        private DiscordSocketClient _client;
        private ILogService _log;
        private RoleService _roles;

        public Roles(ILogService log, IDataService data, DiscordSocketClient client, RoleService roles) 
        {
            _log = log;
            _data = data;
            _client = client;
            _roles = roles;
        }

        [Command("role")]
        [Remarks("Assigns or removes an assignable role to the user")]
        public Task AssignRole(string roleName)
        {
            bool isAssignable = _roles.IsRoleAssignable(roleName);

            _log.Log(isAssignable.ToString());
            return Task.CompletedTask;
        }

        [Command("setassignable")]
        [Remarks("set a role to be assignable or not using the role command")]
        public Task SetAssignable(string roleName, string val)
        {
            throw new Exception();
        }
    }
}
