using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;



namespace Wizards
{
    class Sprite
    {
        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 0.3f;

        private float angle = 0.0f;

        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        private Texture2D mSpriteTexture;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, angle, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //When the scale is modified throught he property, the Size of the 
        //sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }

        //Player Update
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection, GraphicsDeviceManager graphics)
        {
            Vector2 nextPos = Position;
            nextPos += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            if (nextPos.X < 0)
            {
                nextPos.X = 0;
            }
            if (nextPos.Y < 0)
            {
                nextPos.Y = 0;
            }
            if ((nextPos.X + Size.Width) > graphics.GraphicsDevice.Viewport.Width)
            {
                nextPos.X = graphics.GraphicsDevice.Viewport.Width - Size.Width;
            }
            if ((nextPos.Y + Size.Height) > graphics.GraphicsDevice.Viewport.Height)
            {
                nextPos.Y = graphics.GraphicsDevice.Viewport.Height - Size.Height;
            }
            Position = nextPos;
        }

        // Enemy/Fireball Update
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection, float angle)
        {
            Vector2 nextPos = Position;
            nextPos += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            Position = nextPos;
            this.angle = angle;
        }

        // TODO: New Enemy Update
        /*public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection, Vector2 playerPos)
        {
            Vector2 nextPos = Position;
            nextPos += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            Position = nextPos;
        }*/

        // Mouse Update
        public void Update(MouseState ms)
        {
            #if WINDOWS
                Position.X = ms.X;
                Position.Y = ms.Y;
            #endif
        }
    }
}
