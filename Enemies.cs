using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Group4FinalProject
{
	internal class Enemies
	{
		public Rectangle position = new Rectangle(300,400,38,71);
		public int speed;
		static public int radius;


		public Enemies(int speed)
		{
			this.speed = speed;
		}

		public void updateEnemies()
		{
			this.position.X -= this.speed;
		}
	}
}
