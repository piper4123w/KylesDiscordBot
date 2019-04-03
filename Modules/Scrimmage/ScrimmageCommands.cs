using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Scrimmage
{
		[Group("Scrim")]
		public class ScrimmageCommands : ModuleBase<SocketCommandContext>
		{

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
								var emojis = Scrimmage.ScrimmageReactions.AllEmojis;
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
						if (mentionedUsers.Contains("@everyone"))
						{
								foreach(var u in Context.Guild.Users)
								{
										players.Add(u);
								}
						}
						await SetupScrimmage();
				}
		}
}
