using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discord;

namespace UsefulDiscordBot.Modules.Embeds.Error
{
		public class PlayerCount : Discord.EmbedBuilder
		{
				string _title => "Player Count Error";

				string _description => "There are not enough players to generate a scrimmage";

				public EmbedType _type => EmbedType.Rich;

				DateTimeOffset? _timestamp => DateTime.Now;

				Color? _color => new Color(255, 0, 0);

				List<EmbedFieldBuilder> _fields => buildFields();

				public Embed Embed => buildEmbed();

				private Embed buildEmbed()
				{
						return new EmbedBuilder
						{
								Title = _title,
								Description = _description,
								Timestamp = _timestamp,
								Color = _color,
								Fields = _fields
						}.Build();
				}

				private List<EmbedFieldBuilder> buildFields()
				{
						return new List<EmbedFieldBuilder>{
								new EmbedFieldBuilder{
										Name = "Mention Players",
										Value = "example : ```\n?scrim @[User]``` will add all users in current voice channel as well as the mentioned user"
								}
						};
				}
		}
}
