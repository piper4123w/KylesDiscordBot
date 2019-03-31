using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
								embed.Description = "Select a reaction emoji to proceed";
								embed.AddField("Player List", players.toMentionStrings());
								embed.AddField("Options", ":question: Random Teams\n:crown: I am Capitan\n:computer: Random Capitans");
								var msg = await ReplyAsync("Scrimmage", false, embed.Build());
								var emojis = new List<Emoji>
								{
										new Emoji("❓"),
										new Emoji("\uD83D\uDC51"),	//crown
										new Emoji("\uD83D\uDCBB")		//computer
								};
								foreach (var emoji in emojis)
								{
										await msg.AddReactionAsync(emoji);
								}
						}
				}

				public void HandleScrimReaction(SocketReaction reaction, IUserMessage msg)
				{
						switch (reaction.Emote.Name)
						{
								case "❓":
										Console.WriteLine("random");
										msg.DeleteAsync();
										break;
								case "\uD83D\uDC51":
										Console.WriteLine("capitan");
										break;

								case "\uD83D\uDCBB":
										msg.DeleteAsync();
										Console.WriteLine("randCapt");
										break;
								default:
										break;
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
								Title = "Setting up a Scrimmage",
								Color = new Color(8816262),
						};
				}
		}
}
