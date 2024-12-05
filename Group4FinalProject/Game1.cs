using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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

		Random random;

		int score = 0;
		int radius = 0;
		float needleRotation; //Rotation angle of the needle
		float needleSpeed = 2f; //Speed of the needle

		bool skillCheckActive; //Check to see if the skill check is active
		bool resultSuccess; //Result of the skill check

		private TimeSpan elapsedTime;
		private int secondsElapsed;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{

			_graphics.PreferredBackBufferWidth = 1500;
			_graphics.PreferredBackBufferHeight = 900; //THIS WAS FOR TESTING GET RID OF THESES TWO LINES
			_graphics.ApplyChanges();

			elapsedTime = TimeSpan.Zero;
			secondsElapsed = 0;

			base.Initialize();

			StartSkillCheck(); //Starts the minigame

			//if (MediaPlayer.State != MediaState.Playing)
			//{
			//	MediaPlayer.IsRepeating = true;
			//	MediaPlayer.Play(beginingMusic);
			//}
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			random = new Random();

			skillCheckTexture = Content.Load<Texture2D>("Circle");
			needleTexture = Content.Load<Texture2D>("needle");
			successZoneTexture = Content.Load<Texture2D>("good");

			timerFont = Content.Load<SpriteFont>("timerFont");
			explosionSound = Content.Load<SoundEffect>("sfx_generator_explode_01");
			StartSkillCheckSound = Content.Load<SoundEffect>("sfx_hud_skillcheck_open_04");
			SuccessSkillCheck = Content.Load<SoundEffect>("sfx_hud_skillcheck_open_04");
			//beginingMusic = Content.Load<Song>("D&D Menu Survivor");
			//endingMusic = Content.Load<Song>("Alan Wake Menu");
			// TODO: use this.Content to load your game content here
		}

		void StartSkillCheck()
		{
			StartSkillCheckSound.Play();

			skillCheckActive = true;
			resultSuccess = false;
			needleRotation = 0f;

		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

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
					float successZoneEndAngle = successZoneStartAngle + successZoneArc; //Change entire if statement to  fix collsion //INPORTANT!!!!

					if (needleAngle >= successZoneStartAngle && needleAngle <= successZoneEndAngle)
					{
						SuccessSkillCheck.Play();

						resultSuccess = true; //Successful skill check
					}
					else
					{
						explosionSound.Play();

						resultSuccess = false; //Failed skill check
					}
					//skillCheckActive = false; // Ends the skill check
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

		// TODO: Add your drawing code here
		_spriteBatch.Begin();

			if (skillCheckActive)
			{
				//Draw the skill check background
				_spriteBatch.Draw(skillCheckTexture, new Vector2(750, 450), null, Color.White, 0f,
					new Vector2(skillCheckTexture.Width / 2, skillCheckTexture.Height / 2), 1f, SpriteEffects.None, 0f);

				//Draw the success zone
				float zoneMidAngle = 1100; //Temp number for now to allow the draw method to work
				_spriteBatch.Draw(successZoneTexture, new Vector2(1100, 350), null, Color.Red * 0.5f, zoneMidAngle,
					new Vector2(successZoneTexture.Width / 2, successZoneTexture.Height / 2), 1f, SpriteEffects.None, 0f);

				//Draws the needle
				_spriteBatch.Draw(needleTexture, new Vector2(750, 450), null, Color.White, needleRotation,
					new Vector2(needleTexture.Width / 2, needleTexture.Height / 2), 1f, SpriteEffects.None, 0f);

				_spriteBatch.DrawString(timerFont, "Time: " + secondsElapsed, new Vector2(_graphics.PreferredBackBufferWidth / 2, 30), Color.White);

				_spriteBatch.DrawString(timerFont, "Score: " + score, new Vector2(_graphics.PreferredBackBufferWidth / 2, 60), Color.White);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
