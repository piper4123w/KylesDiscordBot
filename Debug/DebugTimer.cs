using System;
using System.Diagnostics;

namespace UsefulDiscordBot.Debug
{
		public class DebugTimer : Stopwatch
		{
				public DebugTimer()
				{
						Start();
				}

				public void StopTimer()
				{
						Stop();
						Console.WriteLine(Elapsed.ToString());
				}
		}
}
