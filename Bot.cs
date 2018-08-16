using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace neogary
{
    public class Bot 
    {
        private const string CONFIG_PATH = "config.txt";

        private Config _config;
        private DiscordSocketClient _client;
        private Commands _commands;
        private ILogService _log;
        private DatabaseService _data;

        public async Task MainAsync(string[] args)
        {
            var sc = new ServiceCollection();
            sc.TryAddSingleton<ILogService, ConsoleLogger>();

            var services = sc.BuildServiceProvider();

            _log = (ILogService)services.GetService(typeof(ILogService));

            _config = new Config();

            _client = new DiscordSocketClient();
            _commands = new Commands(_config.Prefix, _client, services);
            _data = new DatabaseService(_config.ConnectionString, _log);

            _client.Log += m =>
            {
                _log.Log(m.ToString());
                return Task.CompletedTask;
            };

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
