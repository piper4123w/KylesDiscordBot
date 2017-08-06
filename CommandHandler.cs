using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
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

        //SocketUserMessage initializeMessage = null;

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

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
