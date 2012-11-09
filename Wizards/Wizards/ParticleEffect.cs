using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    //manages a group of related particles in a list
    //calls particle update and draw methods and can spawn new particles
    class ParticleEffect
    {
        public static Random Rand = new Random();
        public Vector2 SourcePosition;  //Point from which new particles spawn
        public Vector2 PositionSpread;  //Randomness in spawn location for each particle
        public Vector2 BaseVelocity;    //Default velocity for spawned particles
        public Vector2 VelocitySpread;   //Randomness in velocity for each particle
        public Vector2 BaseAcceleration;    //Default Acceleration for spawned particles
        public Vector2 AccelerationSpread;   //Randomness in Acceleration for each particle
        public int DefaultParticleLife, ParticleLifeSpread;     //Default particle lifetime (see particle class)
        public int SpawnDensity;        //number of particles created per spawn call
        private List<Particle> mParticles;      //list of living particles

        /// <summary>
        /// Create a new particle generator at the given location
        /// If it is anchored to a moving object, Position should be updated every frame by its owner
        /// </summary>
        /// <param name="thePosition">Source Location for spawned particles</param>
        /// <param name="theVelocity">Velocity for spawned particles</param>
        /// <param name="theAcceleration">Acceleration for spawned particles</param>
        /// <param name="theParticleLife">How long spawned particles will exist</param>
        /// <param name="theSpawnDensity">How many particles to spawn per call</param>
        public ParticleEffect(Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration, int theParticleLife, int theSpawnDensity = 1)
        {
            SourcePosition = thePosition;
            BaseVelocity = theVelocity;
            BaseAcceleration = theAcceleration;
            PositionSpread = VelocitySpread = AccelerationSpread = Vector2.Zero;
            DefaultParticleLife = theParticleLife;
            SpawnDensity = theSpawnDensity;
            mParticles = new List<Particle>();
        }

        /// <summary>
        /// Create a new particle generator with randomness in particle spawning factors
        /// Each provided spread argument causes new particles to be spawned with 
        /// A random number between 0 and theSpread added to the corresponding parameter
        /// </summary>
        /// <param name="thePosition"></param>
        /// <param name="thePositionSpread"></param>
        /// <param name="theVelocity"></param>
        /// <param name="theVelocitySpread"></param>
        /// <param name="theAcceleration"></param>
        /// <param name="theAccelerationSpread"></param>
        public ParticleEffect(Vector2 thePosition, Vector2 thePositionSpread, Vector2 theVelocity, Vector2 theVelocitySpread,
            Vector2 theAcceleration,Vector2 theAccelerationSpread, int theParticleLife, int theParticleLifeSpread, int theSpawnDensity = 1)
            :this(thePosition, theVelocity, theAcceleration, theParticleLife, theSpawnDensity)
        {
            PositionSpread = thePositionSpread;
            VelocitySpread = theVelocitySpread;
            AccelerationSpread = theAccelerationSpread;
            ParticleLifeSpread = theParticleLifeSpread;
        }

        public void Spawn()
        {
            Vector2 pos = addRandomSpread(SourcePosition, PositionSpread);
            Vector2 vel = addRandomSpread(BaseVelocity, VelocitySpread);
            Vector2 accel = addRandomSpread(BaseAcceleration, AccelerationSpread);
            int life = addRandomSpread(DefaultParticleLife, ParticleLifeSpread);
            mParticles.Add(new Particle(life, pos, vel, accel));
        }

        public void Update(GameTime theGameTime)
        {
            //Traverse in reverse to allow removal
            for(int i = mParticles.Count - 1 ; i >= 0 ; i--)
            {
                if (mParticles[i].LifeTime <= 0)
                    mParticles.Remove(mParticles[i]);
                else
                    mParticles[i].Update(theGameTime);
            }
        }

        public void Update(GameTime theGameTime, Vector2 newPosition)
        {
            SourcePosition = newPosition;
            this.Update(theGameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle particle in mParticles)
                particle.Draw(sb);
        }

        private Vector2 addRandomSpread(Vector2 baseVector, Vector2 spreadVector)
        {
            float x = baseVector.X + (spreadVector.X - (float)Rand.NextDouble() * spreadVector.X * 2);
            float y = baseVector.Y + (spreadVector.Y - (float)Rand.NextDouble() * spreadVector.Y * 2);
            return new Vector2(x, y);
        }

        private int addRandomSpread(int baseInt, int spread)
        {
            return baseInt - Rand.Next(spread) * 2;
        }

    }
}
