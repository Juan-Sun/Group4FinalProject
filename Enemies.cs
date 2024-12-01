using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace final_projectM
{
    internal class Enemies
    {
        public Vector2 position;
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
