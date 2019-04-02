using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;

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

				public void EmbedPlayerChoiceList(string listTitle, ServerUsers users)
				{
						if (users.Count > 0)
						{
								string output = "";
								char c = 'a';
								foreach (var m in users.toMentionList())
								{
										output += ":regional_indicator_" + c++ + ": " + m + "\n";
								}
								AddField(listTitle, output);
						}
				}
		}
}
