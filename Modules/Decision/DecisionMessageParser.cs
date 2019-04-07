using System.Collections.Generic;
using System.Linq;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Decision
{
		public class DecisionMessageParser
		{
				public static List<string> GetOptions(Discord.IUserMessage message)
				{
						string optionsString = string.Empty;
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
								optionsString = optionsString.Replace(e.ToString(), string.Empty);
						}
						optionsString = optionsString.Replace(" ", string.Empty);
						return optionsString.Split('\n').ToList();
				}

				public static string PrintFormattedOptions(List<string> options)
				{
						string output = string.Empty;
						foreach(var option in options)
						{
								output += option;
								if(option != options.Last())
								{
										output += " ";
								}
						}
						return output;
				}
		}
}
