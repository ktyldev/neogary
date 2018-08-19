using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace neogary
{
    public class RoleService
    {
        private IDataService _data;
        private ILogService _log;
        private DiscordSocketClient _client;

        public RoleService(IDataService data, ILogService log, DiscordSocketClient client)
        {
            _data = data; 
            _log = log;
            _client = client;

            _client.Ready += () =>
            {
                SyncRoles();
                return Task.CompletedTask;
            };
        }

        private void SyncRoles()
        {
            var dbRoles = new List<string>();
            _data.Find(
                "role",
                "1=1",
                r => dbRoles.Add(r.GetString(1)));
            
            var guild = _client
                .Guilds
                .SingleOrDefault();

            if (guild == null)
                throw new Exception();

            var serverRoles = guild
                .Roles
                .Where(r => r.Name.ToLower() != "@everyone")
                .Select(r => r.Id.ToString());

            var allRoles = dbRoles.Concat(serverRoles);
            
            _log.Log("syncing server roles to DB");
            int updated = 0;
            foreach (var r in allRoles)
            {
                bool onServer = serverRoles.Contains(r);
                bool inDb = dbRoles.Contains(r);

                if (inDb && onServer)
                    continue;

                if (inDb && !onServer)
                {
                    _data.Remove(
                        "role",
                        String.Format("discordid = '{0}'", r));
                    updated++;
                }
                else if (!inDb && onServer)
                {
                    _data.Insert(
                        "role",
                        "discordid, isassignable",
                        String.Format("'{0}', true", r));
                    updated++;
                }
            }
            _log.Log(String.Format("updated {0} roles in DB", updated));
        }

        public bool IsRoleAssignable(string roleId)
        {
            bool result = false;
            _data.Find(
                "role",
                String.Format("discordid = '{0}'", roleId),
                r => result = r.GetBoolean(2));

            return result;
        }

        public void SetRoleAssignable(string roleId, bool assignable)
        {
            int updated = _data.Update(
                "role",
                String.Format(
                    "isassignable={0}", 
                    assignable ? "true" : "false"),
                String.Format("discordid='{0}'", roleId));

            if (updated != 1)
                throw new Exception();
        }
    }
}
