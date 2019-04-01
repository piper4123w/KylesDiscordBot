using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;

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

            client.MessageReceived += HandleCommandAsync;

        }

        public async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;

            if (msg == null) return;

            context = new SocketCommandContext(client, msg);

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
}
