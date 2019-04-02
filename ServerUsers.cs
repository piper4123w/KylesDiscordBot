using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
						ServerUsers users = new ServerUsers();
						if ((context.User as IVoiceState).VoiceChannel != null)
						{
								foreach (var u in context.Guild.Users)
								{
										if ((u as IVoiceState)?.VoiceChannel?.Id == (context.User as IVoiceState)?.VoiceChannel?.Id)
										{
												users.Add(u);
										}
								}
						}
						return users;
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
