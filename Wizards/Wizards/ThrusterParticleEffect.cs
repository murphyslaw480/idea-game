using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class ThrusterParticleEffect : ParticleEffect
    {
        //Default particle spawning variance parameters -- play around till it looks right
        private const float defaultParticleLife = 0.3f;
        private const float defaultParticleLifeSpread = 0.08f;
        private const int defaultSpawnDensity = 5;
        static Vector2 defaultPositionSpread = new Vector2(10, 10);
        static Vector2 defaultVelocitySpread = new Vector2(30, 30);
        static Vector2 defaultAccelerationSpread = new Vector2(0, 0);

        public ThrusterParticleEffect(Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(thePosition, defaultPositionSpread,
                   theVelocity, defaultVelocitySpread,
                   theAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        { }

        public void Update(GameTime theGameTime)
        {
            //Traverse in reverse to allow removal
            for(int i = mParticles.Count - 1 ; i >= 0 ; i--)
            {
                if (mParticles[i].LifeTime <= 0)
                {
                    mParticles.Remove(mParticles[i]);
                }

                else
                {//must explicitly cast to ExhaustParticle to get proper update method
                    ((ExhaustParticle)(mParticles[i])).Update(theGameTime);
                }
            }
        }

        public void Update(GameTime theGameTime, Vector2 newPosition)
        {
            SourcePosition = newPosition;
            this.Update(theGameTime);
        }

        /// <summary>
        /// Spawn new exhaust particles at the given angle
        /// Set angle to 0 for upward, 90 for right, 180 for down, 270 for left
        /// </summary>
        /// <param name="rotationAngle"></param>
        public void Spawn(float rotationAngle)
        {
            //spawn a number of particles determined by spawndensity
            for (int i = 0; i < SpawnDensity; i++)
            {
                base.AddNewParticle(new ExhaustParticle(), rotationAngle);
            }
        }

    }
}
