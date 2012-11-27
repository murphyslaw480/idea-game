using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    /// <summary>
    /// A specialization of Sprite with physics properties
    /// Uses Velocity, Acceleration, and mass to respond to forces for movement
    /// </summary>
    class PhysicalSprite : Sprite
    {
        //how long (seconds) it takes black hole to eat sprite
        private const float TOTAL_TIME_TO_DESTROY_SPRITE = 0.5f;
        //change in position per second (px/s)
        private Vector2 _velocity = Vector2.Zero;
        public Vector2 Velocity 
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        //change in velocity (px / s^2)
        private Vector2 _acceleration = Vector2.Zero;
        //Mass affects how an object responds to forces (a = F/m)
        public readonly float Mass;
        //an object in motion remains in motion. Unless its a videogame, like this
        private readonly float naturalDeceleration;
        //magnitude of _velocity cannot exceed maxSpeed
        private readonly float maxSpeed;
        //max speed of a sprite after it is spit out of a black hole
        private const float maxSpriteProjectileSpeed = 40.0f;
        //for being eaten by black hole:
        protected readonly float originalScale;
        private float timeUntilDestroyed = TOTAL_TIME_TO_DESTROY_SPRITE;

        public enum LifeState
        {
            Living,
            BeingEatenByBlackHole,
            Destroyed,
            Projectile
        };
        public LifeState SpriteLifeState;

        //used to indicate what direction the sprite is facing
        public enum Direction
        {
            North,
            East,
            South,
            West
        };
        //store a texture for each direction sprite can face
        //order: [NORTH, EAST, SOUTH, WEST]
        private Texture2D[] directionalTextures;

        public PhysicalSprite(float mass, float originalScale)
            :base()
        {
            Mass = mass;
            this.originalScale = originalScale;
            naturalDeceleration = 0.0f;
            //create space to store one texture for each direction sprite can face
            directionalTextures = new Texture2D[Enum.GetValues(typeof(Direction)).Length];
        }

        public PhysicalSprite(float mass, float originalScale, float theNaturalDeceleration, float theMaxSpeed)
            :this(mass, originalScale)
        {
            naturalDeceleration = theNaturalDeceleration;
            maxSpeed = theMaxSpeed;
        }

        /// <summary>
        /// Load all four directional texures based on theAssetName
        /// For PhysicalSprite named 'mySprite', textures should be named:
        /// mySprite_north, mySprite_east, mySprite_south, mySprite_west
        /// </summary>
        /// <param name="theContentManager"></param>
        /// <param name="theAssetName"></param>
        public new void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            shade = Color.White;
            AssetName = theAssetName;
            directionalTextures[(int)Direction.North] = theContentManager.Load<Texture2D>(theAssetName + "_north");
            directionalTextures[(int)Direction.East] = theContentManager.Load<Texture2D>(theAssetName + "_east");
            directionalTextures[(int)Direction.South] = theContentManager.Load<Texture2D>(theAssetName + "_south");
            directionalTextures[(int)Direction.West] = theContentManager.Load<Texture2D>(theAssetName + "_west");
            //mSpriteTexture = directionalTextures[(int)Direction.North];
            mSpriteTexture = directionalTextures[(int)Direction.North];
            Scale = originalScale;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        public void applyForce(Vector2 theForce)
        {
            _acceleration += theForce / Mass;
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics)
        {
            switch (SpriteLifeState)
            {
                case (LifeState.Living):
                    {
                        _velocity += _acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
                        controlVelocity(maxSpeed);
                        //apply velocity to Sprite Update method - which will also check screen bounds
                        base.Update(theGameTime, _velocity, new Vector2(1, 1), theGraphics);
                        _acceleration = Vector2.Zero;
                        break;
                    }
                case (LifeState.BeingEatenByBlackHole):
                    {
                        timeUntilDestroyed -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
                        Scale = originalScale * (timeUntilDestroyed) / TOTAL_TIME_TO_DESTROY_SPRITE;
                        angle += 1.0f;
                        shade.A = (byte)((timeUntilDestroyed / TOTAL_TIME_TO_DESTROY_SPRITE) * 200);
                        if (timeUntilDestroyed <= 0.0)
                        {
                            //note - these values should be restored when spit back out of black hole
                            shade.A = 0;
                            SpriteLifeState = LifeState.Destroyed;
                        }
                        break;
                    }
                case (LifeState.Projectile):
                    {
                        _velocity += _acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
                        float speed = _velocity.Length();
                        if (speed > maxSpriteProjectileSpeed)
                            _velocity = _velocity * maxSpriteProjectileSpeed / speed;
                        Position += _velocity;
                        _acceleration = Vector2.Zero;
                        angle += 0.1f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    }

            }
        }

        public void Reset()
        {
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
            shade.A = 255;
            Scale = originalScale;
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics, Gravity theGravity)
        {
            applyGravity(theGravity, theGameTime);
            Update(theGameTime, theGraphics);
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics, Gravity theGravity, Vector2 theFocusPoint )
        {
            lookAtThis(theFocusPoint);
            Update(theGameTime, theGraphics, theGravity);
        }

        private void applyGravity(Gravity gravity, GameTime theGameTime)
        {
            Vector2 direction = gravity.Position - Position;
            float distance = direction.Length();
            direction.Normalize();
            _acceleration += gravity.Magnitude * direction * (float)theGameTime.ElapsedGameTime.TotalSeconds / (distance * 0.01f);
        }

        /// <summary>
        /// apply natural deceleration to gradually reduce the object's velocity 
        /// and limit velocity from exceeding maxSpeed
        /// </summary>
        private void controlVelocity(float maximumSpeed)
        {
            float speed = _velocity.Length();
            if ((int)speed > 0 && !((int)_acceleration.Length() > 0))
            {
                float xRatio = _velocity.X / speed;
                float yRatio = _velocity.Y / speed;
                Vector2 deceleration = new Vector2(xRatio * naturalDeceleration, yRatio * naturalDeceleration);
                _velocity -= deceleration;
            }
            //scale down velocity if travelling over maxSpeed
            if (speed > maximumSpeed)
                _velocity = _velocity * maximumSpeed / speed;
        }

        /// <summary>
        /// Find out which direction the sprite should face to look at the given focus point
        /// </summary>
        /// <param name="focusPoint">Point the sprite should look at</param>
        /// <returns></returns>
        private Direction getDirection(Vector2 focusPoint)
        {
            Vector2 difference = focusPoint - Center;
            float angle = (float)Math.Atan2(difference.X, -difference.Y);
            if (angle > -Math.PI / 4 && angle < Math.PI / 4)
                return Direction.North;
            else if (angle >= Math.PI / 4 && angle < 3 * Math.PI / 4)
                return Direction.East;
            else if (angle > 3 * Math.PI / 4 || angle < -3 * Math.PI / 4)
                return Direction.South;
            else
                return Direction.West;
        }

        /// <summary>
        /// Change sprite texture based on direction faced
        /// </summary>
        /// <param name="focusPoint">Point to look at</param>
        protected void lookAtThis(Vector2 focusPoint)
        {
            Direction direction = getDirection(focusPoint);
            mSpriteTexture = directionalTextures[(int)direction];
        }

    }
}
