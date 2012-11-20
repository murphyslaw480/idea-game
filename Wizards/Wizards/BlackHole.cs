using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class BlackHole : ParticleEffect
    {
        public readonly Gravity gravity;
        //Default particle spawning variance parameters -- play around till it looks right
        private const float defaultParticleLife = 0.6f;
        private const float defaultParticleLifeSpread = 0.08f;
        private const int defaultSpawnDensity = 6;
        static Vector2 defaultParticleVelocity = new Vector2(9.0f,0);
        static Vector2 defaultParticleAcceleration = new Vector2(1, 0);
        static Vector2 defaultPositionSpread = new Vector2(40, 40);
        static Vector2 defaultVelocitySpread = new Vector2(0, 0);
        static Vector2 defaultAccelerationSpread = new Vector2(0, 0);

        public BlackHole(float theGravity, Vector2 thePosition)
            : base(thePosition, defaultPositionSpread,
                   defaultParticleVelocity, defaultVelocitySpread,
                   defaultParticleAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        {
            gravity = new Gravity(thePosition, theGravity);
        }
        
        public BlackHole(float theGravity, Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(thePosition, defaultPositionSpread,
                   theVelocity, defaultVelocitySpread,
                   theAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        {
            gravity = new Gravity(thePosition, theGravity);
        }

        public void Update(GameTime theGameTime)
        {
            //auto-spawn new particles
            Spawn();

            //Traverse in reverse to allow removal
            for(int i = mParticles.Count - 1 ; i >= 0 ; i--)
            {
                if (mParticles[i].LifeTime <= 0)
                {
                    mParticles.Remove(mParticles[i]);
                }

                else
                {//must explicitly cast to ExhaustParticle to get proper update method
                    ((BlackHoleParticle)(mParticles[i])).Update(theGameTime);
                }
            }
        }

        public void Update(GameTime theGameTime, Vector2 newPosition)
        {
            SourcePosition = newPosition;
            this.Update(theGameTime);
        }

        public void Spawn()
        {
            //spawn a number of particles determined by spawndensity
            for (int i = 0; i < SpawnDensity; i++)
            {
                base.AddNewParticle(new BlackHoleParticle(SourcePosition));
                //base.AddNewParticle(new BlackHoleParticle(SourcePosition), SourcePosition, BaseVelocity, BaseAcceleration); DELETE
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle particle in mParticles)
            {
                Vector2 rotationCenter = 0.1f * (SourcePosition - particle.Position);
                particle.Draw(sb, rotationCenter);
            }
        }
    }
}
