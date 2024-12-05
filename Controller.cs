using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group4FinalProject
{
	internal class Controller
	{
		// collision player with all objects
		public bool DidCollide(Rectangle player, Rectangle hitbox)
		{
			if (player.Intersects(hitbox))
			{ return true; }
			else
			{ return false; }
		}

        public List<Texture2D> SetCurrentEnemyAnimation(string enemyDirection, List<Texture2D> enemyUpFrames, List<Texture2D> enemyDownFrames, List<Texture2D> enemyRightFrames, List<Texture2D> enemyLeftFrames)
        {
            switch (enemyDirection)
            {
                case "up":
                    return enemyUpFrames;
                case "down":
                    return enemyDownFrames;
                case "right":
                    return enemyRightFrames;
                case "left":
                    return enemyLeftFrames;

                default:
                    return enemyUpFrames;

            }
        }
    }
}