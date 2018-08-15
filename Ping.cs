using Discord.Commands;
using System.Threading.Tasks;

namespace neogary
{
    public class Ping : ModuleBase
    {
        [Command("ping")]
        [Remarks("Replies with pong")]
        public async Task Hello()
        {
            await ReplyAsync("Pong");
        }
    }
}
