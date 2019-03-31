using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace UsefulDiscordBot.Modules
{
		public class Foo : ModuleBase<SocketCommandContext>
		{
				private static string testString;

				public static string test {
						get
						{
								return testString;
						}
						set
						{
								testString = value;
						}
				}

				[Command("initializeString")]
				public async Task initializeString()
				{
						test = "Initialized";
				}

				[Command("addToString")]
				public async Task addToString([Remainder]string s)
				{
						test = test + s;
				}

				[Command("printString")]
				public async Task printString()
				{
						await ReplyAsync(test);
				}

				[Command("react")]
				public async Task react()
				{
						var msg = await ReplyAsync("testReact");
						var emoji = new Emoji("❓");
						await msg.AddReactionAsync(emoji);
				}
		}
}
