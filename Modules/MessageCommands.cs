using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using Discord.Commands;
using Discord;

namespace UsefulDiscordBot.Modules
{
		[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
		[RequireOwner(Group = "Permission")]
		[Group("clear")]
    public class MessageCommands : ModuleBase<SocketCommandContext>
    {
				bool isAdmin => (Context.User as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Mods") != null;

				[Command("from")]
        public async Task deleteFrom([Remainder] string user = "")
        {
						if (isAdmin)
						{
								//await checkPermissions();
								Console.WriteLine("Deleting messages from" + user);
								if (user == "")
								{
										await Context.Channel.SendMessageAsync("`You need to specify the user | !clear \"user\" | Replace \"user\" with anyone`");
								}

								int Amount = 0;
								foreach (var Item in await Context.Channel.GetMessagesAsync(1000).Flatten())
								{
										if (Item.Author.Username == user)
										{
												Amount++;
												await Item.DeleteAsync();
										}

								}
								await ReplyAsync($"`{Context.User.Username} deleted {Amount} messages`");
						}
						await ReplyAsync("You must be a moderator to delete messages");
        }

        [Command()]
        public async Task clear([Remainder] int Delete = 0)
        {
            //await checkPermissions();
            if (Delete == null)
            {
                await Context.Channel.SendMessageAsync("`You need to specify the amount | !clear (amount) | Replace (amount) with anything`");
            }
            int Amount = 0;

            Console.WriteLine("Deleting all Messages");
            foreach (var Item in await Context.Channel.GetMessagesAsync(Delete + 1).Flatten())
            {

                Amount++;
                await Item.DeleteAsync();

            }
            await Context.Channel.SendMessageAsync($"`{Context.User.Username} deleted {Amount} messages`");
        }

        [Command("containing")]
        public async Task delete([Remainder] string key = "")
        {
            //await checkPermissions();
            if (key == "")
            {
                await Context.Channel.SendMessageAsync("`You need to specify the key | !clear \"key\" | Replace \"key\" with any string");
            }

            int Amount = 0;

            Console.WriteLine("Deleting Messages");
            foreach (var Item in await Context.Channel.GetMessagesAsync(1000).Flatten())
            {
                if (Item.Content.Contains(key))
                {
                    Amount++;
                    await Item.DeleteAsync();
                }

            }
            await Context.Channel.SendMessageAsync($"`{Context.User.Username} deleted {Amount} messages`");
        }

        public async Task checkPermissions()
        {
            Console.WriteLine("Checking permissions");
            IGuildUser Bot = Context.Guild.GetUser(Context.User.Id);
            if (!Bot.GetPermissions(Context.Channel as ITextChannel).ManageMessages)
            {
                await Context.Channel.SendMessageAsync("`Bot does not have enough permissions to manage messages`");
                return;
            }
            await Context.Message.DeleteAsync();
            var GuildUser = Context.Guild.GetUser(Context.User.Id);
            if (!GuildUser.GetPermissions(Context.Channel as ITextChannel).ManageMessages)
            {
                await Context.Channel.SendMessageAsync("`You do not have enough permissions to manage messages`");
                return;
            }
            
        }
    }

}