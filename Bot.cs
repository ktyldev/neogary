using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace neogary
{
    public class Bot 
    {
        private const string CONFIG_PATH = "config.txt";

        private string _token;
        private string _owner;

        public async Task MainAsync(string[] args)
        {
            Console.WriteLine("bot alive");

            ReadConfig();

            await Task.Delay(-1);
        }

        private void ReadConfig()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Console.WriteLine("No config found. Creating one...");
                File.WriteAllLines(CONFIG_PATH, new [] 
                { 
                    "token=",
                    "owner="
                });

                Console.WriteLine("Please fill in the config and run again :)");
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(CONFIG_PATH);            
            _token = GetConfigValue(lines, "token");
            _owner = GetConfigValue(lines, "owner");

            if (String.IsNullOrEmpty(_token) || String.IsNullOrEmpty(_owner))
            {
                Console.WriteLine("Please fill in the config and run again :)");
                Environment.Exit(1);
            }

            Console.WriteLine(_token);
            Console.WriteLine(_owner);
        }

        private string GetConfigValue(string[] lines, string key)
        {
            return lines
                .Single(l => l.StartsWith(key))
                .Split('=')[1];
        }
    }
}
