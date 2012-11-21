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
        private float capacityLeft;
        //how far a sprite must be from black hole to be consumed
        private const float eatDistance = 080.0f;
        static Vector2 defaultParticleVelocity = new Vector2(0.0f,0);
        static Vector2 defaultParticleAcceleration = new Vector2(0, 0);
        static Vector2 defaultPositionSpread = new Vector2(40, 40);
        static Vector2 defaultVelocitySpread = new Vector2(0, 0);
        static Vector2 defaultAccelerationSpread = new Vector2(0, 0);

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
            gravity = new Gravity(thePosition, theGravity);
            capacityLeft = theCapacity;
        }
        
        public override void Update(GameTime theGameTime)
        {
            //auto-spawn new particles
            Spawn();

            base.Update(theGameTime);
        }

        public override void Update(GameTime theGameTime, Vector2 newPosition)
        {
            SourcePosition = newPosition;
            this.Update(theGameTime);
        }

        public override void Spawn()
        {
            //spawn a number of particles determined by spawndensity
            for (int i = 0; i < SpawnDensity; i++)
            {
                base.AddNewParticle(new BlackHoleParticle(SourcePosition));
                //base.AddNewParticle(new BlackHoleParticle(SourcePosition), SourcePosition, BaseVelocity, BaseAcceleration); DELETE
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
