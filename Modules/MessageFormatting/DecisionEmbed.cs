using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;

namespace UsefulDiscordBot.Modules.MessageFormatting
{
    public class DecisionEmbed : EmbedBuilder
    {
        static Color color => new Color(8816262);

        public DecisionEmbed(IUser user, string title)
        {
            Author = new EmbedAuthorBuilder
            {
                Name = user.Username,
                IconUrl = user.GetAvatarUrl()
            };
            Title = "Setting up a Decision";
            Color = color;
        }

        public DecisionEmbed(SocketCommandContext context, string title) : this(context.User, title)
        {
        }

        public void EmbedOpitons<T>(string listTitle, List<T> list)
        {
            if (list?.Count > 0)
            {
                string output = "";
                int i = 0;
                foreach (var item in list)
                {
                    output += ChoiceEmojis.All[i++] + " " + item + "\n";
                }
                AddField(listTitle, output);
            }
        }

        internal static EmbedBuilder ChoiceMade(string decision)
        {
            var toReturn = new EmbedBuilder();
            toReturn.Title = "Decision made";
            toReturn.Color = color;
            toReturn.Description = decision;
            return toReturn;
        }
    }
}
