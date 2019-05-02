using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules.Help
{
		[Group("Help")]
		public class HelpCommands : ModuleBase<SocketCommandContext>
		{
				private readonly CommandService _service;

				public HelpCommands(CommandService service)
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
								string commandText = string.Empty;
								foreach(var command in module.Commands)
								{
										if(commandText != string.Empty)
										{
												commandText += ", ";
										}
										commandText += command.Name + ' ' + command.Remarks;
								}
								embedBuilder.AddField(module.Name, commandText);
						}

						await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
				}
		}
}
