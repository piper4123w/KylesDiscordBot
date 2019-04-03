using Discord;
using System.Collections.Generic;
using System.Linq;

namespace UsefulDiscordBot.Modules.MessageFormatting
{
		public class ChoiceEmojis
		{
				public Emoji A => new Emoji("🇦");
				public Emoji B => new Emoji("🇧");
				public Emoji C => new Emoji("🇨");
				public Emoji D => new Emoji("🇩");
				public Emoji E => new Emoji("🇪");
				public Emoji F => new Emoji("🇫");
				public Emoji G => new Emoji("🇬");
				public Emoji H => new Emoji("🇭");
				public Emoji I => new Emoji("🇮");
				public Emoji J => new Emoji("🇯");
				public Emoji K => new Emoji("🇰");
				public Emoji L => new Emoji("🇱");
				public Emoji M => new Emoji("🇲");
				public Emoji N => new Emoji("🇳");
				public Emoji O => new Emoji("🇴");
				public Emoji P => new Emoji("🇵");
				public Emoji Q => new Emoji("🇶");
				public Emoji R => new Emoji("🇷");
				public Emoji S => new Emoji("🇸");
				public Emoji T => new Emoji("🇹");
				public Emoji U => new Emoji("🇺");
				public Emoji V => new Emoji("🇻");
				public Emoji W => new Emoji("🇼");
				public Emoji X => new Emoji("🇽");
				public Emoji Y => new Emoji("🇾");
				public Emoji Z => new Emoji("🇿");

				public List<Emoji> All => new List<Emoji>
				{
						A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
				};

				public List<Emoji> GetNumberOfChoices(int i)
				{
						return new ChoiceEmojis().All.Take(i).ToList();
				}
		}
}
