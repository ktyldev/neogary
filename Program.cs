using System;

namespace neogary
{
    class Program
    {
        static void Main(string[] args)
        {
            new Bot()
                .MainAsync(args)
                .GetAwaiter()
                .GetResult();
        }
    }
}
