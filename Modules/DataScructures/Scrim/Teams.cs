﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace UsefulDiscordBot
{
		public class Teams
		{
				public Team Team1;
				public Team Team2;

				public SocketGuildUser Captain1 => Team1[0];
				public SocketGuildUser Captain2 => Team2[0];

				public Teams()
				{
						Team1 = new Team();
						Team2 = new Team();
				}

				public Teams(Team t1, Team t2)
				{
						Team1 = t1;
						Team2 = t2;
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

				public void AddToCaptainsTeam(string captainMention, SocketGuildUser member)
				{
						if (Captain1.Mention.Equals(captainMention))
						{
								Team1.Add(member);
						}
						else if (Captain2.Mention.Equals(captainMention))
						{
								Team2.Add(member);
						}
				}

				public void AddToCaptainsTeam(SocketGuildUser captain, SocketGuildUser member)
				{
						if (captain.Equals(Captain1))
						{
								Team1.Add(member);
						}else if (captain.Equals(Captain2))
						{
								Team2.Add(member);
						}
				}

				public bool AreBothPopulated()
				{
						return !Team1.isEmpty() && !Team2.isEmpty();
				}

				public override string ToString()
				{
						return "Team1[" + Team1.ToString() + "],Team2[" + Team2.ToString() + "]";
				}
		}

}
