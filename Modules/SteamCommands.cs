using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using Discord.Commands;
using Discord;

using System.Net;
using System.IO;

using Newtonsoft.Json;
using System.Collections;

namespace UsefulDiscordBot.Modules
{
    public class SteamCommands : ModuleBase<SocketCommandContext>
    {
        string steamFile = "steamIDs.dat";

        List<SteamUser> users;

        [Command("LinkSteamID")]
        public async Task linkUserSteamAccount([Remainder] string steamid = "")
        {
            Console.WriteLine("linking steam id");
            string discordId = Context.User.Id.ToString();
            //create the files if it doesnt exist
            if (!File.Exists(steamFile))
                File.Create(steamFile);

            users = readSteamFile();
            bool exists = false;
            foreach (SteamUser user in users)
            {
                Console.WriteLine(user.nickname);
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
            writeSteamFile(users);

        }

        [Command("populateSteamFile")]
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

            writeSteamFile(users);
        }

        [Command("compareGamesBetween")]
        [Summary("compares games between manually entered discord users. Example: ?compareGamesBetween user1,user2.....userN")]
        public async Task compareGamesBetween([Remainder] string userNames = "")
        {
            Console.WriteLine("Comparing games between entries");
            if (users == null)
                users = readSteamFile();

            string[] userNameAr = userNames.Split(',');

            List<SteamUser> comparedUsers = new List<SteamUser>();
            foreach (string s in userNameAr)
            {
                SteamUser tmp = getUser(s);
                if (tmp != null)
                {
                    comparedUsers.Add(tmp);
                    Console.WriteLine("added " + tmp.nickname);
                }else
                {
                    Console.WriteLine(s + " not registered to steamID");
                }
            }


            foreach (Game g in compareGames(comparedUsers))
                Console.WriteLine(g.data.name);

        }

        [Command("showCommonGames")]
        [Summary("shows the common games between all of the members of the Channel")]
        public async Task showCommonGames()
        {
            Console.WriteLine("getting linked users");
            if (users == null)
                users = readSteamFile();

            List<SteamUser> comparedUsers = new List<SteamUser>();
            IChannel chan = Context.Channel;
            
            foreach (IUser usr in (IEnumerable)Context.Channel.GetUsersAsync())
            {
                SteamUser tmp = getUser(usr.Id.ToString());
                if (usr.Status == UserStatus.Online && tmp != null && tmp.steamID.Length > 1)
                {
                    Console.WriteLine("adding " + usr.Username + " to comparison List");
                    comparedUsers.Add(tmp);
                }else
                {
                    Console.WriteLine(usr.Username + " has no valid steamID");
                }
            }

            if (comparedUsers.Count < 2)
            {
                Console.WriteLine("only 1 user is in comparable list, play with self");
                return;
            }
            else
            {
                Console.WriteLine("\n --common games--");
                foreach (Game g in compareGames(comparedUsers))
                    Console.WriteLine(g.data.name);
            }

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

            /*
            System.Net.WebRequest request = System.Net.WebRequest.Create();
            System.Net.WebResponse response = request.GetResponse();
            Console.WriteLine(response.ToString());*/
        }

        private void writeSteamFile(List<SteamUser> users)
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
            foreach (SteamUser u in users)
            {
                if (u.discordID.Equals(strn))
                    return u;
                if (u.nickname.Equals(strn))
                    return u;
            }
            return null;
        }

    }

}
