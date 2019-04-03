using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Scrimmage
{
		public class ScrimmageMessageParser
		{
				public static Teams ParseTeams(IUserMessage message)
				{
						var chan = message.Channel as SocketGuildChannel;
						var guildUsers = chan.Guild.Users;
						Teams teams = new Teams();
						foreach (var e in message.Embeds)
						{
								foreach (var f in e.Fields)
								{
										if (f.Name.Contains("Team 1"))
										{
												teams.Team1 = new Team(f.Value, guildUsers);
										}
										else if (f.Name.Contains("Team 2"))
										{
												teams.Team2 = new Team(f.Value, guildUsers);
										}
										if (teams.AreBothPopulated())
												return teams;
								}
						}
						return null;
				}

				public static ServerUsers getPlayersList(IUserMessage message)
				{
						var channel = message.Channel as SocketGuildChannel;
						var allGuildUsers = channel.Guild.Users;
						foreach (var e in message.Embeds)
						{
								foreach (var f in e.Fields)
								{
										if (f.Name.Contains("Player List"))
										{
												return new ServerUsers(f.Value, allGuildUsers);
										}
								}
						}
						return null;
				}
		}
}
