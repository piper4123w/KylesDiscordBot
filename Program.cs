using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace UsefulDiscordBot
{
		class Program
    {
				private string _token = "NTY1MzY1NzU5NDg5MDgxMzQ1.XLOcNg.iN8ePEp_xmPFFbtkR6AFAIu27GQ";

				private DiscordSocketClient _client;
				private CommandService _commands;
				private IServiceProvider _services;

				static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
						_commands = new CommandService();

						_services = new ServiceCollection()
								.AddSingleton(_client)
								.AddSingleton(_commands)
								.BuildServiceProvider();

						await RegisterCommandsAsync();
						await _client.LoginAsync(Discord.TokenType.Bot, _token);
						await _client.StartAsync();

            await Task.Delay(-1);
        }
				

				public async Task RegisterCommandsAsync()
				{
						_client.MessageReceived += HandleCommandAsync;
						_client.ReactionAdded += HandleReactionAsync;

						await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
				}

				internal async Task HandleCommandAsync(SocketMessage arg)
				{
						var msg = arg as SocketUserMessage;
						if (msg == null || msg.Author.IsBot) return;
						Console.WriteLine("Message Recieved:{0}\n\tAuthor:{1}\n\tChannel:{2}\n\tContent{3}", DateTime.Now, msg.Author, msg.Channel, msg.Content);

						int argPos = 0;

						if (msg.HasCharPrefix('^', ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
						{
								var context = new SocketCommandContext(_client, msg);
								var result = await _commands.ExecuteAsync(context, argPos, _services);
								if (!result.IsSuccess)
								{
										Console.WriteLine("Error:{0}", result.ErrorReason);
								}
						}
				}

				internal async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
				{
						var msg = await cache.DownloadAsync();
						if (reaction.User.Value.Id != _client.CurrentUser.Id && msg.Author.Id == _client.CurrentUser.Id)
						{
								Console.WriteLine("Reaction recieved " + msg.Content);
								Console.WriteLine(msg.Content + ',' + msg.Timestamp);
								if (msg.Content.Contains("Scrimmage"))
								{
										Modules.Scrimmage.ScrimmageReactions m = new Modules.Scrimmage.ScrimmageReactions();
										await m.HandleReaction(reaction, msg);
								}
								if (msg.Content.Contains("Decision"))
								{
										Modules.Decision.DecisionReactions m = new Modules.Decision.DecisionReactions();
										await m.HandleReaction(reaction, msg);
								}
						}
				}


		}
}
