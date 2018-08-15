using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace neogary 
{
    public class Commands
    {
        private string _prefix;
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private ILogger _log;

        public Commands(string commandPrefix, DiscordSocketClient client, ILogger log)
        {
            _prefix = commandPrefix;
            
            _client = client;
            _client.MessageReceived += HandleCommand;

            _commands = new CommandService();
            _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            _services = new ServiceCollection()
                .BuildServiceProvider();

            _log = log;
        }

        private async Task HandleCommand(SocketMessage socketMessage)
        {
            var message = (SocketUserMessage)socketMessage;

            // Ignore messages sent by the bot itself
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content.StartsWith(_prefix))
            {
                _log.Log(message);
            }
        }
    }
}
