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
    class Fire : Sprite
    {
        const string WIZARD_ASSETNAME = "Fireball";
        int START_POSITION_X = 0;
        int START_POSITION_Y = 0;
        const int WIZARD_SPEED = 400;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        float angle;

        public Fire(Vector2 pos, Vector2 pos2)
        {
            START_POSITION_X = (int)pos.X;
            START_POSITION_Y = (int)pos.Y;
            mDirection = pos2 - pos;
            float mDirectionLength = (float)(Math.Sqrt(mDirection.X * mDirection.X + mDirection.Y * mDirection.Y));
            mDirection = mDirection / mDirectionLength;
            mSpeed.X = WIZARD_SPEED;
            mSpeed.Y = WIZARD_SPEED;
            calcAngle();
        }

        private void calcAngle()
        {
            Vector2 reference;
            float temp;
            reference.X = 1;
            reference.Y = 0;
            temp = (reference.X * mDirection.X) + (reference.Y * mDirection.Y);
            angle = (float)Math.Acos(temp);
            if (reference.Y-mDirection.Y > 0)
            {
                angle = (float)((2*Math.PI) - angle);
            }
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, WIZARD_ASSETNAME);
            Scale = 0.1f;
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, mSpeed, mDirection, angle);
        }

    }
}
