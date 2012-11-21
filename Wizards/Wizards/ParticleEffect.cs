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
        protected static Random Rand = new Random();
        protected Vector2 SourcePosition;  //Point from which new particles spawn
        protected Vector2 PositionSpread;  //Randomness in spawn location for each particle
        protected Vector2 BaseVelocity;    //Default velocity for spawned particles
        protected Vector2 VelocitySpread;   //Randomness in velocity for each particle
        protected Vector2 BaseAcceleration;    //Default Acceleration for spawned particles
        protected Vector2 AccelerationSpread;   //Randomness in Acceleration for each particle
        public float DefaultParticleLife, ParticleLifeSpread;     //Default particle lifetime (see particle class)
        protected int SpawnDensity;        //number of particles created per spawn call
        protected List<Particle> mParticles;      //list of living particles

        /// <summary>
        /// Create a new particle generator at the given location
        /// If it is anchored to a moving object, Position should be updated every frame by its owner
        /// </summary>
        /// <param name="thePosition">Source Location for spawned particles</param>
        /// <param name="theVelocity">Velocity for spawned particles</param>
        /// <param name="theAcceleration">Acceleration for spawned particles</param>
        /// <param name="theParticleLife">How long spawned particles will exist</param>
        /// <param name="theSpawnDensity">How many particles to spawn per call</param>
        public ParticleEffect(Vector2 thePosition, Vector2 theVelocity, Vector2 theAcceleration, float theParticleLife, int theSpawnDensity = 1)
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
            Vector2 theAcceleration,Vector2 theAccelerationSpread, float theParticleLife, float theParticleLifeSpread, int theSpawnDensity = 1)
            :this(thePosition, theVelocity, theAcceleration, theParticleLife, theSpawnDensity)
        {
            PositionSpread = thePositionSpread;
            VelocitySpread = theVelocitySpread;
            AccelerationSpread = theAccelerationSpread;
            ParticleLifeSpread = theParticleLifeSpread;
        }

        /// <summary>
        /// Generate a number of new particles equal to the effect's ParticleDensity
        /// Override this in subclasses to add specialized particles
        /// </summary>
        public virtual void Spawn()
        {
            //spawn a number of particles determined by spawndensity
            for (int i = 0; i < SpawnDensity; i++)
            {
                AddNewParticle(new Particle());
            }
        }

        /// <summary>
        /// For internal use within the Spawn method of ParticleEffect and its subclasses
        /// Sets the particle's position, velocity, acceleration, and lifetime
        /// Can pass in a subclass of Particle for more specific behavior
        /// </summary>
        /// <param name="particle">A Particle, or subclass thereof, to adjust the parameters of and add to the particle list</param>
        protected void AddNewParticle(Particle particle)
        {
            //add randomness in positioning, movement, and life
            particle.Position = addRandomSpread(SourcePosition, PositionSpread);
            particle.Velocity = addRandomSpread(BaseVelocity, VelocitySpread);
            particle.Acceleration = addRandomSpread(BaseAcceleration, AccelerationSpread);
            particle.LifeTime = addRandomSpread(DefaultParticleLife, ParticleLifeSpread);
            mParticles.Add(particle);
        }

        /// <summary>
        /// Spawn a particle that travels toward a destination
        /// For use within BlackHole
        /// </summary>
        /// <param name="particle">particle to spawn</param>
        /// <param name="destination">end position of particle</param>
        /// <param name="baseVelocity">velocity to be rotated to point at destination</param>
        /// <param name="baseAcceleration">acceleration to be rotated to point at destination</param>
        protected void AddNewParticle(Particle particle, Vector2 destination, Vector2 baseVelocity, Vector2 baseAcceleration)
        {
            //add randomness in positioning, movement, and life
            particle.Position = addRandomSpread(SourcePosition, PositionSpread);
            float angle = (float)Math.Atan2(destination.Y - particle.Position.Y, destination.X - particle.Position.X);
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            particle.Velocity = Vector2.Zero;
            particle.Acceleration = Vector2.Zero;
            /*particle.Velocity = addRandomSpread(Vector2.Transform(BaseVelocity, rotMatrix), VelocitySpread);
            particle.Acceleration = addRandomSpread(Vector2.Transform(BaseAcceleration, rotMatrix), AccelerationSpread);*/
            particle.LifeTime = addRandomSpread(DefaultParticleLife, ParticleLifeSpread);
            mParticles.Add(particle);
        }

        /// <summary>
        /// For internal use within the Spawn method of ParticleEffect and its subclasses
        /// Sets the particle's position, velocity, acceleration, and lifetime
        /// Velocity and acceleration are rotated by angle degrees
        /// Can pass in a subclass of Particle for more specific behavior
        /// </summary>
        /// <param name="particle">A Particle, or subclass thereof, to adjust the parameters of and add to the particle list</param>
        protected void AddNewParticle(Particle particle, float angle)
        {
            Matrix rotMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle));
            //add randomness in positioning, movement, and life
            particle.Position = addRandomSpread(SourcePosition, PositionSpread);
            particle.Velocity = Vector2.Transform(addRandomSpread(BaseVelocity, VelocitySpread), rotMatrix);
            particle.Acceleration = Vector2.Transform(addRandomSpread(BaseAcceleration, AccelerationSpread), rotMatrix);
            particle.LifeTime = addRandomSpread(DefaultParticleLife, ParticleLifeSpread);
            mParticles.Add(particle);
        }

        /// <summary>
        /// Call Update on each of its particles and remove expired particles
        /// </summary>
        /// <param name="theGameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime theGameTime)
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

        /// <summary>
        /// Call Update on each of its particles and remove expired particles
        /// Also updates position to provided position vector
        /// Use to keep a particle effect anchored to a moving sprite
        /// </summary>
        /// <param name="theGameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime theGameTime, Vector2 newPosition)
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
            float x = baseVector.X + spreadVector.X * (1.0f - 2 * (float)Rand.NextDouble()); 
            float y = baseVector.Y + spreadVector.Y * (1.0f - 2 * (float)Rand.NextDouble());
            return new Vector2(x, y);
        }

        private float addRandomSpread(float baseFloat, float spread)
        {
            return baseFloat + (1.0f - 2 * (float)Rand.NextDouble()) * spread ;
        }

    }
}
