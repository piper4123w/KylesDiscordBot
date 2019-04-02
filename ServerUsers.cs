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

				public ServerUsers(int i) : base(i)
				{
				}

				public ServerUsers(List<SocketGuildUser> list) : base(list)
				{
				}

				public ServerUsers(string str, IReadOnlyCollection<SocketGuildUser> users)
				{
						foreach (var u in users)
						{
								if (str.Contains(u.Id.ToString()))
								{
										Add(u);
								}

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

				public void RemoveUsers(ServerUsers toRemove)
				{
						foreach(var u in toRemove)
						{
								Remove(u);
						}
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
								if (IndexOf(u) != Count - 1)
								{
										s += ',';
								}
						}
						return s;
				}

				public List<string> toMentionList()
				{
						var list = new List<string>();
						foreach(var u in this)
						{
								list.Add(u.Mention);
						}
						return list;
				}
		}
}
