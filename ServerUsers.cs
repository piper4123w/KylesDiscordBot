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
								s += u.Username + ",";
						}
						return s;
				}
		}
}
