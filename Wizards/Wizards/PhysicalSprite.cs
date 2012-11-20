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
        //change in position per second (px/s)
        private Vector2 _velocity = Vector2.Zero;
        //change in velocity (px / s^2)
        private Vector2 _acceleration = Vector2.Zero;
        //Mass affects how an object responds to forces (a = F/m)
        public readonly float Mass;
        //an object in motion remains in motion. Unless its a videogame, like this
        private readonly float naturalDeceleration;
        //magnitude of _velocity cannot exceed maxSpeed
        private readonly float maxSpeed;
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

        public PhysicalSprite(float mass)
            :base()
        {
            Mass = mass;
            naturalDeceleration = 0.0f;
            //create space to store one texture for each direction sprite can face
            directionalTextures = new Texture2D[Enum.GetValues(typeof(Direction)).Length];
        }

        public PhysicalSprite(float mass, float theNaturalDeceleration, float theMaxSpeed)
            :this(mass)
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
            AssetName = theAssetName;
            directionalTextures[(int)Direction.North] = theContentManager.Load<Texture2D>(theAssetName + "_north");
            directionalTextures[(int)Direction.East] = theContentManager.Load<Texture2D>(theAssetName + "_east");
            directionalTextures[(int)Direction.South] = theContentManager.Load<Texture2D>(theAssetName + "_south");
            directionalTextures[(int)Direction.West] = theContentManager.Load<Texture2D>(theAssetName + "_west");
            //mSpriteTexture = directionalTextures[(int)Direction.North];
            mSpriteTexture = directionalTextures[(int)Direction.North];
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        public void applyForce(Vector2 theForce)
        {
            _acceleration += theForce / Mass;
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics)
        {
            _velocity += _acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            controlVelocity();
            //apply velocity to Sprite Update method - which will also check screen bounds
            base.Update(theGameTime, _velocity, new Vector2(1, 1), theGraphics);
            _acceleration = Vector2.Zero;
        }

        public virtual void Update(GameTime theGameTime, GraphicsDeviceManager theGraphics, Vector2 theFocusPoint)
        {
            lookAtThis(theFocusPoint);
            _velocity += _acceleration * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            controlVelocity();
            //apply velocity to Sprite Update method - which will also check screen bounds
            base.Update(theGameTime, _velocity, new Vector2(1, 1), theGraphics);
            _acceleration = Vector2.Zero;
        }

        /// <summary>
        /// apply natural deceleration to gradually reduce the object's velocity 
        /// and limit velocity from exceeding maxSpeed
        /// </summary>
        private void controlVelocity()
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
            if (speed > maxSpeed)
                _velocity = _velocity * maxSpeed / speed;
        }

        /// <summary>
        /// Find out which direction the sprite should face to look at the given focus point
        /// </summary>
        /// <param name="focusPoint">Point the sprite should look at</param>
        /// <returns></returns>
        private Direction getDirection(Vector2 focusPoint)
        {
            Vector2 difference = focusPoint - Position;
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
