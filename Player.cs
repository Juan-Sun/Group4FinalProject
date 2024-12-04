using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG2370_FinalProject_EricaGoodman
{
    internal class Player
    {
        // add variable to hold direction? (for direction facing sprites)
        public Rectangle hitbox = new Rectangle(100, 100, 30, 30); // rectangle works better for hitboxes
        int speed = 3; //adjust speed later to work well with enemy speed

        public void UpdatePlayer(Controller controller, List<Rectangle> walls)
        { 
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                this.hitbox.X -= speed;
                foreach(var wall in walls) // check wall collision
                {
                    if (controller.DidCollide(hitbox, wall))
                    { //undo movement
                        this.hitbox.X += speed;
                        break; // don't need to check more walls, because we already found the wall we've hit (optimization)
                    }
                }
            }
            if (state.IsKeyDown(Keys.Right))
            {
                this.hitbox.X += speed;
                foreach (var wall in walls) // check wall collision
                {
                    if (controller.DidCollide(hitbox, wall))
                    { //undo movement
                        this.hitbox.X -= speed;
                        break;
                    }
                }
            }

            if (state.IsKeyDown(Keys.Up))
            {
                this.hitbox.Y -= speed;
                foreach (var wall in walls) // check wall collision
                {
                    if (controller.DidCollide(hitbox, wall))
                    { //undo movement
                        this.hitbox.Y += speed;
                        break;
                    }
                }
            }
            if (state.IsKeyDown(Keys.Down))
            {
                this.hitbox.Y += speed;
                foreach (var wall in walls) // check wall collision
                {
                    if (controller.DidCollide(hitbox, wall))
                    { //undo movement
                        this.hitbox.Y -= speed;
                        break;
                    }
                }
            }
        }

        public void RespawnPlayer()
        {
            // move player back to spawn position
            hitbox.X = 100;
            hitbox.Y = 100;
        }

    }
}
