using Discord;
using Discord.Commands;
using System.Linq;
using System.Collections.Generic;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Embeds
{
		public class ScrimEmbed : EmbedBuilder
		{
				Color color => new Color(8816262);

				public ScrimEmbed(IUser user, string title)
				{
						Author = new EmbedAuthorBuilder
						{
								Name = user.Username,
								IconUrl = user.GetAvatarUrl()
						};
						Title = "Setting up a Scrimmage";
						Color = color;
				}

				public ScrimEmbed(SocketCommandContext context, string title) : this(context.User, title)
				{						
				}

				public void EmbedTeams(Teams teams)
				{

						AddInlineField("Team 1", teams.Team1.toFormattedMentionString());
						AddInlineField("Team 2", teams.Team2.toFormattedMentionString());
				}

				public void EmbedUserList(string listTitle, ServerUsers users)
				{
						if(users.Count> 0)
								AddField(listTitle, users.toMentionStrings());
				}

				public void EmbedChoiceList<T>(string listTitle, List<T> list)
				{
						if (list?.Count > 0)
						{
								string output = "";
								var choiceEmojis = new ChoiceEmojis();
								int i = 0;
								foreach (var item in list)
								{
										output += choiceEmojis.All[i++] + ":" + item + "\n";
								}
								AddField(listTitle, output);
						}
				}

				public List<Emoji> GenerateChoiceReactionList(int i)
				{
						return new ChoiceEmojis().All.Take(i).ToList();
				}
		}
}
