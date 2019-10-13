using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulDiscordBot.Modules.MessageFormatting;

namespace UsefulDiscordBot.Modules.Decision
{
    [Group("decide")]
    public class DecisionCommands : ModuleBase<SocketCommandContext>
    {
        DecisionEmbed embed;
        public string ContentTag = "Decision";

        [Command]
        public async Task SetUpDecision([Remainder] string optionsString) => await SetUpDecision(optionsString, Context.Message);

        [Command("CSMap")]
        [Priority(1)]
        public async Task CSMapDecision() => await SetUpDecision("Cache Mirage Overpass Inferno Train Nuke Dust2 Vertigo", Context.Message);

        public async Task SetUpDecision(string optionsString, IUserMessage message)
        {
            embed = new DecisionEmbed(message.Author, ContentTag);
            embed.Description = "Select a reaction emoji to proceed";
            var options = optionsString.Split(' ').ToList();
            embed.EmbedOpitons("Options", options);
            var msg = await message.Channel.SendMessageAsync(ContentTag, false, embed);
            var choiceEmojis = new ChoiceEmojis().GetNumberOfChoices(options.Count);
            foreach (var e in choiceEmojis)
            {
                await msg.AddReactionAsync(e);
            }
        }
    }
}
