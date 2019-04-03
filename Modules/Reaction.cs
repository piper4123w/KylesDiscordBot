using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		public class Reaction
		{
				public async static Task<ServerUsers> getReactionUsers(Emoji emoji, IUserMessage message)
				{
						var c = message.Channel as SocketGuildChannel;
						ServerUsers users = new ServerUsers();
						var us = await message.GetReactionUsersAsync(emoji.ToString());
						foreach (var u in us)
						{
								if (!(u.IsBot || u.IsWebhook))
								{
										users.Add(c.Guild.GetUser(u.Id));
								}
						}
						return users;
				}
		}
}
