using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace UsefulDiscordBot
{
		public class Teams : Tuple<Team, Team>
		{
				public Team Team1 => this.Item1;
				public Team Team2 => this.Item2;

				public SocketGuildUser Capitan1 => Team1[0];
				public SocketGuildUser Capitan2 => Team2[0];

				public Teams() : base(new Team(), new Team())
				{
				}

				public Teams(Team t1, Team t2) : base(t1, t2)
				{
				}

				public Teams(ServerUsers players) : this()
				{
						bool switchBool = true;
						while (players.Count > 0)
						{
								int r = new Random().Next(0, players.Count - 1);
								if (switchBool)
								{
										Team1.Add(players[r]);
								}
								else
								{
										Team2.Add(players[r]);
								}
								players.RemoveAt(r);
								switchBool = !switchBool;
						}
				}

				public Teams(SocketGuildUser u1, SocketGuildUser u2) : this()
				{
						Team1.Add(u1);
						Team2.Add(u2);
				}

				public override string ToString()
				{
						return "Team1[" + Team1.ToString() + "],Team2[" + Team2.ToString() + "]";
				}
		}

}
