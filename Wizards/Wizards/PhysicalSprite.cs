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
    /// <summary>
    /// A specialization of Sprite with physics properties
    /// Uses Velocity, Acceleration, and mass to respond to forces for movement
    /// </summary>
    class PhysicalSprite : Sprite
    {
        //change in position per second (px/s)
        private Vector2 _velocity = Vector2.Zero;
        //change in velocity (px / s^2)
        private Vector2 _acceleration = Vector2.Zero;
        //Mass affects how an object responds to forces (a = F/m)
        public readonly float Mass;
        //an object in motion remains in motion. Unless its a videogame, like this
        private readonly float naturalDeceleration;

        public PhysicalSprite(float mass)
        {
            Mass = mass;
            naturalDeceleration = 0.0f;
        }

        public PhysicalSprite(float mass, float theNaturalDeceleration)
        {
            Mass = mass;
            naturalDeceleration = theNaturalDeceleration;
        }

        public void applyForce(Vector2 theForce)
        {
            _acceleration += theForce / Mass;
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics)
        {
            _velocity += _acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            applyNaturalDeceleration();
            //apply velocity to Sprite Update method - which will also check screen bounds
            base.Update(theGameTime, _velocity, new Vector2(1, 1), theGraphics);
            _acceleration = Vector2.Zero;
        }

        /// <summary>
        /// gradually reduce the object's velocity 
        /// </summary>
        private void applyNaturalDeceleration()
        {
            float speed = _velocity.Length();
            if ((int)speed > 0 && !((int)_acceleration.Length() > 0))
            {
                float xRatio = _velocity.X / speed;
                float yRatio = _velocity.Y / speed;
                Vector2 deceleration = new Vector2(xRatio * naturalDeceleration, yRatio * naturalDeceleration);
                _velocity -= deceleration;
            }
        }
    }
}
