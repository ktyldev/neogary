using System;
using Discord.WebSocket;

namespace neogary 
{
    public class ConsoleLogger : ILogService
    {
        private const string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss";

        public void Log(string message, string source = "")
        {
            Console.WriteLine(
                "{0}\t{1}",
                DateTime.Now.ToString(DATE_FORMAT),
                message); 
        }

        public void Log(SocketUserMessage message)
        {
            Console.WriteLine(
                "{0}\t{1}\t{2}\t{3}",
                DateTime.Now.ToString(DATE_FORMAT),
                message.Channel.Id,
                message.Author.Username,
                message.Content);
        }
    }
}
