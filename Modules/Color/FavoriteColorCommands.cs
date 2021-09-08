using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;
using UsefulDiscordBot.Modules.UserInfoModule;

namespace UsefulDiscordBot.Modules.Color
{
  [Group("Color")]
  public class FavoriteColorCommands : ModuleBase<SocketCommandContext>
  {
    public string ContentTag = "Color";

    [Command("Admire")]
    [Summary("allow all other users to admire the color you have chosen")]
    public async Task ViewColor([Remainder] string colorHex = "")
    {
      colorHex = colorHex.Replace(" ", string.Empty);
      ColorViewEmbed embed = new ColorViewEmbed(colorHex);
      await ReplyAsync(ContentTag, false, embed);
    }
  }
}
