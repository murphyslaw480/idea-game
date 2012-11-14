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
        //Default particle spawning variance parameters -- play around till it looks right
        private const float defaultParticleLife = 0.8f;
        private const float defaultParticleLifeSpread = 0.08f;
        private const int defaultSpawnDensity = 58;
        static Vector2 defaultParticleVelocity = Vector2.Zero;
        static Vector2 defaultParticleAcceleration = Vector2.Zero;
        static Vector2 defaultPositionSpread = new Vector2(50, 50);
        static Vector2 defaultVelocitySpread = new Vector2(70, 50);
        static Vector2 defaultAccelerationSpread = new Vector2(1000, 0);

        public BlackHole(Vector2 thePosition)
            : base(thePosition, defaultPositionSpread,
                   defaultParticleVelocity, defaultVelocitySpread,
                   defaultParticleAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        { }
        
        public BlackHole(Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration)
            : base(thePosition, defaultPositionSpread,
                   theVelocity, defaultVelocitySpread,
                   theAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        { }

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
