using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Decision
{
		[ReactionHandler("Decision")]
		public class DecisionReactions
		{
				public async Task HandleReaction(SocketReaction reaction, IUserMessage message)
				{
						if (ChoiceEmojis.All.Contains(reaction.Emote))
						{
								Console.WriteLine("choice made");
								var options = DecisionMessageParser.GetOptions(message);
								var choice = new ChoiceEmojis().IndexOf(reaction.Emote);
								options.RemoveAt(choice);
								await new DecisionCommands().setUpDecision(options.ToString());
						}
				}
		}
}
