using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Scrimmage
{
		[ReactionHandler("Scrimmage")]
		public class ScrimmageReactions : Reaction
		{
				public static Emoji quesiton => new Emoji("\u2753");
				public static Emoji crown => new Emoji("\uD83D\uDC51");
				public static Emoji computer => new Emoji("\uD83D\uDCBB");

				SocketGuildChannel channel;
				Teams teams;
				ServerUsers players;

				public static List<Emoji> ValidReactions => new List<Emoji>(ChoiceEmojis.All){
						quesiton,
						crown,
						computer
				};

				public static List<Emoji> AllEmojis => new List<Emoji>
				{
						quesiton, crown, computer
				};

				public async Task HandleReaction(SocketReaction reaction, IUserMessage message)
				{
						channel = message.Channel as SocketGuildChannel;
						var users = channel.Guild.Users;
						players = ScrimmageMessageParser.getPlayersList(message);
						var captianUsers = await getReactionUsers(crown, message);
						teams = ScrimmageMessageParser.ParseTeams(message);

						if (reaction.Emote.Equals(quesiton))
						{ //random teams
								Console.WriteLine("random");
								await message.DeleteAsync();
								teams = new Teams(players);
						}
						else if (reaction.Emote.Equals(crown))
						{ //nominate as captain
								Console.WriteLine("captain");
								if (captianUsers.Count >= 2)
								{
										await message.DeleteAsync();
										players.RemoveUsers(captianUsers);
										teams = new Teams(new Team(captianUsers[0]), new Team(captianUsers[1]));
								}
								else
								{
										teams = null;
								}
						}
						else if (reaction.Emote.Equals(computer))
						{   //random captains
								Console.WriteLine("randCapt");
								await message.DeleteAsync();
								captianUsers = new ServerUsers();
								for (int i = 2; i > 0; i--)
								{
										int r = new Random().Next(players.Count - 1);
										captianUsers.Add(players[r]);
										players.RemoveAt(r);
								}
								teams = new Teams(new Team(captianUsers[0]), new Team(captianUsers[1]));
						}
						else if (ChoiceEmojis.All.Contains(reaction.Emote))
						{   //choice made
								if (reaction.User.Value.Mention == teams.Captain1.Mention || reaction.User.Value.Mention == teams.Captain2.Mention)
								{
										await message.DeleteAsync();
										Console.WriteLine("choice made");
										var chosenPlayer = players[ChoiceEmojis.All.IndexOf(new Emoji(reaction.Emote.ToString()))];
										players.Remove(chosenPlayer);
										teams.AddToCaptainsTeam(reaction.User.Value.Mention, chosenPlayer);
								}
								else
								{
										teams = null;
								}
						}
						Console.WriteLine(teams);
						if (teams != null)
						{
								await respondToScrimReaction(reaction);
						}
				}

				async Task respondToScrimReaction(SocketReaction reaction)
				{
						var embed = new ScrimEmbed(reaction.User.Value, "Scrimmage");
						embed.EmbedTeams(teams);
						embed.EmbedChoiceList("Remaining Player List", players.toMentionList());
						var choiceEmojis = new ChoiceEmojis().GetNumberOfChoices(players.Count);
						var newMessage = await ((IMessageChannel)(channel)).SendMessageAsync("Scrimmage", false, embed);
						foreach (var choice in choiceEmojis)
						{
								await newMessage.AddReactionAsync(choice);
						}
				}
		}
}
