using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		public class ScrimmageManager : ModuleBase<SocketCommandContext>
		{

				[Command("Scrim")]
				[Summary("Setup a new scirmmage within the current voice channel. @Mention users to add them to the scrimmage if they are not in the current voice channel.")]
				public async Task SetupScrimmage([Remainder] string mentionedUsers )
				{
						var serverMembers = new ServerUsers(Context.Guild.Users.ToList());
						var players = new ServerUsers();

						if ((Context.User as IVoiceState).VoiceChannel != null)
						{  //if user is in a voice channel add all the users in that voice channel
								players = serverMembers.getUsersInVoiceChannel(Context);
						}
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
								embed.Description = "Setting up scrimmage with the following players...";
								embed.AddField("Player List", players.toMentionStrings());

								string s = "Setting up scrimmage with the following players...\n";
								s += players.ToString();

								await Context.Channel.SendMessageAsync("", false, embed.Build());
						}
				}

				private EmbedBuilder scrimEmbed()
				{
						return new EmbedBuilder
						{
								Author = new EmbedAuthorBuilder
								{
										Name = Context.User.Username,
										IconUrl = Context.User.GetAvatarUrl()
								},
								Title = "Scrimmage",
								Color = new Color(8816262),
						};
				}


				/*[Command("Random")]
				public async Task RandomTeamScrimmage()
				{
						Console.WriteLine("generating random teams with " + Players.ToString());
						Teams = new Teams(Players);
						
						await Context.Channel.SendMessageAsync(Teams.ToString());
				}

				[Command("ImCapitan")]
				public async Task setPlayerCapitan()
				{
						
				}

				[Command("RandomCapitans")]
				public async Task setRandomCapitans()
				{

				}*/
		}
}
