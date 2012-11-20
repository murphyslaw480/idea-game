using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Wizards
{
    /// <summary>
    /// Melee enemies seek out and accelerate towards the player
    /// </summary>
    class MeleeEnemy : PhysicalSprite
    {
        protected readonly float MovementForce;

        public MeleeEnemy(float theMass, float theDeceleration, float theMaxSpeed, float theMovementForce)
            :base(theMass, theDeceleration, theMaxSpeed)
        {
            MovementForce = theMovementForce;
        } 

        public override void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics, Vector2 playerPosition, Gravity theGravity)
        {
            applyForce(MovementForce * directionTo(playerPosition));
            base.Update(theGameTime, theGraphics, playerPosition, theGravity);
        }

        private Vector2 directionTo(Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - Center;
            direction.Normalize();
            return direction;
        }
    }
}
