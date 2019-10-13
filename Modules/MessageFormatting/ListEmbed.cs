using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;

namespace UsefulDiscordBot.Modules.MessageFormatting
{
    public class ListEmbed : EmbedBuilder
    {
        static Color color => new Color(8816262);

        public ListEmbed(string title)
        {
            Title = title;
            Color = color;
        }

        public void EmbedList<T>(string title, List<T> list)
        {
            if (list?.Count > 0)
            {
                string output = "";
                foreach (var item in list)
                {
                    output += item + "\n";
                }
                AddField(title, output);
            }
        }
    }
}
