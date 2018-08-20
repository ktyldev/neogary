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
        private RoleService _roles;

        public async Task MainAsync(string[] args)
        {
            _config = new Config();

            _log = new ConsoleLogger();
            _data = new DatabaseService(_config.ConnectionString, _log);
            _client = new DiscordSocketClient();
            _roles = new RoleService(_data, _log, _client); 

            RegisterServices();

            _commands = new Commands(_config.Prefix, _client, _services);

            _client.Log += m =>
            {
                _log.Log(m.ToString());
                return Task.CompletedTask;
            };

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private void RegisterServices()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<ILogService>(_log);
            sc.AddSingleton<IDataService>(_data);
            sc.AddSingleton(_config);
            sc.AddSingleton(_client);
            sc.AddSingleton(_roles);
            sc.AddSingleton(new PermissionService(_log, _data));

            _services = sc.BuildServiceProvider();
        }
    }
}
