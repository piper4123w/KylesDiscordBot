using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UsefulDiscordBot.Models;

namespace UsefulDiscordBot.Modules
{
    //common games
    public class Game
    {
        public int appid { get; set; }
        public int playtime_forever { get; set; }
        public int? playtime_2weeks { get; set; }
        public GameRequest data = new GameRequest();

        internal void populateGameInfo()
        {
            string uri = "http://store.steampowered.com/api/appdetails?appids=" + appid.ToString();
            Stream objStream;
            StreamReader objSR;
            var encode = Encoding.GetEncoding("utf-8");

            string str = uri;
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
            HttpWebResponse getresponse = null;
            getresponse = (HttpWebResponse)wrquest.GetResponse();
            objStream = getresponse.GetResponseStream();
            objSR = new StreamReader(objStream, encode, true);
            string strResponse = objSR.ReadToEnd();
            var json = JsonConvert.DeserializeObject<JObject>(strResponse);
            if (bool.Parse(json.Property(appid.ToString())
                .Value
                .First
                .First
                .ToString()))
                data.name = JsonConvert.DeserializeObject<JObject>(strResponse).Property(appid.ToString()).Value.Last.Last["name"].ToString();
        }
    }

    public class Response
    {
        public int game_count { get; set; }
        public List<Game> games { get; set; }
    }

    public class OwnedLibrary
    {
        public Response response { get; set; }
    }

    //my class
    public class SteamUser
    {
        public string discordID;
        public string steamID;
        public string discordUserName;

        public List<Game> games;

        public bool ContainsGame(Game game)
        {
            int appid = game.appid;
            foreach (Game g in games)
            {
                if (g.appid == appid)
                    return true;
            }
            return false;
        }

        public string serialize()
        {
            return discordUserName + ',' + discordID + ',' + steamID;
        }

        public SteamUser(string data)
        {
            string[] strings = data.Split(',');
            discordUserName = strings[0];
            discordID = strings[1];
            steamID = strings[2];
        }

        public SteamUser(string n, string di, string si)
        {
            discordUserName = n;
            discordID = di;
            steamID = si;
        }

        internal void populateOwnedGameList()
        {
            string key = "D6B8FEC9AF614A1DEB6CBC08DB456C85";
            string uri = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + key + "&steamid=" + steamID + "&format=json";
            Console.WriteLine(uri);

            Stream objStream;
            StreamReader objSR;
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            string str = uri;
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
            HttpWebResponse getresponse = null;
            getresponse = (HttpWebResponse)wrquest.GetResponse();

            objStream = getresponse.GetResponseStream();
            objSR = new StreamReader(objStream, encode, true);
            string strResponse = objSR.ReadToEnd();
            //Console.WriteLine(strResponse);

            OwnedLibrary r = JsonConvert.DeserializeObject<OwnedLibrary>(strResponse);
            games = r.response.games;
        }
    }
}
