using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Data;
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
        private ILogService _log;
        private IDataService _data;

        public Commands(string commandPrefix, DiscordSocketClient client, IServiceProvider services)
        {
            _prefix = commandPrefix;
            _services = services;
            _log = (ILogService)services.GetService(typeof(ILogService));
            _data = services.GetService<IDataService>();
            
            _client = client;
            _client.MessageReceived += HandleCommand;

            _commands = new CommandService();
            _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            UpdateCommandsInDb();
        }

        private void UpdateCommandsInDb()
        {
            _data.Find(
                "botcommand", 
                "1=1", 
                r => _log.Log(r.GetString(1) + "\t" + r.GetString(2)));
        }

        private async Task HandleCommand(SocketMessage socketMessage)
        {
            var message = (SocketUserMessage)socketMessage;

            // Ignore messages sent by the bot itself
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (!message.Content.StartsWith(_prefix))
                return;

            // where the actual command starts, after the prefix
            int argPos = _prefix.Length;

            var context = new CommandContext(_client, message);
            var result = await _commands.ExecuteAsync(
                context, 
                argPos, 
                _services);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
                //_log.Log("command not completed", result.ErrorReason);
            }
        }
    }
}
