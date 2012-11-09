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
    class Knight : Sprite
    {
        const string WIZARD_ASSETNAME = "Knight";
        const int START_POSITION_X = 0;
        const int START_POSITION_Y = 0;
        const int WIZARD_SPEED = 700;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        //Particle Effect Parameters
        private ParticleEffect mDownwardThrusterEffect;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            mDownwardThrusterEffect = new ParticleEffect(new Vector2(Size.Center.X, Size.Center.Y), new Vector2(10, 10), 
                new Vector2(0, 150), new Vector2(30, 30), 
                new Vector2(0, -10f), new Vector2(0, 2), 
                20, 5, 3);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
        }

        public void Update(GameTime theGameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            //TODO: Add a PositionVector property to sprite class
            mDownwardThrusterEffect.Update(theGameTime, new Vector2(Position.X + Size.Center.X, Position.Y + Size.Center.Y));
            base.Update(theGameTime, mSpeed, mDirection, graphics);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            mDownwardThrusterEffect.Draw(theSpriteBatch);
            base.Draw(theSpriteBatch);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    mSpeed.X = WIZARD_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }
                if (aCurrentKeyboardState.IsKeyDown(Keys.W) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_UP;
                    //create thruster particles downward
                    mDownwardThrusterEffect.Spawn();
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.S) == true)
                {
                    mSpeed.Y = WIZARD_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }

    }
}
