using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group4FinalProject
{
	internal class Controller
	{
		private TimeSpan elapsedTime;
		private int secondsElapsed;

	 public Controller()
		{
			elapsedTime = TimeSpan.Zero;
			secondsElapsed = 0;
		}

		public int updateTime(GameTime gameTime)
		{
			elapsedTime += gameTime.ElapsedGameTime;

			if (elapsedTime.TotalSeconds >= 1)
			{
				secondsElapsed++;
				elapsedTime = TimeSpan.Zero;
			}

			return secondsElapsed;
		}
	}
}
