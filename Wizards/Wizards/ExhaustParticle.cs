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
        private const float scalePerMilliSecond = 0.15f; 
        private const float rotatePerMilliSecond = 0.5f; 
        //how much to change each color component each second
        private const byte redDecrement = 10;
        private const byte greenIncrement = 5;
        private const byte blueIncrement = 5;
        private const byte alphaDecrement = 20;

        public ExhaustParticle()
            : base(0, Vector2.Zero, Vector2.Zero, Vector2.Zero, Color.Red, 2.0f)
        { }

        public ExhaustParticle(int theLife, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(theLife, thePosition, theVelocity, theAcceleration, Color.Red, 2.0f)
        {}

        public override void Update(GameTime theGameTime)
        {
            //exhaust particles will expand every frame and fade to gray before disappearing (to look like smoke)
            Scale += scalePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            Angle += rotatePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            ParticleColor.R -= redDecrement;
            ParticleColor.G += greenIncrement;
            ParticleColor.B += blueIncrement;
            ParticleColor.A -= alphaDecrement;
            base.Update(theGameTime);
        }

    }
}
