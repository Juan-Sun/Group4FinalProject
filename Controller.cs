using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG2370_FinalProject_EricaGoodman
{
    internal class Controller
    {
        // collision player with all objects
        public bool DidCollide(Rectangle player, Rectangle hitbox)
        {
            if (player.Intersects(hitbox))
                {   return true;    }
            else
                {   return false;   }
        }
    }
}
