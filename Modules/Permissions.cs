using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace neogary
{
    public class Permissions : ModuleBase
    {
        private IDataService _data;
        private RoleService _roles;
        private ILogService _log;

        public Permissions(IDataService data, RoleService roles, ILogService log)
        {
            _data = data;
            _roles = roles;
            _log = log;
        }

        [Command("permissionrole")]
        [Remarks("set the permission tier for a role")]
        public Task SetRolePermission(string roleName, int tier)
        {
            var role = _roles.GetRole(roleName);
            if (role == null)
                return ReplyAsync("role does not exist");

            int updated = _data.Update(
                "role",
                "permtier=" + tier,
                String.Format("discordid='{0}'", role.Id.ToString()));

            if (updated == 0)
                throw new Exception("no records updated");

            return ReplyAsync("updated role permissions");
        }

        [Command("permissionrolelist")]
        [Remarks("list the configured role permissions")]
        public Task GetRolePermissions()
        {
            throw new Exception();        
        }

        [Command("permissioncommand")]
        [Remarks("set the permission tier for a command")]
        public Task SetCommandPermission(string commandName, int tier)
        {
            throw new Exception();        
        }

        [Command("permissioncommandlist")]
        [Remarks("list the configured command permissions")]
        public Task GetCommandPermissions()
        {
            throw new Exception();        
        }
    }
}
