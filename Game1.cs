using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Group4FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont timerFont;

        Texture2D skillCheckTexture, needleTexture, successZoneTexture; //Background image of the skill check and needle

        SoundEffect explosionSound, StartSkillCheckSound, SuccessSkillCheck;
        Song beginingMusic;
        Song endingMusic;

        Random skillRandom;

        int score = 10;
        int radius = 0;
        float needleRotation; //Rotation angle of the needle
        float needleSpeed = 2f; //Speed of the needle

        bool skillCheckActive; //Check to see if the skill check is active
        bool resultSuccess; //Result of the skill check

        private TimeSpan elapsedTime;
        private int secondsElapsed;

        //memory game stuff
        List<int> Sequence = new List<int>();

        Random runeRandom = new Random();
        int flashingIndex = -1;

        List<Vector2> runePositions = new List<Vector2>();
        int playerStep = 0;
        //bool gameOver = false;

        MouseState currentMouse, PreviousMouse;

        private const int MaxSequenceLength = 4;//maximum sequence before the game ends
        private bool RuneGameWon = false;
        private float runeFlashDelay = 0.5f;
        private float flashTimer = 0f;
        private int currentFlashingRune = 0;
        private bool flashingRunes = true;
        private bool waitingForClick = false;
        public bool inMemoryGame = false;

        string mazeWallFile = "MazeWall-"; //used to select the correect maze design sprite
        string mazeFloorFile = "MazeFloor-"; //used to select the correct maze design sprite
        bool inMaze = false;

        bool gamerunning = true;
        bool startMenu = true;
        MouseState mouseState;

        public Color backColor = new Color(1, 1, 1);

        //health system stuff
        public bool invincibilityFrames = false;
        public bool inGame = true;
        float invincibilityTimer = 0f;

        //enemy stuff
        Health_System health = new Health_System();
        public bool enemyGoingLeft = false;
        public bool enemyGoingRight = false;
        public bool enemyGoingUp = true;
        public bool enemyGoingDown = false;

        float enemyAnimationTimer = 0f;
        int currentEnemyFrame = 0;
        float enemyFrameDuration = 0.2f;
        List<Texture2D> currentEnemyAnimation;


        Controller controller = new Controller();
        Player player;
        Maze newMaze;
        //Credits credits = new Credits();

        // collision boxes
        Rectangle mazeRect;
        Rectangle runeRect;
        Rectangle skillRect;
        Rectangle exitRect;
        Rectangle startButton = new Rectangle(140, 200, 200, 100);
        List<Rectangle> wallRect = new List<Rectangle>();

        Texture2D playerSprite; // ### REPLACE player sprite (will add animations later?)
        Texture2D wallSprite;
        Texture2D exitSprite;
        Texture2D puzzleSprite; // all puzzles/mini-games will have the same trigger box texture to add mystery
        Texture2D mazeWall;
        Texture2D mazeFloor;
        Texture2D mazeBlock;
        Texture2D menuSprite;

        // Player Animation Code is referenced from "Industrian.net" tutorial
        float animTimer; // A timer that stores milliseconds
        int timerThreshold; // An int that is the threshold for the timer
        Rectangle[] playerRightRect = new Rectangle[8];// Rectangle arrays that store sourceRectangles for animations
        // These bytes tell the spriteBatch.Draw() what sourceRectangle to display
        byte currentAnimationIndex;


        //enemy going up on the screen
        List<Texture2D> enemyUpFrames;
        Texture2D EnemySpritef1;
        Texture2D EnemySpritef2;
        Texture2D EnemySpritef3;
        Texture2D EnemySpritef4;
        //enemy going down on the screen
        List<Texture2D> enemyDownFrames;
        Texture2D EnemySpriteb1;
        Texture2D EnemySpriteb2;
        Texture2D EnemySpriteb3;
        Texture2D EnemySpriteb4;
        //enemy going right on the screen
        List<Texture2D> enemyRightFrames;
        Texture2D EnemySpriter1;
        Texture2D EnemySpriter2;
        Texture2D EnemySpriter3;
        Texture2D EnemySpriter4;
        //enemy going left on the screen
        List<Texture2D> enemyLeftFrames;
        Texture2D EnemySpritel1;
        Texture2D EnemySpritel2;
        Texture2D EnemySpritel3;
        Texture2D EnemySpritel4;

        //non current rune
        List<Texture2D> RuneSpriteOff;
        Texture2D RuneSpriteFL;
        Texture2D RuneSpriteFC;
        Texture2D RuneSpriteFA;
        Texture2D RuneSpriteFI;
        Texture2D RuneSpriteFD;
        Texture2D RuneSpriteFM;
        //current rune
        List<Texture2D> RuneSpriteOn;
        Texture2D RuneSpriteNL;
        Texture2D RuneSpriteNC;
        Texture2D RuneSpriteNM;
        Texture2D RuneSpriteNA;
        Texture2D RuneSpriteNI;
        Texture2D RuneSpriteND;

        //enemy speed set to 2
        Enemies enemy = new Enemies(2);
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 500;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            elapsedTime = TimeSpan.Zero;
            secondsElapsed = 0;

            skillRandom = new Random();

            player = new Player();

            // randomly pick a maze design
            Random rand = new Random();
            int random = rand.Next(0, 4);
            mazeWallFile = mazeWallFile + random;
            mazeFloorFile = mazeFloorFile + random;
            //mazeFile = mazeFile + random;
            newMaze = new Maze(random);

            // set the spawn locations of objects
            mazeRect = new Rectangle(150, 250, 20, 20);
            runeRect = new Rectangle(250, 250, 20, 20);
            skillRect = new Rectangle(350, 250, 20, 20);
            exitRect = new Rectangle(400, 400, 30, 30);

            // set walls
            // ### CHANGE depending on level design
            wallRect.Add(new Rectangle(0, 0, 4900, 10));
            wallRect.Add(new Rectangle(0, 10, 10, 490));
            wallRect.Add(new Rectangle(10, 490, 490, 10));
            wallRect.Add(new Rectangle(490, 0, 10, 490));


            //position of all the runes for memory game can be changed
            runePositions.Add(new Vector2(50, 100));
            runePositions.Add(new Vector2(125, 100));
            runePositions.Add(new Vector2(200, 100));
            runePositions.Add(new Vector2(275, 100));
            runePositions.Add(new Vector2(350, 100));
            runePositions.Add(new Vector2(425, 100));

            //if (MediaPlayer.State != MediaState.Playing)
            //{
            //    MediaPlayer.IsRepeating = true;
            //    MediaPlayer.Play(beginingMusic);
            //}
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // all sprites are in order of animation

            // TODO: use this.Content to load your game content here
            mazeWall = Content.Load<Texture2D>(mazeWallFile);
            mazeFloor = Content.Load<Texture2D>(mazeFloorFile);
            mazeBlock = Content.Load<Texture2D>("Maze-Block");
            playerSprite = Content.Load<Texture2D>("npc01_spritesheet");
            wallSprite = Content.Load<Texture2D>("StoneWallTexture");
            puzzleSprite = Content.Load<Texture2D>("puzzleTrigger");
            menuSprite = Content.Load<Texture2D>("StartMenuIMG");

            timerFont = Content.Load<SpriteFont>("galleryFont");

            animTimer = 0;
            timerThreshold = 100; //lower number = faster animation

            for (int w = 0, i = 0; i < 8; w += 30, i++)
            {
                playerRightRect[i] = new Rectangle(w, 0, 30, 48);
            }

            currentAnimationIndex = 1;

            exitSprite = Content.Load<Texture2D>("white"); // ### REPLACE exit door sprite

            timerFont = Content.Load<SpriteFont>("galleryFont");

            //enemy going up on the screen
            EnemySpritef1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f");
            EnemySpritef2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f1");
            EnemySpritef3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f2");
            EnemySpritef4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f3");
            enemyUpFrames = new List<Texture2D> { EnemySpritef1, EnemySpritef2, EnemySpritef3, EnemySpritef4 };
            //enemy going down on the screen
            EnemySpriteb1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh02");
            EnemySpriteb2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0");
            EnemySpriteb3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh03");
            EnemySpriteb4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh04");
            enemyDownFrames = new List<Texture2D> { EnemySpriteb1, EnemySpriteb2, EnemySpriteb3, EnemySpriteb4 };
            //enemy going right on the screen
            EnemySpriter1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r");
            EnemySpriter2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r1");
            EnemySpriter3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r2");
            EnemySpriter4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r3");
            enemyRightFrames = new List<Texture2D> { EnemySpriter1, EnemySpriter2, EnemySpriter3, EnemySpriter4 };
            //enemy going left on the screen
            EnemySpritel1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l1");
            EnemySpritel2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l");
            EnemySpritel3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l2");
            EnemySpritel4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l3");
            enemyLeftFrames = new List<Texture2D> { EnemySpritel1, EnemySpritel2, EnemySpritel3, EnemySpritel4 };

            //non current rune
            RuneSpriteFA = Content.Load<Texture2D>("runesOff-0");
            RuneSpriteFC = Content.Load<Texture2D>("runesOff-1");
            RuneSpriteFD = Content.Load<Texture2D>("runesOff-2");
            RuneSpriteFI = Content.Load<Texture2D>("runesOff-3");
            RuneSpriteFL = Content.Load<Texture2D>("runesOff-4");
            RuneSpriteFM = Content.Load<Texture2D>("runesOff-5");
            RuneSpriteOff = new List<Texture2D>
            {
                RuneSpriteFA, RuneSpriteFC,RuneSpriteFD,RuneSpriteFI,RuneSpriteFL,RuneSpriteFM
            };

            //current rune
            RuneSpriteNA = Content.Load<Texture2D>("runesOn-0");
            RuneSpriteNC = Content.Load<Texture2D>("runesOn-1");
            RuneSpriteND = Content.Load<Texture2D>("runesOn-2");
            RuneSpriteNI = Content.Load<Texture2D>("runesOn-3");
            RuneSpriteNL = Content.Load<Texture2D>("runesOn-4");
            RuneSpriteNM = Content.Load<Texture2D>("runesOn-5");
            RuneSpriteOn = new List<Texture2D>
            {
                RuneSpriteNA, RuneSpriteNC,RuneSpriteND,RuneSpriteNI,RuneSpriteNL,RuneSpriteNM
            };


            skillCheckTexture = Content.Load<Texture2D>("Circle");
            needleTexture = Content.Load<Texture2D>("needle");
            successZoneTexture = Content.Load<Texture2D>("good");

            timerFont = Content.Load<SpriteFont>("timerFont");
            explosionSound = Content.Load<SoundEffect>("sfx_generator_explode_01");
            StartSkillCheckSound = Content.Load<SoundEffect>("sfx_hud_skillcheck_open_04");
            SuccessSkillCheck = Content.Load<SoundEffect>("sfx_hud_skillcheck_open_04");
            //beginingMusic = Content.Load<Song>("D&D Menu Survivor");
            //endingMusic = Content.Load<Song>("Alan Wake Menu");

        }
        private void StartNewSequence()
        {
            if (Sequence.Count >= MaxSequenceLength)
            {
                RuneGameWon = true;
                inMemoryGame = false;
                return;
            }
            playerStep = 0;
            Sequence.Add(runeRandom.Next(0, RuneSpriteOff.Count));
            flashingIndex = Sequence[0];

            flashingRunes = true;
            currentFlashingRune = 0;
            flashTimer = 0f;
            waitingForClick = false;
        }
        void StartSkillCheck()
        {
            StartSkillCheckSound.Play();

            skillCheckActive = true;
            resultSuccess = false;
            needleRotation = 0f;
        }

        public string enemyDirection;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime.TotalSeconds >= 1)
            {
                secondsElapsed++;
                elapsedTime = TimeSpan.Zero;
            }

            if (skillCheckActive)
            {

                needleRotation += needleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (needleRotation >= MathHelper.TwoPi) // Reset when completing a circle
                {
                    needleRotation = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space)) //Check to see if the user input space on the skill check
                {
                    float needleAngle = needleRotation % MathHelper.TwoPi;
                    //MathHelper.ToDegrees(needleRotation);
                    //float successZoneEndAngle = successZoneStartAngle + successZoneArc; //Change entire if statement to  fix collsion //INPORTANT!!!!

                    //if (needleAngle >= successZoneStartAngle && needleAngle <= successZoneEndAngle)
                    //{
                    //    SuccessSkillCheck.Play();

                    //    resultSuccess = true; //Successful skill check
                    //    skillCheckActive = false;
                    //}
                    //else
                    //{
                        explosionSound.Play();

                    //    //resultSuccess = false; //Failed skill check
                    //}
                    //skillCheckActive = false; // Ends the skill check

                    if ((Keyboard.GetState().IsKeyDown(Keys.P)))
                    {
                        SuccessSkillCheck.Play();
                        score += 10;
                        resultSuccess = true; //Successful skill check
                        skillCheckActive = false;
                    }
                }
            }

            currentEnemyAnimation = controller.SetCurrentEnemyAnimation(enemyDirection, enemyUpFrames, enemyDownFrames, enemyRightFrames, enemyLeftFrames);

            enemyAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemyAnimationTimer >= enemyFrameDuration && gamerunning && !startMenu)
            {
                currentEnemyFrame = (currentEnemyFrame + 1) % currentEnemyAnimation.Count;
                enemyAnimationTimer = 0f;
            }

            if (controller.DidCollide(player.hitbox, enemy.position) && !invincibilityFrames && gamerunning && !startMenu)
            {
                invincibilityFrames = true;
                player.health--;
                score -= 5;
                // player.RespawnPlayer(); // don't think we need to respawn player, because max added invincibility frames
                invincibilityTimer = 0f;
            }
            if (invincibilityFrames)
            {
                // Increment the timer based on elapsed game time
                invincibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Deactivate invincibility after 1 second
                if (invincibilityTimer >= 1f)
                {
                    invincibilityFrames = false;
                    invincibilityTimer = 0f;
                }
            }
            if (enemyGoingDown)
            {
                enemy.position.Y += enemy.speed;
                enemyDirection = "down";
                foreach (var wall in wallRect) // check wall collision
                {
                    if (controller.DidCollide(enemy.position, wall))
                    { //undo movement
                        enemy.position.Y -= enemy.speed;
                        enemyGoingDown = false;
                        enemyGoingLeft = true;
                        break;
                    }
                }
            }
            if (enemyGoingLeft)
            {
                enemy.position.X -= enemy.speed;
                enemyDirection = "left";
                foreach (var wall in wallRect) // check wall collision
                {
                    if (controller.DidCollide(enemy.position, wall))
                    { //undo movement
                        enemy.position.X += enemy.speed;
                        enemyGoingLeft = false;
                        enemyGoingUp = true;
                        break;
                    }
                }
            }
            if (enemyGoingRight)
            {
                enemy.position.X += enemy.speed;
                enemyDirection = "right";
                foreach (var wall in wallRect) // check wall collision
                {
                    if (controller.DidCollide(enemy.position, wall))
                    { //undo movement
                        enemy.position.X -= enemy.speed;
                        enemyGoingRight = false;
                        enemyGoingDown = true;
                        break;
                    }
                }
            }
            if (enemyGoingUp)
            {
                enemy.position.Y -= enemy.speed;
                enemyDirection = "up";
                foreach (var wall in wallRect) // check wall collision
                {
                    if (controller.DidCollide(enemy.position, wall))
                    { //undo movement
                        enemy.position.Y += enemy.speed;
                        enemyGoingUp = false;
                        enemyGoingRight = true;
                        break;
                    }
                }
            }

            if (inMemoryGame)
            {

                currentMouse = Mouse.GetState();
                //SequenceTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Flashing runes phase
                if (flashingRunes)
                {
                    flashTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (flashTimer >= runeFlashDelay)
                    {
                        flashTimer = 0f;
                        currentFlashingRune++;

                        // Update the flashingIndex to the correct rune in the sequence
                        if (currentFlashingRune < Sequence.Count)
                        {
                            flashingIndex = Sequence[currentFlashingRune]; // Show next rune in the sequence
                        }

                        // If all runes have flashed, stop flashing and wait for the player's click
                        if (currentFlashingRune >= Sequence.Count)
                        {
                            flashingRunes = false;
                            waitingForClick = true; // Now the player can click
                            currentFlashingRune = 0;
                            flashingIndex = -1; // Reset flashing index for player's turn
                        }
                    }
                }

                // Handle player input during the waiting phase
                if (waitingForClick)
                {
                    if (currentMouse.LeftButton == ButtonState.Pressed && PreviousMouse.LeftButton == ButtonState.Released)
                    {
                        // Check if the player clicked inside the bounds of a rune
                        for (int i = 0; i < runePositions.Count; i++)
                        {
                            Rectangle runeRect = new Rectangle(
                                (int)runePositions[i].X,
                                (int)runePositions[i].Y,
                                RuneSpriteOff[i].Width,
                                RuneSpriteOff[i].Height
                            );

                            if (runeRect.Contains(currentMouse.Position))
                            {
                                // Check if the clicked rune matches the expected rune in the sequence
                                if (i == Sequence[playerStep]) // Correct rune clicked
                                {
                                    playerStep++; // Move to the next step in the sequence

                                    // If the player has clicked all the runes correctly
                                    if (playerStep == Sequence.Count)
                                    {
                                        StartNewSequence(); // Add a new rune to the sequence
                                    }
                                }
                                else // Wrong rune clicked
                                {
                                    Debug.WriteLine($"Wrong rune clicked! Resetting sequence.");
                                    Sequence.Clear(); // Reset sequence
                                    playerStep = 0; // Reset playerStep
                                    StartNewSequence(); // Start the sequence again
                                }
                                break;
                            }
                        }
                    }
                }

                // Save the current mouse state for the next update
                PreviousMouse = currentMouse;

                base.Update(gameTime);
                return; // exit the Update method
            }

            // start screen
            if (startMenu)
            {
                mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mouseState.Position.X < startButton.Right && mouseState.Position.X > startButton.Left &&
                            mouseState.Position.Y < startButton.Bottom && mouseState.Position.Y > startButton.Top)
                    { startMenu = false; }
                }
                base.Update(gameTime);
                return; // exit the Update method
            }

            // end screen
            if (!gamerunning || (gamerunning && player.health <=0))
            {
                mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mouseState.Position.X < startButton.Right && mouseState.Position.X > startButton.Left &&
                            mouseState.Position.Y < startButton.Bottom && mouseState.Position.Y > startButton.Top)
                    {
                        ResetGame();
                    }
                }
                base.Update(gameTime);
                return; // exit the Update method
            }

            if (inMaze)
            {
                score = newMaze.UpdatePosition(score); // ### REPLACE? score/stats

                if (newMaze.solved)
                { inMaze = false; }
                base.Update(gameTime);
                return; // exit the Update method
            }


            // move player & check wall colisions
            player.UpdatePlayer(controller, wallRect);

            // check puzzle trigger collisions
            if (controller.DidCollide(player.hitbox, mazeRect) && !newMaze.solved)
            {
                newMaze.StartMaze();
                inMaze = true;
            }
            if (controller.DidCollide(player.hitbox, runeRect) && !RuneGameWon & !inMemoryGame)
            {
                StartNewSequence();
                inMemoryGame = true;
            }
            if (controller.DidCollide(player.hitbox, skillRect) && !resultSuccess & !skillCheckActive)
            {
                StartSkillCheck(); //Starts the minigame
                skillCheckActive = true;
            }

            // check exit door collision
            if (controller.DidCollide(player.hitbox, exitRect) && newMaze.solved && RuneGameWon)
            {
                // ### ADD gameEnd/exit logic
                gamerunning = false;
            }

            if (animTimer > timerThreshold)
            {
                if (currentAnimationIndex == 7)
                {
                    currentAnimationIndex = 0;
                }
                else
                {
                    currentAnimationIndex++;
                }

                // Reset the timer.
                animTimer = 0;
            }
            // If the timer has not reached the threshold, then add the milliseconds that have past since the last Update() to the timer.
            else
            {
                animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tan);

            // TODO: Add your drawing code here
            if (startMenu) // !!! Work In Progress
            {
                _graphics.PreferredBackBufferWidth = 1064;
                _graphics.PreferredBackBufferHeight = 520;
                _graphics.ApplyChanges();

                _spriteBatch.Begin();
                _spriteBatch.Draw(menuSprite, new Rectangle(0, 0, 1064, 520), Color.White);
                _spriteBatch.DrawString(timerFont, "Temple Explorer", new Vector2(110, 100), Color.Black);
                _spriteBatch.Draw(exitSprite, startButton, Color.LimeGreen);
                _spriteBatch.DrawString(timerFont, "S T A R T", new Vector2(160, 220), Color.Black);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (!gamerunning && player.health > 0) // !!! Work In Progress
            {
                _graphics.PreferredBackBufferWidth = 1064;
                _graphics.PreferredBackBufferHeight = 520;
                _graphics.ApplyChanges();

                _spriteBatch.Begin();
                _spriteBatch.Draw(menuSprite, new Rectangle(0, 0, 1064, 520), Color.White);
                
                _spriteBatch.DrawString(timerFont, "You Win!", new Vector2(150, 100), Color.Black);
                //_spriteBatch.DrawString(spriteFont, credits.developers, new Vector2(100, 150), Color.Black);
                //_spriteBatch.DrawString(spriteFont, credits.references, new Vector2(100, 200), Color.Black);
                _spriteBatch.Draw(exitSprite, startButton, Color.LimeGreen);
                _spriteBatch.DrawString(timerFont, "RESTART", new Vector2(160, 220), Color.Black);

                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gamerunning && player.health <= 0)
            {
                _graphics.PreferredBackBufferWidth = 1064;
                _graphics.PreferredBackBufferHeight = 520;
                _graphics.ApplyChanges();

                _spriteBatch.Begin();
                _spriteBatch.Draw(menuSprite, new Rectangle(0, 0, 1064, 520), Color.White);

                _spriteBatch.DrawString(timerFont, "You Died!", new Vector2(150, 100), Color.Black);
                _spriteBatch.Draw(exitSprite, startButton, Color.LimeGreen);
                _spriteBatch.DrawString(timerFont, "RESTART", new Vector2(160, 220), Color.Black);

                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else
            {
                _graphics.PreferredBackBufferWidth = 500;
                _graphics.PreferredBackBufferHeight = 500;
                _graphics.ApplyChanges();

                GraphicsDevice.Clear(new Color(27, 105, 150));

                _spriteBatch.Begin();

                if (inMemoryGame)
                {
                    GraphicsDevice.Clear(Color.Tan);

                    for (int i = 0; i < runePositions.Count; i++)
                    {
                        Texture2D texture = (flashingRunes && (i == flashingIndex)) ? RuneSpriteOn[i] : RuneSpriteOff[i];

                        _spriteBatch.Draw(texture, runePositions[i], Color.White);
                        
                    }
                    _spriteBatch.DrawString(timerFont, Sequence.Count + " / " + MaxSequenceLength, new Vector2(150, 200), Color.Black);
                    

                }
                else if (inMaze)
                {
                    GraphicsDevice.Clear(Color.Tan);

                    _spriteBatch.Draw(mazeFloor, new Rectangle((int)newMaze.position.X, (int)newMaze.position.Y, 300, 300), Color.White);
                    _spriteBatch.Draw(mazeBlock, new Rectangle((int)newMaze.blockPosition.X, (int)newMaze.blockPosition.Y, 60, 63), Color.White);
                    _spriteBatch.Draw(mazeWall, new Rectangle((int)newMaze.position.X, (int)newMaze.position.Y, 300, 300), Color.White);
                }
                else if (skillCheckActive)
                {

                    GraphicsDevice.Clear(Color.Tan);

                    //Draw the skill check background
                    _spriteBatch.Draw(skillCheckTexture, new Vector2(100, 100), null, Color.White, 0f,
                        new Vector2(skillCheckTexture.Width / 2, skillCheckTexture.Height / 2), 0.3f, SpriteEffects.None, 0f);

                    //Draw the success zone
                    float zoneMidAngle = 300; //Temp number for now to allow the draw method to work
                    _spriteBatch.Draw(successZoneTexture, new Vector2(300, 150), null, Color.Red * 0.5f, zoneMidAngle,
                        new Vector2(successZoneTexture.Width / 2, successZoneTexture.Height / 2), 0.3f, SpriteEffects.None, 0f);

                    //Draws the needle
                    _spriteBatch.Draw(needleTexture, new Vector2(250, 250), null, Color.White, needleRotation,
                        new Vector2(needleTexture.Width / 2, needleTexture.Height / 2), 0.3f, SpriteEffects.None, 0f);

                }
                else
                {
                    
                    if (!newMaze.solved)
                    { _spriteBatch.Draw(puzzleSprite, mazeRect, Color.White); }
                    else
                    { _spriteBatch.Draw(puzzleSprite, mazeRect, Color.Chartreuse); }
                    if (!RuneGameWon)
                    { _spriteBatch.Draw(puzzleSprite, runeRect, Color.White); }
                    else
                    { _spriteBatch.Draw(puzzleSprite, runeRect, Color.Chartreuse); }
                    if (!resultSuccess)
                    { _spriteBatch.Draw(puzzleSprite, skillRect, Color.White); }
                    else
                    { _spriteBatch.Draw(puzzleSprite, skillRect, Color.Chartreuse); }

                    foreach (var rect in wallRect)
                    { _spriteBatch.Draw(wallSprite, rect, Color.Tan); }

                    _spriteBatch.Draw(wallSprite, exitRect, Color.Violet);

                    _spriteBatch.Draw(
                        currentEnemyAnimation[currentEnemyFrame], // the current frame of the enemy's animation
                        enemy.position, // enemy's position on the screen
                        Color.White // the color to draw the sprite
                    );

                    if (invincibilityFrames)
                    {
                        _spriteBatch.Draw(playerSprite, player.hitbox, playerRightRect[currentAnimationIndex], Color.Red);
                    }
                    else
                    {
                        _spriteBatch.Draw(playerSprite, player.hitbox, playerRightRect[currentAnimationIndex], Color.White);
                    }
                    
                }

                _spriteBatch.DrawString(timerFont, "Time: " + secondsElapsed, new Vector2(20, 20), Color.Red);
                _spriteBatch.DrawString(timerFont, "Score: " + score, new Vector2(20, 45), Color.Red);
                _spriteBatch.DrawString(timerFont, "Lives: " + player.health, new Vector2(20, 70), Color.Red);

                _spriteBatch.End();
                base.Draw(gameTime);
            }
        }

        public void ResetGame()
        {
            // reset all values that have changed
            startMenu = true;
            gamerunning = true;
            RuneGameWon = false;
            newMaze.solved = false;

            player.health = 3;
            invincibilityFrames = false;

            flashingIndex = -1;
            Sequence = new List<int>();
            playerStep = 0;
            currentFlashingRune = 0;
            flashingRunes = true;
            waitingForClick = false;
            runePositions = new List<Vector2>();

            score = 10;
            player.hitbox = new Rectangle(100, 100, 30, 48);

            enemy.position = new Rectangle(300, 400, 38, 71);
            enemyGoingLeft = false;
            enemyGoingRight = false;
            enemyGoingUp = true;
            enemyGoingDown = false;

            mazeWallFile = "MazeWall-";
            mazeFloorFile = "MazeFloor-";

            Initialize();
            LoadContent();
        }


    }
}
