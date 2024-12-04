using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PROG2370_FinalProject_EricaGoodman
{
    internal class Maze
    {
        public int version;
        private readonly string[] solution = { "DDDRURDRUULLURR", "RDRURDDDLULDLUU", "DDDRRRULLUURDRU", "RRRDDDLLLUURRDL", "DDRRULURRDDDLLL" };

        public bool solved = false;
        public Vector2 blockPosition = new Vector2(112, 112); // hold the position of the block within the maze (starts in top left corner)
        public Vector2 position = new Vector2(100, 100); // holds the maze trigger position
        private bool leftReleased = true;
        private bool rightReleased = true;
        private bool upReleased = true;
        private bool downReleased = true;
        int speed = 72; // a fixed int to move the block that distance (based on scale of maze) (300x300 maze = 72)
        char[] charSolution;
        //public int mazeScore = 0;

        static public int radius = 60;



        public Maze(int version)
        {
            this.version = version;
            this.charSolution = solution[version].ToCharArray();
        }

        public int UpdatePosition(int score)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) && leftReleased)
            {
                if (charSolution.First() == 'L')
                {
                    this.blockPosition.X -= speed;
                    this.charSolution = this.charSolution.Skip(1).ToArray();
                }
                else
                { // put the block back at the start of maze, remove points, and reset solution array
                    score -= 1;
                    this.blockPosition = new Vector2(112, 112);
                    this.charSolution = solution[version].ToCharArray();
                }
                leftReleased = false;
            }
            if (state.IsKeyDown(Keys.Right) && rightReleased)
            {
                if (charSolution.First() == 'R')
                {
                    this.blockPosition.X += speed;
                    this.charSolution = this.charSolution.Skip(1).ToArray();
                }
                else
                { // put the block back at the start of maze, remove points, and reset solution array
                    score -= 1;
                    this.blockPosition = new Vector2(112, 112);
                    this.charSolution = solution[version].ToCharArray();
                }
                rightReleased = false;
            }

            if (state.IsKeyDown(Keys.Up) && upReleased)
            {
                if (charSolution.First() == 'U')
                {
                    this.blockPosition.Y -= speed;
                    this.charSolution = this.charSolution.Skip(1).ToArray();
                }
                else
                { // put the block back at the start of maze, remove points, and reset solution array
                    score -= 1;
                    this.blockPosition = new Vector2(112, 112);
                    this.charSolution = solution[version].ToCharArray();
                }
                upReleased = false;
            }
            if (state.IsKeyDown(Keys.Down) && downReleased)
            {
                if (charSolution.First() == 'D')
                {
                    this.blockPosition.Y += speed;
                    this.charSolution = this.charSolution.Skip(1).ToArray();
                }
                else
                { // put the block back at the start of maze, remove points, and reset solution array
                    score -= 1;
                    this.blockPosition = new Vector2(112, 112);
                    this.charSolution = solution[version].ToCharArray();
                }
                downReleased = false;
            }

            // mark each key as being released
            if (state.IsKeyUp(Keys.Left))
            {
                leftReleased = true;
            }
            if (state.IsKeyUp(Keys.Right))
            {
                rightReleased = true;
            }
            if (state.IsKeyUp(Keys.Up))
            {
                upReleased = true;
            }
            if (state.IsKeyUp(Keys.Down))
            {
                downReleased = true;
            }

            if(this.charSolution.Length <= 0) 
            {
                this.solved = true;
                score += 10;
            }

            return score;

        }

        internal void StartMaze()
        {
            // set all keys to pressed, to prevent auto fail maze upon start
            leftReleased = false;
            rightReleased = false;
            upReleased = false;
            downReleased = false;
        }
    }
}
