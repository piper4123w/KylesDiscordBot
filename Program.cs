using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace UsefulDiscordBot
{
		class Program
    {
				string Token = "NTYwODE2Nzg3MTM5MDY3OTA1.D37lAA.yanU7soYr1AIJuLhwbla3EqDaqY";

				private DiscordSocketClient client;

        private CommandHandler handler;

				static void Main(string[] args)
        {
            new Program().StartAsync().GetAwaiter().GetResult();
						Console.WriteLine("Online");
        }

        public async Task StartAsync()
        {
            client = new DiscordSocketClient();

            await client.LoginAsync(TokenType.Bot, Token);
            await client.StartAsync();

            handler = new CommandHandler(client);
            await Task.Delay(-1);
        }
		}
}
