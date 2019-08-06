using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Logic
{
	public class LogicController
	{
		public static GameTime Time = new GameTime();
		public static TimeSpan PauseTime { get; private set; }
		private static TimeSpan timeOfPause;

		public static bool Paused { get { return paused; } }
		private static bool paused = false;
		private static bool unPause;

		public static double CurrentMillisecond { get { return Time.TotalGameTime.TotalMilliseconds; }}
		public static float TimeDelta;

		public static int NextID { get { return nextID++;} }
		private static int nextID = 0;
        
		
		public static void Update(GameTime time)
		{
			
			if(unPause)
			{
				TimeSpan test = (time.TotalGameTime - timeOfPause);
				Console.WriteLine(test);

				PauseTime = PauseTime.Add(test);

				paused = false;
				unPause = false;
			}

			if(!paused)
			{
				Time = new GameTime(time.TotalGameTime - PauseTime, time.ElapsedGameTime);
				TimeDelta = (float)time.ElapsedGameTime.Milliseconds / 1000;
			}
			else
			{
				if(TimeDelta != 0)
				{
					timeOfPause = time.TotalGameTime;
					Console.WriteLine(timeOfPause);
					TimeDelta = 0;
				}
				
			}
				

			//Console.WriteLine("PausableTime: " + Time.TotalGameTime.TotalMilliseconds + " Time: " + time.TotalGameTime.TotalMilliseconds);

		}

		public static void Pause()
		{
			paused = true;
		}

		public static void Continue()
		{
			if(paused)
				unPause = true;
		}
	}
}
