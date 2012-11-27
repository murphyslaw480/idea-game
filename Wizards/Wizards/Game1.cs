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
        List<Goblin> goblins = new List<Goblin>();
        //sprites spit out by black hole
        List<PhysicalSprite> spitSprites = new List<PhysicalSprite>();

        TimeSpan minForFire = TimeSpan.FromMilliseconds(200);
        TimeSpan totalForFireElapsed;
        TimeSpan minForSpawn = TimeSpan.FromMilliseconds(5000);
        TimeSpan totalForSpawnElapsed;
        Astronaut player = new Astronaut();
        MouseIcon mMouseIconSprite = new MouseIcon();

        BlackHole blackHole;
        const float blackHoleGravity = 100000f;

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
            Vector2 blackHolePosition = new Vector2(0, graphics.GraphicsDevice.Viewport.Height);
            blackHole = new BlackHole(blackHoleGravity, blackHolePosition, 10.0f);

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
            player.Update(gameTime, graphics, blackHole.Gravity, mMouseIconSprite.Position);
            MouseState ms = Mouse.GetState();
            UpdateFire(ms, gameTime);
            SpawnGoblin(gameTime);
            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Update(gameTime);
            }

            //iterate in reverse so elements can be deleted while iterating
            //TODO -- include hit detection in this loop
            Goblin g;
            for (int i = goblins.Count - 1; i >= 0 ; i--)
            {
                g = goblins[i];
                blackHole.TryToEat(g);
                if (g.SpriteLifeState == PhysicalSprite.LifeState.Destroyed)
                    goblins.Remove(g);
                else
                    g.Update(gameTime, this.graphics, blackHole.Gravity, player.Position);
            }

            PhysicalSprite ps;
            for (int i = spitSprites.Count - 1; i >= 0 ; i--)
            {
                ps = spitSprites[i];
                if (outOfBounds(ps))
                    spitSprites.Remove(ps);
                else
                {
                    ps.Update(gameTime, this.graphics, blackHole.Gravity);
                }
            }

            removeLostBalls();
            checkFireEnemyCollision();
            blackHole.Update(gameTime);
            //check for sprites that black hole spits out
            PhysicalSprite SpitSprite = blackHole.SpriteToSpit;
            if (SpitSprite != null)
            {
                spitSprites.Add(SpitSprite);
            }
            base.Update(gameTime);
        }

        private void checkFireEnemyCollision()
        {
            List<Fire> fireToDelete = new List<Fire>();
            List<Goblin> enemyToDelete = new List<Goblin>();
            float firePosX, firePosY, enemyPosX, enemyPosY, enemyOffsetX, enemyOffsetY;
            foreach (Fire a in balls)
            {
                firePosX = a.Position.X + (a.Size.Width/2);
                firePosY = a.Position.Y + (a.Size.Width/2);
                foreach (Goblin g in goblins)
                {
                    enemyPosX = g.Position.X;
                    enemyPosY = g.Position.Y;
                    enemyOffsetX = g.Position.X + g.Size.Width;
                    enemyOffsetY = g.Position.Y + g.Size.Width;
                    if (firePosX > enemyPosX && firePosX < enemyOffsetX && firePosY > enemyPosY && firePosY < enemyOffsetY)
                    {
                        fireToDelete.Add(a);
                        enemyToDelete.Add(g);
                    }
                }
            }
            foreach (Fire a in fireToDelete)
            {
                balls.Remove(a);
            }
            foreach (Goblin g in enemyToDelete)
            {
                goblins.Remove(g);
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

        private void SpawnGoblin(GameTime gameTime)
        {
            if (((totalForSpawnElapsed += gameTime.ElapsedGameTime) > minForSpawn) && goblins.Count < 5)
            {
                Goblin g = new Goblin(Vector2.Zero);
                g.LoadContent(this.Content);
                totalForSpawnElapsed = TimeSpan.Zero;
                goblins.Add(g);
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

            blackHole.Draw(spriteBatch);

            foreach (Fire a in balls) // Loop through List with foreach
            {
                a.Draw(spriteBatch);
            }

            foreach (Goblin g in goblins) // Loop through List with foreach
            {
                g.Draw(spriteBatch);
            }

            foreach (PhysicalSprite ps in spitSprites) // Loop through List with foreach
            {
                ps.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
            mMouseIconSprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool outOfBounds(Sprite s)
        {
            return (s.Position.X < 0 || s.Position.Y < 0 || s.Position.X > graphics.GraphicsDevice.Viewport.Width || s.Position.Y > graphics.GraphicsDevice.Viewport.Height);
        }
    }
}
