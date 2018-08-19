using Discord.Commands;
using System;
using System.Linq;
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
            string result = "```\n";

            var serverRoles = _roles.All;
            int maxNameLength = serverRoles
                .Select(r => r.Name)
                .Max(s => s.Length);

            _data.Find(
                "role",
                "1=1",
                r =>
                {
                    string name = serverRoles
                        .Single(sr => sr.Id.ToString() == r.GetString(1))
                        .Name
                        .PadRight(maxNameLength);            
                    
                    result += String.Format(
                        "{0}\t{1}\n",
                        name,
                        r.GetInt32(3));
                });

            result += "```";

            return ReplyAsync(result);
        }

        [Command("permissioncommand")]
        [Remarks("set the permission tier for a command")]
        public Task SetCommandPermission(string commandName, int tier)
        {
            bool exists = _data.Find(
                "botcommand",
                String.Format("name='{0}'", commandName),
                _ => { }) == 1;
            if (!exists)
                return ReplyAsync("command does not exist");

            int updated = _data.Update(
                "botcommand",
                "permtier=" + tier,
                String.Format("name='{0}'", commandName));
            if (updated == 0)
                throw new Exception("no records updated");

            return ReplyAsync("updated command permissions");
        }

        [Command("permissioncommandlist")]
        [Remarks("list the configured command permissions")]
        public Task GetCommandPermissions()
        {
            throw new Exception();        
        }
    }
}
