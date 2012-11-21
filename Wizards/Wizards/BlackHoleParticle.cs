using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class BlackHoleParticle : Particle
    {
        //how much to expand particle each frame
        private const float scalePerMilliSecond = -0.07f; 
        //how much to rotate particle each frame
        private const float rotatePerMilliSecond = 0.001f; 
        //what size to start paticles at
        private const float startingScale = 30.0f;
        //color of the particle
        private static Color particleColor = Color.Black;
        //point about which to rotate - should be center of black hole
        private Vector2 blackHoleCenter;

        public BlackHoleParticle(Vector2 theBlackHoleCenter)
            : base(0, Vector2.Zero, Vector2.Zero, Vector2.Zero, particleColor, startingScale)
        {
            blackHoleCenter = theBlackHoleCenter;
        }

        public override void Update(GameTime theGameTime)
        {
            //black hole particles will scale and rotate each frame
            Scale += scalePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            Angle += rotatePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            base.Update(theGameTime);
        }

    }
}
