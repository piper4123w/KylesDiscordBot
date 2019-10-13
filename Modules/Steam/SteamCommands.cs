using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;
using UsefulDiscordBot.Modules.UserInfoModule;

namespace UsefulDiscordBot.Modules.Steam
{
    [Group("Steam")]
    public class SteamCommands : ModuleBase<SocketCommandContext>
    {
        public string ContentTag = "Steam";

        ServerUsers _serverMembers => new ServerUsers(Context.Guild.Users.ToList());
        ServerUsers _voiceUsers => _serverMembers.getUsersInVoiceChannel(Context);
        ServerUsers users;

        [Command("LinkID")]
        [Summary("Add's 17digit SteamID number to list of knows discord/steam users")]
        public async Task linkUserSteamAccount([Remainder] string steamid = "")
        {
            steamid = steamid.Replace(" ", string.Empty);
            if (steamid.Length != 17)
            {
                await ReplyAsync(steamid + " is not a valid steamId input.Please enter the 17 digit steam ID located on your profile URL.For more help finding your SteamID see the following link https://steamidfinder.com/");
            }
            Console.WriteLine("Linking SteamID\n\tUser:{0} \n\tID:{1}", Context.Message.Author, steamid);
            new UserInfo(Context.Message.Author, new UserInfoField(InfoField.SteamId, steamid)).Save();
        }

        private List<Game> getCommonGames(ServerUsers users)
        {
            List<Game> commonGames = new List<Game>();
            foreach(SocketUser u in users)
            {
                var infoTask = new UserInfo(u).Load();
                infoTask.Wait();
                var steamUser = new SteamUser(infoTask.Result.SteamID);
                steamUser.populateOwnedGameList();
                commonGames = commonGames.Except(steamUser.games).ToList();
            }
            return commonGames;
        }

        [Command("Compare")]
        [Summary("Compares steam libraries for all linked users in the current voice channel/mentioned users")]
        public async Task CompareGames()
        {
            users = new ServerUsers();
            users.Add((SocketGuildUser)Context.Message.Author);
            users.AddRange(_voiceUsers);
            List<Game> commonGames = getCommonGames(users);
            ListEmbed embed = new ListEmbed("Common games are...");
            embed.EmbedList("Games", commonGames);
            await ReplyAsync(ContentTag, false, embed);

        }

        [Command("Compare")]
        [Summary("Compares steam libraries for all linked users in the current voice channel/mentioned users")]
        public async Task CompareGames([Remainder] string mentionedUsers)
        {
            users = new ServerUsers();
            users.Add((SocketGuildUser)Context.Message.Author);
            users.AddRange(_voiceUsers);
            foreach (var u in Context.Message.MentionedUsers)
            {
                var p = Context.Guild.Users.Where(guildUser => guildUser.Id == u.Id).FirstOrDefault();
                if (!users.Contains(p))
                {
                    users.Add(p);
                }
            }
            List<Game> commonGames = getCommonGames(users);
            ListEmbed embed = new ListEmbed("Common games are...");
            embed.EmbedList("Games", commonGames);
            await ReplyAsync(ContentTag, false, embed);
        }
    }
}
