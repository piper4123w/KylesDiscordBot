using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		public class ScrimmageManager : ModuleBase<SocketCommandContext>
		{
				private static ServerUsers _players;
				private static Teams _teams;

				public static ServerUsers Players
				{
						get
						{
								return _players;
						}
						set
						{
								_players = value;
						}
				}

				public static Teams Teams
				{
						get
						{
								return _teams;
						}
						set
						{
								_teams = value;
						}
				}



				[Command("Scrim")]
				public async Task SetupScrimmage([Remainder] string mentionedUsers )
				{
						Players = new ServerUsers(Context.Guild.Users.ToList());

						if ((Context.User as IVoiceState).VoiceChannel != null)
						{
								Players = Players.getUsersInVoiceChannel(Context);
						}
						if (Context.Message.MentionedUsers.Count > 0)
						{
								foreach(var u in Context.Message.MentionedUsers)
								{
										var p = Context.Guild.Users.Where(guildUser => guildUser.Id == u.Id).FirstOrDefault();
										if (!Players.Contains(p))
										{
												Players.Add(p);
										}
								}
						}

						if (Players.Count > 0)
						{
								string s = "Setting up scrimmage with the following players...\n";
								s += Players.ToString();

								s += "\n Enter ?Random for Random teams" +
										 "\n Enter ?Capitan to nominate yourself as a capitan (2 required)" +
										 "\n Enter ?RandomCapitans, and I will choose 2 capitans randomly";
								await Context.Channel.SendMessageAsync(s);
						}

				}

				[Command("Random")]
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

				}
		}
}
