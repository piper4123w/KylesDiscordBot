using Discord;
using System.Collections.Generic;
using System.Linq;

namespace UsefulDiscordBot.Modules.MessageFormatting
{
		public class ChoiceEmojis
		{
				public static Emoji A => new Emoji("🇦");
				public static Emoji B => new Emoji("🇧");
				public static Emoji C => new Emoji("🇨");
				public static Emoji D => new Emoji("🇩");
				public static Emoji E => new Emoji("🇪");
				public static Emoji F => new Emoji("🇫");
				public static Emoji G => new Emoji("🇬");
				public static Emoji H => new Emoji("🇭");
				public static Emoji I => new Emoji("🇮");
				public static Emoji J => new Emoji("🇯");
				public static Emoji K => new Emoji("🇰");
				public static Emoji L => new Emoji("🇱");
				public static Emoji M => new Emoji("🇲");
				public static Emoji N => new Emoji("🇳");
				public static Emoji O => new Emoji("🇴");
				public static Emoji P => new Emoji("🇵");
				public static Emoji Q => new Emoji("🇶");
				public static Emoji R => new Emoji("🇷");
				public static Emoji S => new Emoji("🇸");
				public static Emoji T => new Emoji("🇹");
				public static Emoji U => new Emoji("🇺");
				public static Emoji V => new Emoji("🇻");
				public static Emoji W => new Emoji("🇼");
				public static Emoji X => new Emoji("🇽");
				public static Emoji Y => new Emoji("🇾");
				public static Emoji Z => new Emoji("🇿");

				public static List<Emoji> All => new List<Emoji>
				{
						A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
				};

				public int IndexOf(Emoji e)
				{
						return All.IndexOf(e);
				}

				public int IndexOf(IEmote e)
				{
						return IndexOf(new Emoji(e.ToString()));
				}

				public List<Emoji> GetNumberOfChoices(int i)
				{
						return All.Take(i).ToList();
				}
		}
}
