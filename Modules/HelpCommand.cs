using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		[Group("Help")]
		public class HelpCommand : ModuleBase<SocketCommandContext>
		{
				private readonly CommandService _service;

				public HelpCommand(CommandService service)
				{
						_service = service;
				}

				[Command]
				public async Task Help()
				{
						List<CommandInfo> commands = _service.Commands.ToList();
						List<ModuleInfo> modules = _service.Modules.ToList();
						EmbedBuilder embedBuilder = new EmbedBuilder();

						foreach (ModuleInfo module in modules)
						{
								// Get the command Summary attribute information
								string embedFieldText = module.Summary ?? "No description available\n";

								embedBuilder.AddField(module.Name, embedFieldText);
						}

						await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
				}
		}
}
