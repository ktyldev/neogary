using Discord;
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
        private DiscordSocketClient _client;
        private ILogService _log;
        private RoleService _roles;

        public Roles(ILogService log, DiscordSocketClient client, RoleService roles) 
        {
            _log = log;
            _client = client;
            _roles = roles;
        }

        [Command("role")]
        [Remarks("Assigns or removes an assignable role to the user")]
        public async Task AssignRole(string roleName)
        {
            var role = GetRole(roleName);
            var roleId = role.Id.ToString();

            if (!_roles.IsRoleAssignable(roleId))
            {
                string message = "role is not assignable";
                await ReplyAsync(message);
                throw new Exception(message);
            }

            var user = (SocketGuildUser)Context.User;
            if (user.Roles.Any(r => r.Id.ToString() == roleId))
            {
                await user.RemoveRoleAsync(role);
                await ReplyAsync("removed role: " + role.Name);
            }
            else
            {
                await user.AddRoleAsync(role);
                await ReplyAsync("added role: " + role.Name);
            }
        }

        [Command("roleassignable")]
        [Remarks("set a role to be assignable or not using the role command")]
        public Task SetAssignable(string roleName, string val)
        {
            bool assignable;
            switch (val.ToLower())
            {
                case "true":
                    assignable = true;
                    break;

                case "false":
                    assignable = false;
                    break;

                default:
                    throw new Exception("couldn't parse boolean");
            }

            var role = GetRole(roleName);
            var roleId = role.Id.ToString();
            _roles.SetRoleAssignable(role.Id.ToString(), assignable);

            return ReplyAsync(
                String.Format(
                    "set {0} assignable: {1}",
                    role.Name,
                    assignable));
        }

        private IRole GetRole(string roleName)
        {
            var role = _client
                .Guilds
                .Single()
                .Roles
                .SingleOrDefault(r => 
                {
                    return r.Name.ToLower() == roleName.ToLower(); 
                });

            if (role == null)
                throw new Exception("role does not exist");

            return role;
        }
    }
}
