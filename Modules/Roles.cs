using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading;
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
        public async Task AssignRole(string roleName)
        {
            var guild = _client.Guilds.Single();
            var role = guild.Roles
                .SingleOrDefault(r => r.Name.ToLower() == roleName.ToLower());
            if (role == null)
                throw new Exception("role does not exist");

            var roleId = role.Id.ToString();
            if (!_roles.IsRoleAssignable(roleId))
                throw new Exception("role is not assignable");

            var user = (SocketGuildUser)Context.User;
            if (user.Roles.Any(r => r.Id.ToString() == roleId))
            {
                await user.RemoveRoleAsync(role);
                await ReplyAsync("removed role: " + role);
            }
            else
            {
                await user.AddRoleAsync(role);
                await ReplyAsync("added role: " + role);
            }
        }

        [Command("roleassignable")]
        [Remarks("set a role to be assignable or not using the role command")]
        public Task SetAssignable(string roleName, string val)
        {
            throw new Exception();
        }
    }
}
