using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace neogary
{
    public class MessageFilter : ModuleBase
    {
        private MessageFilterService _filter;

        public MessageFilter(MessageFilterService filter)
        {
            _filter = filter;
        }

        [Command("filteradd")]
        [Remarks("add a string to the message blacklist")]
        public async Task FilterAdd([Remainder] string content)
        {
            bool result = _filter.Add(content);
            string reply;

            if (result)
            {
                reply = String.Format("Added `{0}` to filter", content);
            }
            else
            {
                reply = "Unable to update filter";
            }

            await ReplyAsync(reply);
        }

        [Command("filterremove")]
        [Remarks("remove a string from the message blacklist")]
        public async Task FilterRemove([Remainder] string content)
        {
            bool result = _filter.Remove(content);
            string reply;

            if (result)
            {
                reply = String.Format("Removed `{0}` from filter", content);
            }
            else
            {
                reply = "Unable to update filter";
            }

            await ReplyAsync(reply);
        }

        [Command("filtershow")]
        [Remarks("show blacklisted strings")]
        public async Task FilterShow()
        {
            string reply;

            if (_filter.Blacklist.Any())
            {
                reply = "Filtered content:\n";
                
                foreach (string item in _filter.Blacklist)
                {
                    reply += String.Format("`{0}`\n", item);
                }
            }
            else
            {
                reply = "Nothing currently filtered";
            }

            await ReplyAsync(reply);
        }
    }
}
