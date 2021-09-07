using Discord;
using System.Collections.Generic;
using System.Linq;

namespace UsefulDiscordBot.Modules.MessageFormatting
{
  public class ColorViewEmbed : EmbedBuilder
  {
    public ColorViewEmbed( string colorHex )
    {
      Title = "Admire this color...";
      Color = new Discord.Color((uint)System.Convert.ToInt32(colorHex, 16));
    }

  }
}
