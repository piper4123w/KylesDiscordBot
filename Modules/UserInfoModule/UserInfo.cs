using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules.UserInfoModule
{
		public class UserInfo : List<UserInfoField>
		{
				private SocketUser socketUser;
				private string InfoMessageTitle = "User Info";
				public string SteamID => this?.First(i => i.Item1 == InfoField.SteamId)?.Item2;

				public UserInfo(SocketUser user)
				{
						socketUser = user;
				}

				public UserInfo(SocketUser user, ImmutableArray<EmbedField> fields)
				{
						socketUser = user;
						foreach (var f in fields)
						{
								this.Add(new UserInfoField((InfoField)Enum.Parse(typeof(InfoField), f.Name), f.Value));
						}
				}

				public UserInfo(SocketUser user, UserInfoField field)
				{
						socketUser = user;
						this.Add(field);
				}

				public async Task<UserInfo> Load()
				{
						var chan = await socketUser.GetOrCreateDMChannelAsync();
						var msgs = await chan.GetPinnedMessagesAsync();
						foreach(var m in msgs)
						{
								var e = m.Embeds.FirstOrDefault();
								if(e.Title.Equals(InfoMessageTitle))
								{
										return new UserInfo(socketUser, m.Embeds.FirstOrDefault().Fields);
								}
						}
						return new UserInfo(socketUser);
				}

				public async void Save()
				{
						Delete();
						var chan = await socketUser.GetOrCreateDMChannelAsync();
						var m = await chan.SendMessageAsync(string.Empty, false, embed);
						await m.PinAsync();
				}

				public async void Delete()
				{
						var chan = await socketUser.GetOrCreateDMChannelAsync();
						var msgs = await chan.GetPinnedMessagesAsync();
						foreach (var m in msgs)
						{
								var e = m.Embeds.FirstOrDefault();
								if (e.Title.Equals(InfoMessageTitle))
								{
										await m.DeleteAsync();
										return;
								}
						}
						return;
				}

				public Embed embed => new EmbedBuilder
				{
						Title = InfoMessageTitle,
						Description = "This message contains your linked user info",
						Fields = new List<EmbedFieldBuilder>
								{
										new UserInfoField(InfoField.SteamId, SteamID).embedField
								},
						Footer = new EmbedFooterBuilder
						{
								Text = "Deleting this message will require you to relink your SteamID"
						}
				}.Build();
		}
}
