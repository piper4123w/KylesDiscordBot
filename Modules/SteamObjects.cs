using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
    //common games
    public class Game
    {
        public int appid { get; set; }
        public int playtime_forever { get; set; }
        public int? playtime_2weeks { get; set; }

        public Data data;

        internal void populateGameInfo()
        {
            string uri = "http://store.steampowered.com/api/appdetails?appids=" + appid.ToString();
            //Console.WriteLine(uri);

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
            Console.WriteLine(strResponse);
            //TODO: parse json into app data (just grab the name if it is too hard)

            data = JsonConvert.DeserializeObject<Data>(strResponse);
            Console.WriteLine("type:" + data.ToString());
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
        public string nickname;

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
            return nickname + ',' + discordID + ',' + steamID;
        }

        public SteamUser(string data)
        {
            string[] strings = data.Split(',');
            nickname = strings[0];
            discordID = strings[1];
            steamID = strings[2];
        }

        public SteamUser(string n, string di, string si)
        {
            nickname = n;
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


    //------------------------------------------
    public class PcRequirements
    {
        public string minimum { get; set; }
    }

    public class MacRequirements
    {
        public string minimum { get; set; }
    }

    public class LinuxRequirements
    {
        public string minimum { get; set; }
    }

    public class PriceOverview
    {
        public string currency { get; set; }
        public int initial { get; set; }
        public int final { get; set; }
        public int discount_percent { get; set; }
    }

    public class Sub
    {
        public int packageid { get; set; }
        public string percent_savings_text { get; set; }
        public int percent_savings { get; set; }
        public string option_text { get; set; }
        public string option_description { get; set; }
        public string can_get_free_license { get; set; }
        public bool is_free_license { get; set; }
        public int price_in_cents_with_discount { get; set; }
    }

    public class PackageGroup
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string selection_text { get; set; }
        public string save_text { get; set; }
        public int display_type { get; set; }
        public string is_recurring_subscription { get; set; }
        public IList<Sub> subs { get; set; }
    }

    public class Platforms
    {
        public bool windows { get; set; }
        public bool mac { get; set; }
        public bool linux { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class Genre
    {
        public string id { get; set; }
        public string description { get; set; }
    }

    public class Screenshot
    {
        public int id { get; set; }
        public string path_thumbnail { get; set; }
        public string path_full { get; set; }
    }

    public class Recommendations
    {
        public int total { get; set; }
    }

    public class ReleaseDate
    {
        public bool coming_soon { get; set; }
        public string date { get; set; }
    }

    public class SupportInfo
    {
        public string url { get; set; }
        public string email { get; set; }
    }

    public class Data
    {
        public string type { get; set; }
        public string name { get; set; }
        public string steam_appid { get; set; }
        public string required_age { get; set; }
        public string is_free { get; set; }
        public string detailed_description { get; set; }
        public string about_the_game { get; set; }
        public string short_description { get; set; }
        public string supported_languages { get; set; }
        public string header_image { get; set; }
        public string website { get; set; }
        public string pc_requirements { get; set; }
        public string mac_requirements { get; set; }
        public string linux_requirements { get; set; }
        public IList<string> developers { get; set; }
        public IList<string> publishers { get; set; }
        public string price_overview { get; set; }
        public IList<string> packages { get; set; }
        public IList<string> package_groups { get; set; }
        public string platforms { get; set; }
        public IList<string> categories { get; set; }
        public IList<string> genres { get; set; }
        public IList<string> screenshots { get; set; }
        public string recommendations { get; set; }
        public string release_date { get; set; }
        public string support_info { get; set; }
        public string background { get; set; }
    }

    public class AppRequestDetails
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }

    public class Example
    {
        public AppRequestDetails appRequest { get; set; }
    }
}
