using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace neogary
{
    public class Bot 
    {
        private const string CONFIG_PATH = "config.txt";

        private Config _config;

        public async Task MainAsync(string[] args)
        {
            Console.WriteLine("bot alive");

            _config = new Config();
            Console.WriteLine(_config.Token);
            Console.WriteLine(_config.Owner);

            await Task.Delay(-1);
        }
    }
}
