using System;

namespace neogary
{
    public class PermissionService
    {
        private IDataService _data;
        private ILogService _log;

        public PermissionService(ILogService log, IDataService data)
        {
            _data = data; 
            _log = log;
        }

        public bool HasPermission(string roleId, string commandName)
        {
            int found = 0;
            int commandPermission = -1;
            found = _data.Find(
                "botcommand",
                String.Format("name='{0}'", commandName),
                r => commandPermission = r.GetInt32(3));
            // default perms on command, allow by default
            if (commandPermission == -1)
                return true;

            int rolePermission = -1;
            found = _data.Find(
                "role",
                String.Format("discordid='{0}'", roleId),
                r => rolePermission = r.GetInt32(3));
            if (found == 0)
                return false;

            // default role perms using a configured command - reject
            if (rolePermission == -1)
                return false;

            // neither command nor role if default, return is role has clearance
            return rolePermission <= commandPermission;
        }
    }
}
