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
        private const float scalePerMilliSecond = 0.15f; 
        //how much to rotate particle each frame
        private const float rotatePerMilliSecond = 0.5f; 
        //color of the particle
        private static Color particleColor = Color.Black;
        //point about which to rotate - should be center of black hole
        private Vector2 blackHoleCenter;

        public BlackHoleParticle(Vector2 theBlackHoleCenter)
            : base(0, Vector2.Zero, Vector2.Zero, Vector2.Zero, particleColor, 2.0f)
        {
            blackHoleCenter = theBlackHoleCenter;
        }

        public BlackHoleParticle(Vector2 theBlackHoleCenter, int theLife, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(theLife, thePosition, theVelocity, theAcceleration, particleColor, 2.0f)
        {
            blackHoleCenter = theBlackHoleCenter;
        }

        public void Update(GameTime theGameTime)
        {
            //exhaust particles will expand every frame and fade to gray before disappearing (to look like smoke)
            Scale += scalePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            Angle += rotatePerMilliSecond * theGameTime.ElapsedGameTime.Milliseconds;
            base.Update(theGameTime);
        }

    }
}
