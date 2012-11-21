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
    class Astronaut : PhysicalSprite
    {
        const string ASTRONAUT_ASSETNAME = "astronaut";
        const int START_POSITION_X = 0;
        const int START_POSITION_Y = 0;
        const float THRUSTER_FORCE = 10000;
        const float ASTRONAUT_MASS = 10;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const float THRUSTER_PARTICLE_VELOCITY = 120.0f;
        const float THRUSTER_PARTICLE_DECELERATION = 500.0f;
        const float ASTRONAUT_NATURAL_DECELERATION = 50.0f;
        const float ASTRONAUT_MAX_SPEED = 500.0f;
        const float ASTRONAUT_SCALE = 1.0f;
        //Particle Effect Parameters
        private ThrusterParticleEffect mDownwardThrusterEffect;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;
        KeyboardState mPreviousKeyboardState;

        public Astronaut()
            :base(ASTRONAUT_MASS, ASTRONAUT_SCALE, ASTRONAUT_NATURAL_DECELERATION, ASTRONAUT_MAX_SPEED)
        {
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, ASTRONAUT_ASSETNAME);
            mDownwardThrusterEffect = new ThrusterParticleEffect(new Vector2(Size.Center.X, Size.Center.Y),
                                                                 new Vector2(0, -THRUSTER_PARTICLE_VELOCITY),
                                                                 new Vector2(0, THRUSTER_PARTICLE_DECELERATION));
        }

        public override void Update(GameTime theGameTime, GraphicsDeviceManager graphics, Vector2 theFocusPoint)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            //TODO: Add a PositionVector property to sprite class
            //Reposition Downward Thruster to MidBottom of rect
            mDownwardThrusterEffect.Update(theGameTime, Center);
            base.Update(theGameTime, graphics, theFocusPoint);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            mDownwardThrusterEffect.Draw(theSpriteBatch);
            base.Draw(theSpriteBatch);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            Vector2 direction = Vector2.Zero;
            if (mCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    direction.X = MOVE_LEFT;
                    mDownwardThrusterEffect.Spawn(270.0f);
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    direction.X = MOVE_RIGHT;
                    mDownwardThrusterEffect.Spawn(90.0f);
                }
                if (aCurrentKeyboardState.IsKeyDown(Keys.W) == true)
                {
                    direction.Y = MOVE_UP;
                    //create thruster particles downward
                    mDownwardThrusterEffect.Spawn(0.0f);
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.S) == true)
                {
                    direction.Y = MOVE_DOWN;
                    mDownwardThrusterEffect.Spawn(180.0f);
                }
            }
            applyForce(direction * THRUSTER_FORCE);
        }
    }
}
