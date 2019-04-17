using Discord.Commands;
using System;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.UserInfoModule;

namespace UsefulDiscordBot.Modules.Steam
{
		[Group("Steam")]
		public class SteamCommands : ModuleBase<SocketCommandContext>
		{
				[Command("LinkID")]
				[Summary("Add's 17digit SteamID number to list of knows discord/steam users")]
				public async Task linkUserSteamAccount([Remainder] string steamid = "")
				{
						steamid = steamid.Replace(" ", string.Empty);
						if(steamid.Length != 17)
						{
								await ReplyAsync(steamid + " is not a valid steamId input.Please enter the 17 digit steam ID located on your profile URL.For more help finding your SteamID see the following link https://steamidfinder.com/");
						}
						Console.WriteLine("Linking SteamID\n\tUser:{0} \n\tID:{1}", Context.Message.Author, steamid);
						var userInfo = new UserInfo(Context.Message.Author, new Tuple<InfoField, string>(InfoField.SteamId, steamid));
				}
		}
}
