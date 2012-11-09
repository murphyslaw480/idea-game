using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    //basic particle class that maintains information about its own location, velocity
    //scale, and lifetime.
    //It is up to the ParticleEffect to draw particles with the appropriate texture
    //as well as delete them when their life has expired

    class Particle
    {
        public static Texture2D BlankParticleTexture;
        public Vector2 Position, Velocity, Acceleration;
        public float Scale, Angle;
        public int LifeTime;        //how many frames the particle has left to exist
        public Color ParticleColor;

        public Particle()
        {
            LifeTime = 0;
            Position = Velocity = Acceleration = Vector2.Zero;
            Scale = Angle = 0.0f;
            ParticleColor = Color.White;
        }
        /// <summary>
        /// Create a new particle
        /// </summary>
        /// <param name="theLife">The number of frames the particle should exist for</param>
        /// <param name="thePosition">Initial Particle Location</param>
        /// <param name="theVelocity">Initial Particle Velocity</param>
        /// <param name="theAcceleration">Initial Particle Acceleration</param>
        public Particle(int theLife, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
        {
            LifeTime = theLife;
            Position = thePosition;
            Velocity = theVelocity;
            Acceleration = theAcceleration;
            Scale = 9.0f;
            Angle = 0.0f;
            ParticleColor = Color.White;
        }

        public Particle(int theLife, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration, Color theColor, float theScale = 0.0f, float theAngle = 0.0f)
            : this(theLife, thePosition, theVelocity, theAcceleration)
        {
            ParticleColor = theColor;
            Scale = theScale;
            Angle = theAngle;
        }

        public void Update(GameTime theGameTime)
        {
            Velocity += Acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            LifeTime--;     //Decrement Life, Particle Effect should remove this particle if it reaches 0
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(BlankParticleTexture, Position, null, ParticleColor, Angle, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
