using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class Enemy : Sprite
    {

        const string WIZARD_ASSETNAME = "CatCreature";
        int START_POSITION_X = 0;
        int START_POSITION_Y = 0;
        const int WIZARD_SPEED = 100;
        float angle = 0.0f;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        public Enemy(GraphicsDeviceManager graphics)
        {
            Random generator = new Random();
            START_POSITION_X = generator.Next(graphics.GraphicsDevice.Viewport.Width);
            START_POSITION_Y = generator.Next(graphics.GraphicsDevice.Viewport.Height);
            mSpeed.X = WIZARD_SPEED;
            mSpeed.Y = WIZARD_SPEED;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
            Scale = 0.3f;
        }

        public void Update(GameTime theGameTime, Vector2 playerPos, Rectangle playerSize)
        {
            playerPos.X += (playerSize.Width/2);
            playerPos.Y += (playerSize.Height/2);
            Vector2 adjsPos = Position;
            adjsPos.X += (Size.Width/2);
            adjsPos.Y += (Size.Height/2);
            mDirection = playerPos - adjsPos;
            float mDirectionLength = (float)(Math.Sqrt(mDirection.X * mDirection.X + mDirection.Y * mDirection.Y));
            mDirection = mDirection / mDirectionLength;
            base.Update(theGameTime, mSpeed, mDirection, angle);
        }

    }
}
