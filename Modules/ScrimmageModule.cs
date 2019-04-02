using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.Embeds;

namespace UsefulDiscordBot.Modules
{
		[Group("Scrim")]
		public class ScrimmageManager : ModuleBase<SocketCommandContext>
		{
				Emoji quesiton = new Emoji("\u2753");
				Emoji crown = new Emoji("\uD83D\uDC51");
				Emoji computer = new Emoji("\uD83D\uDCBB");

				ServerUsers _serverMembers => new ServerUsers(Context.Guild.Users.ToList());
				ServerUsers _voiceUsers => _serverMembers.getUsersInVoiceChannel(Context);

				ServerUsers players;
				ScrimEmbed embed;

				[Command]		//no sub commands
				public async Task SetupScrimmage()
				{
						players = players ?? _voiceUsers;

						if (players.Count > 1)
						{
								embed = new ScrimEmbed(Context, "Setting up a Scrimmage");
								embed.Description = "Select a reaction emoji to proceed";
								embed.EmbedUserList("Player List", players);
								embed.AddField("Options", ":question: Random Teams\n:crown: I am Capitan\n:computer: Random Capitans");
								var msg = await ReplyAsync("Scrimmage", false, embed.Build());
								var emojis = new List<Emoji>
								{
										quesiton,
										crown,
										computer
								};
								foreach (var emoji in emojis)
								{
										await msg.AddReactionAsync(emoji);
								}
						}
						else
						{
								await ReplyAsync("Not Enough Players");
						}
				}


				[Command()]	//no sub commands + @mentions
				[Summary("Setup a new scirmmage within the current voice channel. @Mention users to add them to the scrimmage if they are not in the current voice channel.")]
				public async Task SetupScrimmage([Remainder] string mentionedUsers )
				{
						players = _voiceUsers;
						if (Context.Message.MentionedUsers.Count > 0)
						{
								foreach(var u in Context.Message.MentionedUsers)
								{
										var p = Context.Guild.Users.Where(guildUser => guildUser.Id == u.Id).FirstOrDefault();
										if (!players.Contains(p))
										{
												players.Add(p);
										}
								}
						}
						await SetupScrimmage();
				}

				public async Task HandleScrimReaction(SocketReaction reaction, IUserMessage message)
				{
						var chan = message.Channel as SocketGuildChannel;
						var users = chan.Guild.Users;
						var players = getPlayersList(message);
						var captianUsers = await getReactionUsers(crown, message);
						
						Teams teams = parseTeams(message);
						switch (reaction.Emote.Name)
						{
								case "\u2753":
										Console.WriteLine("random");
										await message.DeleteAsync();
										teams = new Teams(players);	//random teams
										break;
								case "\uD83D\uDC51":
										Console.WriteLine("capitan");
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
										break;

								case "\uD83D\uDCBB":
										Console.WriteLine("randCapt");
										await message.DeleteAsync();
										captianUsers = new ServerUsers();
										for(int i = 2; i > 0; i--)
										{
												int r = new Random().Next(players.Count - 1);
												captianUsers.Add(players[r]);
												players.RemoveAt(r);
										}
										teams = new Teams(new Team(captianUsers[0]), new Team(captianUsers[1]));
										break;
								default:
										teams = null;
										break;
						}
						Console.WriteLine(teams);
						embed = new ScrimEmbed(reaction.User.Value, "Scrimmage");
						embed.EmbedPlayerChoiceList("Remaining Players", players);
						embed.EmbedTeams(teams);
						await ((IMessageChannel)(chan)).SendMessageAsync("Scrimmage", false, embed);
				}

				Teams parseTeams(IUserMessage message)
				{
						var chan = message.Channel as SocketGuildChannel;
						var guildUsers = chan.Guild.Users;
						Teams teams = new Teams();
						foreach(var e in message.Embeds)
						{
								foreach(var f in e.Fields)
								{
										if (f.Name.Contains("Team 1"))
										{
												teams.Team1 = new Team(f.Value, guildUsers);
										}else if(f.Name.Contains("Team 2"))
										{
												teams.Team2 = new Team(f.Value, guildUsers);
										}
										if (teams.AreBothPopulated())
												return teams;
								}
						}
						return null;
				}

				ServerUsers getPlayersList(IUserMessage message)
				{
						var channel = message.Channel as SocketGuildChannel;
						var allGuildUsers = channel.Guild.Users;
						foreach (var e in message.Embeds)
						{
								foreach(var f in e.Fields)
								{
										if(f.Name.Contains("Player List"))
										{
												return new ServerUsers(f.Value, allGuildUsers);
										}
								}
						}
						return null;
				}

				async Task<ServerUsers> getReactionUsers(Emoji crown, IUserMessage message)
				{
						var c = message.Channel as SocketGuildChannel;
						ServerUsers capitans = new ServerUsers(2);
						var us = await message.GetReactionUsersAsync(crown.ToString());
						foreach (var u in us)
						{
								if (!(u.IsBot || u.IsWebhook))
								{
										capitans.Add(c.Guild.GetUser(u.Id));
								}
						}
						return capitans;
				}
		}
}
