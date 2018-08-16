using System;
using System.IO;
using System.Linq;

namespace neogary
{
    public class Config
    {
        private const string CONFIG_PATH = "config.txt";

        public string Token { get; private set; }
        public string Owner { get; private set; }
        public string Prefix { get; private set; }
        public string ConnectionString { get; private set; }

        public Config()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Console.WriteLine("No config found. Creating one...");
                File.WriteAllLines(CONFIG_PATH, new [] 
                { 
                    "token=",
                    "owner=",
                    "prefix=",
                    "connectionstring"
                });

                Console.WriteLine("Please fill in the config and run again :)");
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(CONFIG_PATH);            
            Token = GetConfigValue(lines, "token");
            Owner = GetConfigValue(lines, "owner");
            Prefix = GetConfigValue(lines, "prefix");
            ConnectionString = GetConfigValue(lines, "connectionstring");

            var configValues = new []
            {
                Token, 
                Owner, 
                Prefix, 
                ConnectionString
            };

            if (configValues.Any(s => s == ""))
            {
                Console.WriteLine("Please fill in the config and run again :)");
                Environment.Exit(1);
            }
        }

        private string GetConfigValue(string[] lines, string key)
        {
            return String.Join("=", 
                lines
                .Single(l => l.StartsWith(key))
                .Split('=')
                .Skip(1));
;
        }
    }
}


