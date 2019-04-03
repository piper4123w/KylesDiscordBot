using System.Collections.Generic;
using System.Linq;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Decision
{
		public class DecisionMessageParser
		{
				public static List<string> GetOptions(Discord.IUserMessage message)
				{
						string optionsString = "";
						foreach (var e in message.Embeds)
						{
								foreach (var field in e.Fields)
								{
										if (field.Name.Contains("Options"))
										{
												optionsString = field.Value;
												break;
										}
								}
						}
						foreach (var e in ChoiceEmojis.All)
						{   //remove emojis
								optionsString = optionsString.Replace(e.ToString(), "");
						}
						return optionsString.Split('\n').ToList();
				}
		}
}
