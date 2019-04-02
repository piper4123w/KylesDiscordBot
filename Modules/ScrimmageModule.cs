using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

				[Command]		//no sub commands
				public async Task SetupScrimmage()
				{

				}


				[Command()]	//no sub commands + @mentions
				[Summary("Setup a new scirmmage within the current voice channel. @Mention users to add them to the scrimmage if they are not in the current voice channel.")]
				public async Task SetupScrimmage([Remainder] string mentionedUsers )
				{
						var players = _voiceUsers;
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
						var embed = scrimEmbed();

						if (players.Count > 0)
						{
								embed.Description = "Select a reaction emoji to proceed";
								embed.AddField("Player List", players.toMentionStrings());
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
								await ReplyAsync("Not Enough Players", false, new Embeds.Error.PlayerCount());
						}
				}

				public void HandleScrimReaction(SocketReaction reaction, IUserMessage msg)
				{
						var chan = msg.Channel as SocketGuildChannel;
						var users = chan.Guild.Users;
						var players = new ServerUsers(msg.Embeds.ToList()[0].Fields.Where(f => f.Name == "Player List").First().Value, users);

						Teams teams;
						switch (reaction.Emote.Name)
						{
								case "\u2753":
										Console.WriteLine("random");
										msg.DeleteAsync();
										teams = new Teams(players);	//random teams
										break;
								case "\uD83D\uDC51":
										Console.WriteLine("capitan");
										teams = null;
										break;

								case "\uD83D\uDCBB":
										msg.DeleteAsync();
										Console.WriteLine("randCapt");
										teams = null;
										break;
								default:
										teams = null;
										break;
						}

						Console.WriteLine(teams);
						((IMessageChannel)(chan)).SendMessageAsync("Scrimmage", false, teamBuilder(teams));
				}

				EmbedBuilder teamBuilder(Teams teams)
				{
						var b = new EmbedBuilder();
						b.AddInlineField("Team 1", teams.Team1.ToFormatedMentions());
						b.AddInlineField("Team 2", teams.Team2.ToFormatedMentions());
						return b;
				}

				EmbedBuilder scrimEmbed()
				{
						return new EmbedBuilder
						{
								Author = new EmbedAuthorBuilder
								{
										Name = Context.User.Username,
										IconUrl = Context.User.GetAvatarUrl()
								},
								Title = "Setting up a Scrimmage",
								Color = new Color(8816262),
						};
				}
		}
}
