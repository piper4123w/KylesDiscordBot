using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Decision
{
		[Group("decide")]
		public class DecisionCommands : ModuleBase<SocketCommandContext>
		{
				DecisionEmbed embed;
				public string ContentTag = "Decision";

				[Command]
				public async Task setUpDecision([Remainder] string optionsString)
				{
						embed = new DecisionEmbed(Context, ContentTag);
						embed.Description = "Select a reaction emoji to proceed";
						var options = optionsString.Split(' ').ToList();
						embed.EmbedOpitons("Options", options);
						var msg = await ReplyAsync("", false, embed);
						var choiceEmojis = new ChoiceEmojis().GetNumberOfChoices(options.Count);
						foreach (var e in choiceEmojis)
						{
								await msg.AddReactionAsync(e);
						}
				}
		}
}
