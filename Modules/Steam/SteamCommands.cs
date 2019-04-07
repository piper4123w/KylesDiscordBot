using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using System.IO;

namespace UsefulDiscordBot.Modules
{
		public class SteamCommands : ModuleBase<SocketCommandContext>
    {
        string steamFile = "steamIDs.dat";

        List<SteamUser> users;

        [Command("LinkSteamID")]
				[Summary("Add's 17digit SteamID number to list of knows discord/steam users")]
        public async Task linkUserSteamAccount([Remainder] string steamid = "")
        {
						if(steamid.Length < 17)
						{
								Console.WriteLine(steamid + " is not a valid steamid input");
								await Context.Channel.SendMessageAsync(steamid + " is not a valid steamId input. Please enter the 17 digit steam ID located on your profile URL. For more help finding your SteamID see the following link https://steamidfinder.com/lookup/https%3A%2F%2Fsteamcommunity.com%2Fprofiles%2F76561198013452327%2F/");
						}
            Console.WriteLine("linking steam id");
            string discordId = Context.User.Id.ToString();
            //create the files if it doesnt exist
            if (!File.Exists(steamFile))
                File.Create(steamFile);

            users = readSteamFile();
            bool exists = false;
            foreach (SteamUser user in users)
            {
                Console.WriteLine(user.discordUserName);
                if (user.discordID.Equals(discordId))
                {
                    Console.WriteLine("Replacing old user data");
                    user.steamID = steamid;
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Console.WriteLine("Linking new user data");
                users.Add(new SteamUser(Context.User.Username.ToString(), discordId, steamid));
            }
            writeSteamUsersFile(users);
						await Context.Channel.SendMessageAsync(Context.User.Username + " has been added as a Steam User. ID = " + steamid);

				}

        [Command("populateSteamUserFile")]
				[Summary("Adds user's steam profile to the list of known steam profiles. This is needed to lookup steam info based on Discord ID")]
				public async Task populateSteamFile()
        {
            Console.WriteLine("populating steam file");

            //create the files if it doesnt exist
            if (!File.Exists(steamFile))
                File.Create(steamFile);

            users = readSteamFile();

            foreach (Discord.WebSocket.SocketGuildUser discordUsr in Context.Guild.Users)
            {
                if (!discordUsr.IsBot || !discordUsr.IsWebhook)
                {//skip bots and webhooks
                    bool exists = false;
                    foreach (SteamUser u in users)
                    {   //check if the user exists
                        if (u.discordID.Equals(discordUsr.Id.ToString()))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)    //add new users with empty steam ID TODO: learn how to integrate steamID into bot
                        users.Add(new SteamUser(discordUsr.Username, discordUsr.Id.ToString(), ""));
                }
            }
						writeSteamUsersFile(users);
        }

        [Command("compareGamesBetween")]
        [Summary("compares games between manually entered discord users. Example: ?compareGamesBetween user1,user2.....userN")]
        public async Task compareGamesBetween([Remainder] string userNames = "")
        {
            Console.WriteLine("Comparing games between entries");

            if (users == null)
                users = readSteamFile();

            string[] userNameAr = userNames.Split(',');

            IReadOnlyCollection<Discord.WebSocket.SocketGuildUser> serverUsers = Context.Guild.Users;
            foreach (Discord.WebSocket.SocketGuildUser usr in serverUsers)
            {
                if (userNameAr.Contains(usr.Nickname, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine(usr.Nickname + " was a nick name, replacing with user name " + usr.Username);
                    userNameAr[Array.IndexOf(userNameAr, usr.Nickname)] = usr.Username;
                }
                    
            }

            List<SteamUser> comparedUsers = new List<SteamUser>();
            foreach (string s in userNameAr)
            {
                SteamUser tmp = getUser(s);
                if (tmp != null)
                {
                    comparedUsers.Add(tmp);
                    Console.WriteLine("added " + tmp.discordUserName);
                }
                else
                {
                    await Context.Channel.SendMessageAsync(s + "is either an invalid username, or is not linked with steam.\ntry command \"?llinkSteamID (" + s + "'s Numerical SteamID)\"" + System.Environment.NewLine + "removing " + s + " from comparison");
                }
            }

            await Context.Channel.SendMessageAsync(sendComparedGames(comparedUsers));
        }

        [Command("showCommonGames")]
        [Summary("shows the common games between all of the members of the Channel")]
        public async Task showCommonGames()
        {
            Console.WriteLine("getting linked users");
            if (users == null)
                users = readSteamFile();

            List<SteamUser> comparedUsers = new List<SteamUser>();
            IReadOnlyCollection<Discord.WebSocket.SocketGuildUser> serverUsers = Context.Guild.Users;
            foreach (Discord.WebSocket.SocketGuildUser usr in serverUsers)
            {
                if (usr.VoiceChannel == (Context.User as IVoiceState).VoiceChannel && usr.Status == UserStatus.Online)
                {
                    SteamUser tmp = getUser(usr.Username);
                    Console.WriteLine(tmp.steamID);
                    if (tmp != null && !String.IsNullOrEmpty(tmp.steamID))
                    {
                        Console.WriteLine("adding " + usr.Username + " to comparison List");
                        comparedUsers.Add(tmp);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(usr.Username + "is not linked with steam.\ntry command \"?llinkSteamID (" + usr.Username + "'s Numerical SteamID)\"" + System.Environment.NewLine + "removing " + usr.Username + " from comparison");
                    }
                }
                
            }

            await Context.Channel.SendMessageAsync(sendComparedGames(comparedUsers));

        }

        private string sendComparedGames(List<SteamUser> comparedUsers)
        {
						if(comparedUsers.Count <= 1)
						{
								return "There must be at least 2 users to compare steam libraries";
						}
            Console.WriteLine("\n --Sending Common Games--");
            string message = "";
            foreach (Game g in compareGames(comparedUsers))
            {
                if (!String.IsNullOrEmpty(g.data.name))
                    message += g.data.name + System.Environment.NewLine;
                else
                    Console.WriteLine(g.appid + " has no name");
            }
            Console.WriteLine("Done");
            return "Common Games are..." + System.Environment.NewLine + "```" + message + "```";
        }

        public List<Game> compareGames(SteamUser usr1, SteamUser usr2)
        {
            List<SteamUser> comparedUsers = new List<SteamUser>();
            comparedUsers.Add(usr1);
            comparedUsers.Add(usr2);
            return compareGames(comparedUsers); //overloading as fuck!
        }

        public List<Game> compareGames(List<SteamUser> comparedUsers)
        {
            Console.WriteLine("Populating Games Lists");
            foreach (SteamUser user in comparedUsers)
            {   //populate all games owned by user
                user.populateOwnedGameList();
            }
            Console.WriteLine("Games Lists Populated");

            List<Game> commonGames = new List<Game>();

            foreach (Game g in comparedUsers[0].games)
            {   //go through all of first users games
                bool isCommon = true;
                for (int i = 1; i < comparedUsers.Count; i++)
                {

                    if (!comparedUsers[i].ContainsGame(g))
                    {   //game is not found in other library
                        isCommon = false;
                        break;
                    }
                }
                if (isCommon)
                {
                    g.populateGameInfo();
                    commonGames.Add(g);
                    Console.WriteLine("Match: " + g.data.name);
                }
            }

            Console.WriteLine("Comparison Complete");
            return commonGames;
        }

        [Command("GetOwnedGames")]
        public async Task getOwnedGames([Remainder] string userName = "")
        {
            Console.WriteLine("getting owned games");
            if (users == null)
                users = readSteamFile();

            if (userName == "")
                userName = Context.User.Username;

            SteamUser user = getUser(userName);

            user.populateOwnedGameList();

            Console.WriteLine("finished getting all " + user.games.Count + " games");
            string message = "";
            foreach (Game g in user.games)
            {
                g.populateGameInfo();
                message += g.data.name + System.Environment.NewLine;
            }
            /*
            System.Net.WebRequest request = System.Net.WebRequest.Create();
            System.Net.WebResponse response = request.GetResponse();
            Console.WriteLine(response.ToString());*/
        }

        [Command("GetSteamValue")]
        public async Task getSteamLibraryValue()
        {
            Console.WriteLine("getting owned games");
            if (users == null)
                users = readSteamFile();

            SteamUser user = getUser(Context.Message.Author.Id.ToString());

            user.populateOwnedGameList();

            float totalCost = 0f;
            foreach(Game g in user.games)
            {
                g.populateGameInfo();
                //if(!g.data.is_free)
                  //  totalCost += (g.data.price_overview.currancy * .01)
                //totalCost += g.data
            }
        }

        private void writeSteamUsersFile(List<SteamUser> users)
        {
            StreamWriter sw = new StreamWriter(steamFile);
            foreach (SteamUser usr in users)
            {
                sw.WriteLine(usr.serialize());
            }
            sw.Close();
        }

        private List<SteamUser> readSteamFile()
        {
            StreamReader sr = new StreamReader(steamFile);
            string data = sr.ReadToEnd();
            Console.WriteLine(data);
            sr.Close();
            string[] dataAr = data.Split(System.Environment.NewLine.ToCharArray());
            users = new List<SteamUser>();
            if (dataAr.Length > 0)
            {
                foreach (string s in dataAr)
                {
                    if (s.Length > 1)
                    {
                        SteamUser tmp = new SteamUser(s);
                        users.Add(tmp);
                    }
                }
            }
            else
            {
                users.Add(new SteamUser(data));
            }

            return users;
        }

        private SteamUser getUser(string strn)
        {
            Console.WriteLine(strn);
            foreach (SteamUser u in users)
            {
                if (u.discordID.Equals(strn))
                    return u;
                if(String.Equals(strn,u.discordUserName, StringComparison.OrdinalIgnoreCase))
                    return u;
            }
            return null;
        }

    }

}
