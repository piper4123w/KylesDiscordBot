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

						client.ReactionAdded += OnReactionAdded;

            await client.StartAsync();

            handler = new CommandHandler(client);
            await Task.Delay(-1);
        }

				async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
				{
						var msg = await cache.DownloadAsync();
						if (reaction.User.Value.Id != client.CurrentUser.Id && msg.Author.Id == client.CurrentUser.Id)
						{								
								Console.WriteLine("Reaction recieved");
								Console.WriteLine(msg.Content + ',' + msg.Timestamp);
								if (msg.Content.Contains("Scrimmage"))
								{
										Modules.ScrimmageManager m = new Modules.ScrimmageManager();
										m.HandleScrimReaction(reaction, msg);
								}
						}
				}
		}
}
