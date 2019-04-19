using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules.UserInfoModule
{
		public class UserInfoField : Tuple<InfoField, string>
		{
				InfoField infoField => this.Item1;
				string value => this.Item2;
				public EmbedFieldBuilder embedField => new EmbedFieldBuilder
				{
						Name = infoField.ToString(),
						Value = value
				};

				public UserInfoField(InfoField item1, string item2) : base(item1, item2)
				{
				}





		}


		public enum InfoField
		{
				SteamId
		}
}
