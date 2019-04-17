using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules.UserInfoModule
{
		public class UserInfo
		{
				private IMessage userInfoMessage;
				public string SteamID;

				public UserInfo(SocketUser user)
				{
						populateUserInfo(user);
				}

				public UserInfo(SocketUser user, Tuple<InfoField, string> pair)
				{
						populateUserInfo(user);			//This is being cut half way through
						switch (pair.Item1)
						{
								case InfoField.SteamId:
										SteamID = pair.Item2;
										break;
						}
						build(user);
				}

				public async void build(SocketUser user)
				{
						IMessageChannel channel;
						if(userInfoMessage == null)
						{
								channel = await user.GetOrCreateDMChannelAsync();
						}
						else
						{
								channel = userInfoMessage.Channel;
								await userInfoMessage.DeleteAsync();
						}
						
						var msg = await channel.SendMessageAsync(string.Empty, false, embed);
						await msg.PinAsync();
				}

				async void populateUserInfo(SocketUser user)
				{
						var dmChan = await user.GetOrCreateDMChannelAsync();
						var msgs = await dmChan.GetPinnedMessagesAsync();
						foreach (var m in msgs)
						{
								var e = m.Embeds.FirstOrDefault();
								if (e.Title.Contains("User Info"))
								{
										userInfoMessage = m;
										SteamID = e.Fields.First(f => f.Name.Contains(InfoField.SteamId.ToString())).Value;
										return;
								}
						}
				}

				public Embed embed => new EmbedBuilder
				{
						Title = "User Info",
						Description = "This message contains your linked user info",
						Fields = new List<EmbedFieldBuilder>
								{
										new EmbedFieldBuilder
										{
												Name = InfoField.SteamId.ToString(),
												Value = SteamID
										}
								},
						Footer = new EmbedFooterBuilder
						{
								Text = "Deleting this message will require you to relink your SteamID"
						}
				}.Build();
		}

		public enum InfoField
		{
				SteamId
		}
}
