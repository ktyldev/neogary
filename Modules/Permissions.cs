using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace neogary
{
    public class Permissions : ModuleBase
    {
        private IDataService _data;

        public Permissions(IDataService data)
        {
            _data = data;
        }

        [Command("permissionnew")]
        [Remarks("create a new permission group")]
        public Task New(string name, int tier)
        {
            if (Exists(name))
                return ReplyAsync("group already exists");

            int updated = _data.Insert(
                "permission",
                "name, tier",
                String.Format("'{0}', {1}", name, tier));

            if (updated != 1)
                throw new Exception();

            return ReplyAsync(
                String.Format(
                    "added permission group {0} tier {1}", 
                    name, 
                    tier));
        }

        [Command("permissionlist")]
        [Remarks("get a list of the permission tiers")]
        public Task List()
        {
            throw new Exception();
        }

        private bool Exists(string name)
        {
            return _data.Find(
                "permission",
                String.Format("name like '{0}'", name),
                _ => { }) > 0;
        }
    }
}
