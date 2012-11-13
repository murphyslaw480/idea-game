using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class ExhaustParticle : Particle
    {
        //how much to expand particle each frame
        private const float scalePerUpdate = 1.5f; 
        //how much to change each color component each frame
        private const byte redDecrement = 7;
        private const byte greenIncrement = 3;
        private const byte blueIncrement = 3;
        private const byte alphaDecrement = 15;

        public ExhaustParticle()
            : base(0, Vector2.Zero, Vector2.Zero, Vector2.Zero, Color.Red, 2.0f)
        { }

        public ExhaustParticle(int theLife, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(theLife, thePosition, theVelocity, theAcceleration, Color.Red, 2.0f)
        {}

        public void Update(GameTime theGameTime)
        {
            //exhaust particles will expand every frame and fade to gray before disappearing (to look like smoke)
            Scale += scalePerUpdate;
            ParticleColor.R -= redDecrement;
            ParticleColor.G += greenIncrement;
            ParticleColor.B += blueIncrement;
            ParticleColor.A -= alphaDecrement;
            Angle += 0.5f;
            base.Update(theGameTime);
        }

    }
}
