using Discord.WebSocket;
using System.Collections.Generic;

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

				public void makeCapitan(SocketGuildUser captain)
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
								if (this.IndexOf(u) != this.Count - 1)
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
