namespace UsefulDiscordBot
{
		public class ReactionHandler : System.Attribute
		{
				private string contentTag;
				public ReactionHandler(string _contentTag)
				{
						contentTag = _contentTag;
				}
		}
}
