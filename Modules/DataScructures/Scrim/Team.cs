using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace UsefulDiscordBot
{
		public class Team : ServerUsers
		{
				public Team() : base()
				{
				}

				public Team(ServerUsers users)
				{
						foreach(var u in users)
						{
								Add(u);
						}
				}

				public Team(string str, IReadOnlyCollection<SocketGuildUser> users)
				{
						foreach(var s in str.Split('\n'))
						{
								foreach(var u in users)
								{
										if (s.Contains(u.Mention))
										{
												Add(u);
										}
								}
						}
				}

				public Team(SocketGuildUser u)
				{
						Clear();
						Add(u);
				}

				public void makeCaptain(SocketGuildUser captain)
				{
						if(Count > 0)
						{
								this.Clear();
						}
						this.Add(captain);
				}

				public bool isEmpty()
				{
						return Count == 0;
				}

				public override string ToString()
				{
						string s = "";
						foreach(var u in this)
						{
								s += u.Username;
								if (u != this.Last())
								{
										s += ',';
								}
								
						}
						return s;
				}

				public string toFormattedMentionString()
				{
						string s = "";
						foreach(var u in this)
						{
								s += u.Mention + "\n";
						}
						return s;
				}
		}
}
