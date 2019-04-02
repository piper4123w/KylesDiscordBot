using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Discord;

namespace UsefulDiscordBot
{
		class CommandHandler
    {
        private DiscordSocketClient client;

        private CommandService service;

        public SocketCommandContext context;

				public CommandHandler(DiscordSocketClient c)
        {
						client = c;

            service = new CommandService();
            service.AddModulesAsync(Assembly.GetEntryAssembly());

            client.MessageReceived += OnMessageReceived;
						client.ReactionAdded += OnReactionAdded;
				}

        public async Task OnMessageReceived(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            if (msg == null) return;

            context = new SocketCommandContext(client, msg);

						if (!context.IsPrivate)  //Sent in group chat
						{
								int argPos = 0;
								if (msg.HasCharPrefix('?', ref argPos))
								{
										var result = await service.ExecuteAsync(context, argPos);

										if (result.Error == CommandError.UnknownCommand)
										{
												Console.WriteLine("Error: " + result.ErrorReason);
												await context.Channel.SendMessageAsync(result.ErrorReason);
										}
										await msg.DeleteAsync();
								}
						}
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
