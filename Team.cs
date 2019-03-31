using Discord.WebSocket;
using System.Collections.Generic;

namespace UsefulDiscordBot
{
		public class Team : List<SocketGuildUser>
		{

				public void makeCapitan(SocketGuildUser captain)
				{
						if(Count > 0)
						{
								this.Clear();
						}
						this.Add(captain);
				}

				public override string ToString()
				{
						string s = "";
						foreach(var u in this)
						{
								s += u.Username;
								if (this.IndexOf(u) != this.Count - 1)
								{
										s += ',';
								}
								
						}
						return s;
				}
		}
}
