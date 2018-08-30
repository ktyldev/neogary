using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace neogary
{
    public class MessageFilterService
    {
        private DiscordSocketClient _client;
        private ILogService _log;
        private IDataService _data;

        public List<string> Blacklist { get; private set; }

        public MessageFilterService(ILogService log, DiscordSocketClient client, 
                IDataService data)
        {
            _log = log;
            _client = client;
            _data = data;

            _client.MessageReceived += CheckMessage;

            RefreshBlacklist();
        }

        private async Task CheckMessage(SocketMessage socketMessage)
        {
            var message = (SocketUserMessage)socketMessage;

            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            foreach (string s in Blacklist)
            {
                if (socketMessage.Content.Contains(s))
                {
                    _log.Log("deleted blacklisted message");
                    await socketMessage.DeleteAsync();
                    break;
                }
            }

            await Task.CompletedTask;
        }

        public bool Add(string content)
        {
            // check db doesn't contain content
            bool result = _data.Find(
                "messageblacklist",
                String.Format("text='{0}'", content),
                _ => { }) != 0;

            // unable to add string, already exists
            if (result)
                return false;

            // add content to db
            result = _data.Insert(
                "messageblacklist",
                "text",
                String.Format("'{0}'", content)) == 1;

            // unable to update db, reason unknown
            if (!result)
                throw new Exception();

            RefreshBlacklist();
            return true;
        }

        public bool Remove(string content)
        {
            string condition = String.Format("text='{0}'", content);

            // check db contains content
            bool result = _data.Find(
                "messageblacklist",
                condition,
                _ => { }) != 0;

            // nothing to remove
            if (!result)
                return false;
            
            // remove content from db
            result = _data.Remove(
                "messageblacklist", 
                condition) == 1;
            
            // unable to remove entry; reason unknown
            if (!result)
                throw new Exception();

            RefreshBlacklist();
            return true;
        }

        private void RefreshBlacklist()
        {
            Blacklist = new List<string>();

            _data.Find(
                "messageblacklist",
                "1=1",
                r => Blacklist.Add(r.GetString(1)));
        }
    }
}
