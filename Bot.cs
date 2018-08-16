using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace neogary
{
    public class Bot 
    {
        private const string CONFIG_PATH = "config.txt";

        private Config _config;
        private DiscordSocketClient _client;
        private Commands _commands;
        private ILogger _log;
        private DataAccess _data;

        public async Task MainAsync(string[] args)
        {
            _log = new ConsoleLogger();

            _config = new Config();

            _client = new DiscordSocketClient();
            _commands = new Commands(_config.Prefix, _client, _log);
            _data = new DataAccess(_config.ConnectionString, _log);

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
