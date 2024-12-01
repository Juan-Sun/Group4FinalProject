using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace final_projectM
{
    public class Game1 : Game
    {
        //memory game stuff
        public bool inMemoryGame=false;

        List<int> Sequence = new List<int>();
        int currentStep = 0;

        Random random = new Random();
        float flashTime = 0.5f;
        float SequenceTimer = 0;
        bool isFlashing = false;
        int flashingIndex = -1;

        Vector2[] runePositions;
        int playerStep = 0;
        bool gameOver = false;

        MouseState currentMouse, PreviousMouse;

        private const int MaxSequenceLength = 10;//maximum sequence before the game ends
        private bool RuneGameWon = false;


        //health system stuff
        public int lives = 3;
        public bool invincibilityFrames=false;
        public bool inGame = true;
        float invincibilityTimer = 0f;

        //enemy stuff
        Health_System health = new Health_System();
        public bool enemyGoingLeft=false;
        public bool enemyGoingRight=false;
        public bool enemyGoingUp=false;
        public bool enemyGoingDown=false;

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
        Enemies enemy =new Enemies();
        
       


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
            if (inMemoryGame)
            {
                runePositions = new Vector2[]
                {
                new Vector2(50, 50), new Vector2(150, 50),
                new Vector2(250, 50), new Vector2(350, 50),
                new Vector2(450, 50), new Vector2(550, 50)
                };
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // all sprites are in order of animation

            //enemy going up on the screen
            enemyUpFrames = new List<Texture2D> {
                EnemySpritef1, EnemySpritef2, EnemySpritef3, EnemySpritef4
            };

            EnemySpritef1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f");
            EnemySpritef2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f1");
            EnemySpritef3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f2");
            EnemySpritef4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0f3");

            //enemy going down on the screen
            enemyDownFrames = new List<Texture2D> {
            EnemySpriteb1, EnemySpriteb2, EnemySpriteb3, EnemySpriteb4
            };
            EnemySpriteb1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh02");
            EnemySpriteb2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0");
            EnemySpriteb3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh03");
            EnemySpriteb4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh04");
            //enemy going right on the screen
            enemyRightFrames = new List<Texture2D> {
            EnemySpriter1, EnemySpriter2, EnemySpriter3, EnemySpriter4
            };
            EnemySpriter1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r");
            EnemySpriter2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r1");
            EnemySpriter3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r2");
            EnemySpriter4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0r3");
            //enemy going left on the screen
            enemyLeftFrames = new List<Texture2D> {
            EnemySpritel1, EnemySpritel2, EnemySpritel3, EnemySpritel4
            };
            EnemySpritel1 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l1");
            EnemySpritel2 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l");
            EnemySpritel3 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l2");
            EnemySpritel4 = Content.Load<Texture2D>("360_F_144585898_2SJyac3Cm047wvA63dVTsELaws9c8Mh0l3");

            //non current rune
            RuneSpriteOff = new List<Texture2D>
            {
                RuneSpriteFA, RuneSpriteFC,RuneSpriteFD,RuneSpriteFI,RuneSpriteFL,RuneSpriteFM
            };
            RuneSpriteFA = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811A");
            RuneSpriteFC = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811C");
            RuneSpriteFD = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811D");
            RuneSpriteFI = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811I");
            RuneSpriteFL = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811L");
            RuneSpriteFM = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811M");

            //current rune
            RuneSpriteOn = new List<Texture2D>
            {
                RuneSpriteNA, RuneSpriteNC,RuneSpriteND,RuneSpriteNI,RuneSpriteNL,RuneSpriteNM
            };
            RuneSpriteNA = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuA");
            RuneSpriteNC = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuC");
            RuneSpriteND = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuD");
            RuneSpriteNI = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuI");
            RuneSpriteNL = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuL");
            RuneSpriteNM = Content.Load<Texture2D>("scandinavian-runes-carved-on-stones-600nw-2399046811CuM");

            if (inMemoryGame == true)
            {
                StartNewSequence();
            }
            
        }
        private void StartNewSequence()
        {
            if (Sequence.Count>=MaxSequenceLength)
            {
                RuneGameWon = true;
                return;
            }
            playerStep = 0;
            Sequence.Clear();
            Sequence.Add(random.Next(0, RuneSpriteOff.Count));
            currentStep = 0;
            isFlashing = true;
            SequenceTimer = 0;
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
            }
            if (enemyGoingDown)
            {
                enemyDirection = "down";
            }
            if (enemyGoingLeft)
            {
                enemyDirection = "left";
            }
            if (enemyGoingRight)
            {
                enemyDirection = "right";
            }
            if (enemyGoingUp)
            {
                enemyDirection = "up";
            }

            if (inMemoryGame)
            {
                currentMouse = Mouse.GetState();
                SequenceTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (isFlashing)
                {
                    // Flash runes in sequence
                    if (SequenceTimer > flashTime)
                    {
                        SequenceTimer = 0;
                        currentStep++;
                        if (currentStep >= Sequence.Count)
                        {
                            isFlashing = false;
                            flashingIndex = -1;
                        }
                        else
                        {
                            flashingIndex = Sequence[currentStep];
                        }
                    }
                }
                else
                {
                    flashingIndex = -1;

                    // Handle player input
                    if (currentMouse.LeftButton == ButtonState.Pressed &&
                        PreviousMouse.LeftButton == ButtonState.Released)
                    {
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
                                if (i == Sequence[playerStep])
                                {
                                    playerStep++;
                                    if (playerStep >= Sequence.Count)
                                    {
                                        StartNewSequence(); // Progress to the next level
                                    }
                                }
                                else
                                {
                                    {
                                        //add score loss here
                                        Sequence.Clear();
                                        StartNewSequence();
                                    }

                                }
                                break;

                            }
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            for (int i = 0; i < runePositions.Length; i++)
            {
                Texture2D texture = (i == flashingIndex) ? RuneSpriteOn[i] : RuneSpriteOff[i];
                _spriteBatch.Draw(texture, runePositions[i], Color.White);
            }
            

            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
