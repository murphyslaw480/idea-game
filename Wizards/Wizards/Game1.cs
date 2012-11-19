using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Wizards
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        List<Fire> balls = new List<Fire>();
        List<Enemy> enemies = new List<Enemy>();
        TimeSpan minForFire = TimeSpan.FromMilliseconds(200);
        TimeSpan totalForFireElapsed;
        TimeSpan minForSpawn = TimeSpan.FromMilliseconds(5000);
        TimeSpan totalForSpawnElapsed;
        Astronaut player = new Astronaut();
        MouseIcon mMouseIconSprite = new MouseIcon();

        BlackHole blackHole;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //balls.Add(new Knight());
            //balls.Add(new MouseIcon());
            player = new Astronaut();
            mMouseIconSprite = new MouseIcon();
            totalForFireElapsed = TimeSpan.FromMilliseconds(200);
            totalForSpawnElapsed = TimeSpan.Zero;

            //create a black hole in bottom left of screen
            Vector2 blackHolePosition = new Vector2(30, graphics.GraphicsDevice.Viewport.Height - 30);
            blackHole = new BlackHole(blackHolePosition, Vector2.Zero, Vector2.Zero);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            /*foreach (Fire a in balls)
            {
                a.LoadContent(this.Content);
            }*/
            player.LoadContent(this.Content);
            mMouseIconSprite.LoadContent(this.Content);
            //Load single white pixel for particle texture
            Particle.BlankParticleTexture = Content.Load<Texture2D>("BlankParticle");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            mMouseIconSprite.Update();
            player.Update(gameTime, graphics, mMouseIconSprite.Position);
            MouseState ms = Mouse.GetState();
            UpdateFire(ms, gameTime);
            SpawnEnemy(gameTime);
            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Update(gameTime);
            }
            foreach (Enemy a in enemies) // Loop through List with foreach
            {
                a.Update(gameTime, player.Position, player.Size);
            }
            removeLostBalls();
            checkFireEnemyCollision();
            blackHole.Update(gameTime);
            base.Update(gameTime);
        }

        private void checkFireEnemyCollision()
        {
            List<Fire> fireToDelete = new List<Fire>();
            List<Enemy> enemyToDelete = new List<Enemy>();
            float firePosX, firePosY, enemyPosX, enemyPosY, enemyOffsetX, enemyOffsetY;
            foreach (Fire a in balls)
            {
                firePosX = a.Position.X + (a.Size.Width/2);
                firePosY = a.Position.Y + (a.Size.Width/2);
                foreach (Enemy b in enemies)
                {
                    enemyPosX = b.Position.X;
                    enemyPosY = b.Position.Y;
                    enemyOffsetX = b.Position.X + b.Size.Width;
                    enemyOffsetY = b.Position.Y + b.Size.Width;
                    if (firePosX > enemyPosX && firePosX < enemyOffsetX && firePosY > enemyPosY && firePosY < enemyOffsetY)
                    {
                        fireToDelete.Add(a);
                        enemyToDelete.Add(b);
                    }
                }
            }
            foreach (Fire a in fireToDelete)
            {
                balls.Remove(a);
            }
            foreach (Enemy a in enemyToDelete)
            {
                enemies.Remove(a);
            }
        }

        private void removeLostBalls()
        {
            List<Fire> toDelete = new List<Fire>();
            foreach (Fire a in balls)
            {
                // If the fireball is off the screen - delete it.
                if (a.Position.X < 0 || a.Position.Y < 0 || a.Position.X > graphics.GraphicsDevice.Viewport.Width || a.Position.Y > graphics.GraphicsDevice.Viewport.Height)
                {
                    toDelete.Add(a);
                }
            }
            foreach (Fire a in toDelete)
            {
                balls.Remove(a);
            }
        }

        private void SpawnEnemy(GameTime gameTime)
        {
            if (((totalForSpawnElapsed += gameTime.ElapsedGameTime) > minForSpawn) && enemies.Count < 5)
            {
                Enemy a = new Enemy(graphics);
                a.LoadContent(this.Content);
                totalForSpawnElapsed = TimeSpan.Zero;
                enemies.Add(a);
            }
        }

        private void UpdateFire(MouseState ms, GameTime gameTime)
        {
            if (ms.LeftButton == ButtonState.Pressed && (totalForFireElapsed += gameTime.ElapsedGameTime) > minForFire)
            {
                Vector2 adjustedPlayPos = player.Position;
                adjustedPlayPos.X += (player.Size.Width/2);
                adjustedPlayPos.Y += (player.Size.Height/2);
                Fire a = new Fire(adjustedPlayPos, mMouseIconSprite.Position);
                a.LoadContent(this.Content);
                totalForFireElapsed = TimeSpan.Zero;
                balls.Add(a);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Draw(spriteBatch);
            }
            foreach (Enemy a in enemies) // Loop through List with foreach
            {
                a.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            blackHole.Draw(spriteBatch);
            mMouseIconSprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
