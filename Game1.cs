using Group4FinalProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace final_projectM
{
    public class Game1 : Game
    {
        //memory game stuff
        public bool inMemoryGame = true;
        

        List<int> Sequence = new List<int>();


        Random random = new Random();

        int flashingIndex = -1;

        Vector2[] runePositions;
        int playerStep = 0;
        bool gameOver = false;

        MouseState currentMouse, PreviousMouse;

        private const int MaxSequenceLength = 10;//maximum sequence before the game ends
        private bool RuneGameWon = false;
        private float runeFlashDelay = 0.5f;
        private float flashTimer = 0f;
        private int currentFlashingRune = 0;
        private bool flashingRunes = false;
        private bool waitingForClick = false;

        //health system stuff
        public int lives = 3;
        public bool invincibilityFrames = false;
        public bool inGame = true;
        float invincibilityTimer = 0f;

        //enemy stuff
        Health_System health = new Health_System();
        public bool enemyGoingLeft = true;
        public bool enemyGoingRight = false;
        public bool enemyGoingUp = false;
        public bool enemyGoingDown = false;

        float enemyAnimationTimer = 0f;
        int currentEnemyFrame = 0;
        float enemyFrameDuration = 0.2f;
        List<Texture2D> currentEnemyAnimation;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //full sprite sheet if needed

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

        //enemy speed
        Enemies enemy = new Enemies(2);

        SpriteFont gameFont;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
            //position of all the runes for memory game can be changed

            runePositions = new Vector2[]
            {
                new Vector2(50, 50), new Vector2(150, 50),
                new Vector2(250, 50), new Vector2(350, 50),
                new Vector2(450, 50), new Vector2(550, 50)
            };

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // all sprites are in order of animation
            gameFont = Content.Load<SpriteFont>("spaceFont");

            //enemy going up on the screen


            EnemySpritef1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f");
            EnemySpritef2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f1");
            EnemySpritef3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f2");
            EnemySpritef4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f3");
            enemyUpFrames = new List<Texture2D> {
                EnemySpritef1, EnemySpritef2, EnemySpritef3, EnemySpritef4
            };
            //enemy going down on the screen

            EnemySpriteb1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh02");
            EnemySpriteb2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0");
            EnemySpriteb3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh03");
            EnemySpriteb4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh04");
            enemyDownFrames = new List<Texture2D> {
            EnemySpriteb1, EnemySpriteb2, EnemySpriteb3, EnemySpriteb4
            };
            //enemy going right on the screen

            EnemySpriter1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r");
            EnemySpriter2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r1");
            EnemySpriter3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r2");
            EnemySpriter4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r3");
            enemyRightFrames = new List<Texture2D> {
            EnemySpriter1, EnemySpriter2, EnemySpriter3, EnemySpriter4
            };
            //enemy going left on the screen

            EnemySpritel1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l1");
            EnemySpritel2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l");
            EnemySpritel3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l2");
            EnemySpritel4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l3");
            enemyLeftFrames = new List<Texture2D> {
            EnemySpritel1, EnemySpritel2, EnemySpritel3, EnemySpritel4
            };

            //non current rune


            RuneSpriteFA = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811A");
            RuneSpriteFC = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811C");
            RuneSpriteFD = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811D");
            RuneSpriteFI = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811I");
            RuneSpriteFL = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811L");
            RuneSpriteFM = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811M");
            RuneSpriteOff = new List<Texture2D>
            {
                RuneSpriteFA, RuneSpriteFC, RuneSpriteFD, RuneSpriteFI, RuneSpriteFL, RuneSpriteFM
            };

            //current rune

            RuneSpriteNA = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuA");
            RuneSpriteNC = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuC");
            RuneSpriteND = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuD");
            RuneSpriteNI = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuI");
            RuneSpriteNL = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuL");
            RuneSpriteNM = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuM");
            RuneSpriteOn = new List<Texture2D>
            {
                RuneSpriteNA, RuneSpriteNC,RuneSpriteND,RuneSpriteNI,RuneSpriteNL,RuneSpriteNM
            };

            if (inMemoryGame == true)
            {

                StartNewSequence();
            }

        }
        private void StartNewSequence()
        {
            if (Sequence.Count >= MaxSequenceLength)
            {
                RuneGameWon = true;
                
                return;

            }
            
            playerStep = 0;
            Sequence.Add(random.Next(0, RuneSpriteOff.Count));


            flashingIndex = Sequence[0];

            flashingRunes = true;
            currentFlashingRune = 0;
            flashTimer = 0f;
            waitingForClick = false;
        }

        public void setCurrentEnemyAnimation(string enemyDirection)
        {
            switch (enemyDirection)
            {
                case "up":
                    currentEnemyAnimation = enemyUpFrames;
                    break;
                case "down":
                    currentEnemyAnimation = enemyDownFrames;
                    break;
                case "right":
                    currentEnemyAnimation = enemyRightFrames;
                    break;
                case "left":
                    currentEnemyAnimation = enemyLeftFrames;
                    break;
            }
        }
        public string enemyDirection;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            setCurrentEnemyAnimation(enemyDirection);

            enemyAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemyAnimationTimer >= enemyFrameDuration)
            {
                currentEnemyFrame = (currentEnemyFrame + 1) % currentEnemyAnimation.Count;
                enemyAnimationTimer = 0f;
            }
            /*
            if (health.didCollisionHappen(player, enemy) && !invincibilityFrames && inGame)
            {
                invincibilityFrames = true;
                lives--;
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
            }*/
            if (lives <= 0)
            {
                gameOver = true;
            }
            if (enemyGoingDown)
            {
                enemy.position.Y += enemy.speed;
                enemyDirection = "down";
            }
            if (enemyGoingLeft)
            {
                enemy.position.X -= enemy.speed;
                enemyDirection = "left";
            }
            if (enemyGoingRight)
            {
                enemy.position.X += enemy.speed;
                enemyDirection = "right";
            }
            if (enemyGoingUp)
            {
                enemy.position.Y -= enemy.speed;
                enemyDirection = "up";
            }

            if (inMemoryGame)
            {
                currentMouse = Mouse.GetState();

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
                        for (int i = 0; i < runePositions.Length; i++)
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
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            for (int i = 0; i < runePositions.Length; i++)
            {
                Texture2D texture = (flashingRunes && i == flashingIndex) ? RuneSpriteOn[i] : RuneSpriteOff[i];

                _spriteBatch.Draw(texture, runePositions[i], Color.White);
            }
            _spriteBatch.DrawString(gameFont, "lives" + lives, new Vector2(_graphics.PreferredBackBufferWidth / 2 - 400, _graphics.PreferredBackBufferHeight / 2 - 240), Color.White);
            

            _spriteBatch.Draw(
                currentEnemyAnimation[currentEnemyFrame], // the current frame of the enemy's animation
                new Vector2(enemy.position.X, enemy.position.Y), // enemy's position on the screen
                Color.White // the color to draw the sprite
            );

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
