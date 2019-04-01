using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UsefulDiscordBot
{
		public class ServerUsers : List<SocketGuildUser>
		{
				public ServerUsers()
				{
				}

				public ServerUsers(List<SocketGuildUser> list) : base(list)
				{
				}

				public ServerUsers(string str, IReadOnlyCollection<SocketGuildUser> users)
				{
						var split = str.Split(',');
						foreach(var s in split)
						{
								Add(users.Where(u => s.Contains(u.Id.ToString())).ToList()[0]);
						}
				}

				public ServerUsers getUsersInVoiceChannel(SocketCommandContext context)
				{
						return getUsersInVoiceChannel((context.User as IVoiceState).VoiceChannel);
				}

				public ServerUsers getUsersInVoiceChannel(IVoiceChannel channel)
				{
						Console.WriteLine("Getting users in voice channel");
						var sus = new ServerUsers();
						foreach(var u in this)
						{
								if (u.VoiceChannel == channel)
										sus.Add(u);
						}
						return sus;
				}

				public override string ToString()
				{
						string s = "";
						foreach(var u in this)
						{
								s += u.Username;
								if(this.IndexOf(u) != this.Count - 1)
								{
										s += ',';
								}
						}
						return s;
				}

				public string toMentionStrings()
				{
						string s = "";
						foreach (var u in this)
						{
								s += u.Mention;
								if (this.IndexOf(u) != this.Count - 1)
								{
										s += ',';
								}
						}
						return s;
				}
		}
}
