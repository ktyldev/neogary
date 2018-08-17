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
        private IServiceProvider _services;

        private ILogService _log;
        private DatabaseService _data;

        public async Task MainAsync(string[] args)
        {
            StartServices();

            _log = _services.GetService<ILogService>();

            _config = new Config();

            _client = new DiscordSocketClient();
            _commands = new Commands(_config.Prefix, _client, _services);
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

        private void StartServices()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<ILogService, ConsoleLogger>();

            _services = sc.BuildServiceProvider();
        }
    }
}
