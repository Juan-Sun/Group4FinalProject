using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace final_projectM
{
    internal class Health_System
    {

        //replace player with proper player property
        public bool didCollisionHappen(Player p, Enemies e)
        {
            int playerRadius = p.getRadius();
            int eRadius = Enemies.radius;
            int distance = playerRadius + eRadius;
            if (Vector2.Distance(p.position, e.position) < distance)
            {
                return true;
            }

            return false;
        }
    }
}