using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		public class MediaCommands : ModuleBase<SocketCommandContext>
    {
        [Command("showUploads")]
        public async Task sendUploads([Remainder] int images = 0)
        {
            Console.WriteLine("Getting Uploads");
            foreach (var Item in await Context.Channel.GetMessagesAsync(1000).Flatten())
            {

                if (Item.Attachments.Count > 0 && images > 0)
                {
                    var file = Item.Attachments.FirstOrDefault();
                    await Context.User.SendMessageAsync(file.Url);
                    Console.WriteLine(file.Url);
                    images--;
                }
            }
						Console.WriteLine("Done Getting Uploads");
        }

        [Command("showGallery")]
        public async Task sendGallery([Remainder] int count = 0)
        {
            Console.WriteLine("Getting Gallery");
            List<string> images = new List<string>();
            foreach (var Item in await Context.Channel.GetMessagesAsync(1000).Flatten())
            {
                if (count > 0)
                {
                    if (Item.Attachments.Count > 0)
                    {
                        var file = Item.Attachments.FirstOrDefault();
                        images.Add(file.Url);
                        count--;
                    }
                    else if (Item.Content.Contains("http://"))
                    {
                        images.Add(Item.Content);
                        count--;
                    }
                }
                else
                    break;
            }
            for(int i = images.Count-1; i >= 0; i--)
            {
                await Context.User.SendMessageAsync(images[i]);
                Console.WriteLine(images[i]);
            }
            Console.WriteLine("Finshed Sending Gallery");
        }
    }
}
