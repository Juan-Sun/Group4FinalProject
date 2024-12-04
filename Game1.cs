using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PROG2370_FinalProject_EricaGoodman
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Controller controller = new Controller();

        Texture2D mazeDesign;
        Texture2D mazeBlock;
        Texture2D mazeTrigger; // ### REPLACE minigameobject/trigger texture

        Texture2D playerSprite; // ### REPLACE player sprite (will add animations later?)
        Texture2D enemySprite; // ### REMOVE enemy texture

        Texture2D wallSprite; // ### REPLACE wall texture


        Player player;
        Maze newMaze;

        // collision boxes
        // use a dictionary?
        Rectangle mazeRect; // ### ADD more for each puzzle
        Rectangle enemyRect;
        Rectangle exitRect;
        Rectangle startButton = new Rectangle(150, 200, 200, 100);
        List<Rectangle> wallRect = new List<Rectangle>();

        string mazeFile = "Maze_"; // used to select the correct maze design sprite
        bool inMaze = false;

        bool gamerunning = true;
        bool startMenu = true;
        MouseState mouseState;

        public int score = 10; // ### REMOVE score
        SpriteFont spriteFont; // ### REMOVE score

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 500; // ### CHANGE screen size
            _graphics.PreferredBackBufferHeight = 500; // ### CHANGE screen size
            _graphics.ApplyChanges();

            player = new Player();

            // randomly pick a maze design
            Random rand = new Random();
            int random = rand.Next(0, 4);
            mazeFile = mazeFile + random;
            newMaze = new Maze(random);

            // set the spawn locations of objects
            mazeRect = new Rectangle(300, 200, 20, 20);
            // ### ADD other puzzles & Exit & Enemies
            enemyRect = new Rectangle(100, 350, 40, 40);
            exitRect = new Rectangle(400, 400, 20, 20);

            // set walls
            // ### CHANGE depending on level design
            wallRect.Add(new Rectangle(0, 0, 4900, 10));
            wallRect.Add(new Rectangle(0, 10, 10, 490));
            wallRect.Add(new Rectangle(10, 490, 490, 10));
            wallRect.Add(new Rectangle(490, 0, 10, 490));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mazeDesign = Content.Load<Texture2D>(mazeFile);
            mazeBlock = Content.Load<Texture2D>("Maze-block");
            // ### CHANGE Textures
            playerSprite = Content.Load<Texture2D>("white");
            wallSprite = Content.Load<Texture2D>("white");
            mazeTrigger = Content.Load<Texture2D>("white");
            enemySprite = Content.Load<Texture2D>("white");

            spriteFont = Content.Load<SpriteFont>("galleryFont"); // ### REMOVE score
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (startMenu)
            {
                mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mouseState.Position.X < startButton.Right && mouseState.Position.X > startButton.Left &&
                            mouseState.Position.Y < startButton.Bottom && mouseState.Position.Y > startButton.Top)
                    {   startMenu = false;  }
                }
                base.Update(gameTime);
                return; // exit the Update method
            }

            if (inMaze)
            {
                score = newMaze.UpdatePosition(score); // ### REPLACE? score/stats

                if (newMaze.solved)
                    {   inMaze = false; }
                base.Update(gameTime);
                return; // exit the Update method
            }


            // move player & check wall colisions
            player.UpdatePlayer(controller, wallRect);

            // Check enemy collisions:
            if (controller.DidCollide(player.hitbox, enemyRect))
            {
                // ### ADD health decrease
                player.RespawnPlayer();
                score -= 5;// ### REPLACE score

                // ### MOVE this logic to controller or Enemy?
            }

            // check puzzle trigger collisions
            if (controller.DidCollide(player.hitbox, mazeRect) && !newMaze.solved)
            {
                newMaze.StartMaze();
                inMaze = true;
            }
            // ### ADD other puzzle/mini-game triggers & logic

            // check exit door collision
            if (controller.DidCollide(player.hitbox, exitRect) && newMaze.solved)
            {
                // ### ADD gameEnd/exit logic
                gamerunning = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (startMenu) // !!! Work In Progress
            {
                GraphicsDevice.Clear(Color.Tan);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(spriteFont, "Temple Explorer", new Vector2(140, 100), Color.Black);
                _spriteBatch.Draw(wallSprite, startButton, Color.LimeGreen);
                _spriteBatch.DrawString(spriteFont, "S T A R T", new Vector2(180, 230), Color.Black);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (!gamerunning) // !!! Work In Progress
            {
                GraphicsDevice.Clear(Color.LightGreen);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(spriteFont, "You Win!", new Vector2(200, 100), Color.Black);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else
            {
                GraphicsDevice.Clear(Color.Wheat);

                _spriteBatch.Begin(); // ### CHANGE all draw colours to white

                if (inMaze)
                {
                    _spriteBatch.Draw(mazeDesign, new Vector2(newMaze.position.X, newMaze.position.Y), Color.White);
                    _spriteBatch.Draw(mazeBlock, new Vector2(newMaze.blockPosition.X, newMaze.blockPosition.Y), Color.White);
                }
                else
                {
                    if (!newMaze.solved)
                    { _spriteBatch.Draw(mazeTrigger, mazeRect, Color.MediumTurquoise); }
                    else
                    { _spriteBatch.Draw(mazeTrigger, mazeRect, Color.LawnGreen); }

                    foreach (var rect in wallRect)
                    { _spriteBatch.Draw(wallSprite, rect, Color.Sienna); }

                    _spriteBatch.Draw(wallSprite, exitRect, Color.Violet); // ### CHANGE placeholder sprite

                    _spriteBatch.Draw(enemySprite, enemyRect, Color.DarkRed); // ### REMOVE placeholder enemy

                    _spriteBatch.Draw(playerSprite, player.hitbox, Color.LightSlateGray);
                }

                _spriteBatch.DrawString(spriteFont, "Score: " + score, new Vector2(20, 20), Color.BlueViolet);

                _spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }

}
