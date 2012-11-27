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
        static Random rand;
        private Gravity gravity;
        public Gravity Gravity
        {
            get { return gravity; }
        }

        //Default particle spawning variance parameters -- play around till it looks right
        private const float defaultParticleLife = 0.6f;
        private const float defaultParticleLifeSpread = 0.08f;
        private const int defaultSpawnDensity = 6;
        private float capacityLeft;
        //how far a sprite must be from black hole to be consumed
        private const float eatDistance = 50.0f;
        static Vector2 defaultParticleVelocity = new Vector2(0.0f,0.0f);
        static Vector2 defaultParticleAcceleration = new Vector2(0, 0);
        static Vector2 defaultPositionSpread = new Vector2(40, 40);
        static Vector2 defaultVelocitySpread = new Vector2(10, 10);
        static Vector2 defaultAccelerationSpread = new Vector2(0, 0);

        //particle attributes
        //how much to expand particle each frame
        private const float scalePerMilliSecond = -0.07f; 
        //how much to rotate particle each frame
        private const float rotatePerMilliSecond = 0.001f; 
        //what size to start paticles at
        private const float startingScale = 30.0f;

        //exploding particle attributes
        //how much to expand particle each frame
        private const float scalePerMilliSecondExploding = 0.15f; 
        //how much to rotate particle each frame
        private const float rotatePerMilliSecondExploding = -0.001f; 
        //what size to start paticles at
        private const float startingScaleExploding = 0.1f;

        //store eaten sprites to spit back out when exploding
        private List<PhysicalSprite> consumedSprites = new List<PhysicalSprite>();
        //time between spitting back out successive sprites
        private const float spitTime = 0.5f;
        private const float spitTimeVariance = 0.1f;
        private float tillNextSpit;
        private PhysicalSprite spriteToSpit;

        public PhysicalSprite SpriteToSpit
        {
            get 
            {
                PhysicalSprite p = spriteToSpit;
                spriteToSpit = null;
                return p;
            }
        }

        public enum State
        {
            Sucking,
            Exploding
        }

        private State state;

        public State BlackHoleState
        {
            get { return state; }
        }
       
        /// <summary>
        /// Create a new Black Hole
        /// </summary>
        /// <param name="theGravity">float magnitude of gravitational pull</param>
        /// <param name="thePosition">Center Position</param>
        /// <param name="theCapacity">Amount of mass units it can absorb before collapsing</param>
        public BlackHole(float theGravity, Vector2 thePosition, float theCapacity)
            : base(thePosition, defaultPositionSpread,
                   defaultParticleVelocity, defaultVelocitySpread,
                   defaultParticleAcceleration, defaultAccelerationSpread,
                   defaultParticleLife, defaultParticleLifeSpread,
                   defaultSpawnDensity)
        {
            rand = new Random();
            state = State.Sucking;
            gravity = new Gravity(thePosition, theGravity);
            capacityLeft = theCapacity;
        }
        
        public override void Update(GameTime theGameTime)
        {
            //auto-spawn new particles
            Spawn();

            if (state == State.Sucking && capacityLeft <= 0)
            {
                state = State.Exploding;
                tillNextSpit = spitTime;
                gravity = new Gravity(SourcePosition, -gravity.Magnitude);
            }

            if (state == State.Exploding)
            {
                tillNextSpit -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
                if (tillNextSpit <= 0 && consumedSprites.Count > 0)
                {
                    spriteToSpit = consumedSprites[0];
                    spriteToSpit.SpriteLifeState = PhysicalSprite.LifeState.Projectile;
                    spriteToSpit.Reset();
                    consumedSprites.Remove(spriteToSpit);
                    Vector2 spitDirection = new Vector2((float)rand.NextDouble(), -(float)rand.NextDouble());
                    spriteToSpit.Velocity = spitDirection * (float)(0.5 + rand.NextDouble()) * 40;
                    tillNextSpit = spitTime + spitTimeVariance * (1 - 2 * (float)rand.NextDouble());
                }
            }

            base.Update(theGameTime);
        }

        public override void Update(GameTime theGameTime, Vector2 newPosition)
        {
            SourcePosition = newPosition;
            gravity.UpdatePosition(newPosition);
            this.Update(theGameTime);
        }

        public override void Spawn()
        {
            if (state == State.Sucking)
            {
                //spawn a number of particles determined by spawndensity
                for (int i = 0; i < SpawnDensity; i++)
                {
                    base.AddNewParticle(new BlackHoleParticle(SourcePosition, rotatePerMilliSecond, startingScale, scalePerMilliSecond));
                }
            }
            else if (state == State.Exploding)
            {
                //spawn a number of particles determined by spawndensity
                for (int i = 0; i < SpawnDensity; i++)
                {
                    base.AddNewParticle(new BlackHoleParticle(SourcePosition, rotatePerMilliSecondExploding, startingScaleExploding, scalePerMilliSecondExploding));
                }
            }

        }

        /// <summary>
        /// Try to consume a physical sprite
        /// Only succeeds if sprite is close enough
        /// </summary>
        /// <param name="theSprite"></param>
        public void TryToEat(PhysicalSprite theSprite)
        {
            if ((theSprite.SpriteLifeState == PhysicalSprite.LifeState.Living)
                && ((theSprite.Position - this.SourcePosition).Length() < eatDistance))
                eat(theSprite);
        }

        private void eat(PhysicalSprite theSprite)
        {
            capacityLeft -= theSprite.Mass;
            consumedSprites.Add(theSprite);
            theSprite.SpriteLifeState = PhysicalSprite.LifeState.BeingEatenByBlackHole;
        }

        public new void Draw(SpriteBatch sb)
        {
            foreach (Particle particle in mParticles)
            {
                Vector2 rotationCenter = 0.1f * (SourcePosition - particle.Position);
                particle.Draw(sb, rotationCenter);
            }
        }

    }
}
