using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Discord;
using Discord.WebSocket;

namespace UsefulDiscordBot
{
    class Program
    {
        private DiscordSocketClient client;

        private CommandHandler handler;

        static void Main(string[] args)
        {
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync()
        {
            client = new DiscordSocketClient();

            await client.LoginAsync(TokenType.Bot, "MzM5OTYwNjg3NzUxMDY5Njk4.DFrkkw.RBqHibnk9sZf9OJoI4EO4cZQihc");

            await client.StartAsync();

            handler = new CommandHandler(client);
            await Task.Delay(-1);
        }

    }
}
