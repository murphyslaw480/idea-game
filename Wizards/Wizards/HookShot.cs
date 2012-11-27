using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Wizards
{
    class HookShot
    {
        //spacing between chain links to draw between handle and hook
        private const float DISTANCE_PER_LINK = 30.0f; 
        //speed with which hook moves
        private const float SPEED = 15.0f;
        //magnitute of force with which hook pulls
        private const float HOOK_FORCE = 10000.0f;

        //position of handle and hook
        private Vector2 _handlePosition;
        private Vector2 _hookPosition;
        private Vector2 _direction;

        //sprite holding the hookshot
        private PhysicalSprite _holderSprite;
        //sprite hooked by the hookshot
        private PhysicalSprite _hookedSprite;

        //texture for hook (end part that attaches to target)
        private Texture2D _hookTexture;
        //texture for links (drawn between handle and hook)
        private Texture2D _linkTexture;

        public enum State
        {
            Idle,
            Fired,
            Connected,
            Pulling
        }

        private State _hookState;

        public State HookState
        {
            get { return _hookState; }
            private set
            {
                if (value == State.Idle)
                {
                    _hookedSprite = null;
                    _hookPosition = _holderSprite.Center;
                }
                _hookState = value;
            }
        }

        public PhysicalSprite HookedSprite
        {
            get { return _hookedSprite; }
            set
            {
                if (_hookState == State.Fired)
                {
                    _hookedSprite = value;
                }
            }
        }

        /// <summary>
        /// Create a new hookshot
        /// </summary>
        /// <param name="ownerSprite">Sprite that holds the hookshot</param>
        public HookShot(PhysicalSprite ownerSprite)
        {
            _holderSprite = ownerSprite;
            _hookPosition = ownerSprite.Center;
            _handlePosition = ownerSprite.Center;
        }

        public void Update(GameTime theGameTime, GraphicsDevice theGraphics)
        {
            //always keep handle with holder
            _handlePosition = _holderSprite.Center;
            switch(_hookState)
            {
                case State.Idle:
                    {
                        _hookPosition = _holderSprite.Center;
                        break;
                    }
                case State.Fired:
                    {
                        _hookPosition += _direction * SPEED;
                        if (outOfBounds(theGraphics))        //reset if out of bounds
                        {
                            HookState = State.Idle;
                        }
                        break;
                    }
                case State.Connected:
                    {
                        if (_hookedSprite.SpriteLifeState == PhysicalSprite.LifeState.Living)
                        {
                            //keep hook position at sprite
                            //MODIFY: keep track of location where hook originally hit sprite
                            _hookPosition = _hookedSprite.Center;
                            setDirection();
                        }
                        else
                        {   //if hooked sprite is no longer living, reset
                            HookState = State.Idle;
                        }
                        break;
                    }
                case State.Pulling:
                    {
                        _hookPosition = _hookedSprite.Center;
                        setDirection();
                        _hookedSprite.applyForce(-_direction * HOOK_FORCE);
                        _holderSprite.applyForce(_direction * HOOK_FORCE);
                        break;
                    }

            }
        }

        private bool outOfBounds(GraphicsDevice graphics)
        {
            if (_hookPosition.X < 0
                || _hookPosition.Y < 0
                || (_hookPosition.X > graphics.Viewport.Width)
                || (_hookPosition.Y > graphics.Viewport.Height))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// set _direction based on hook and handle positions
        /// </summary>
        private void setDirection()
        {
            //TODO: Add helper method to get direction from vectors
            _direction = Vector2.Normalize(_hookPosition - _handlePosition);
        }

        /// <summary>
        /// Call whenever the button mapped to the hookshot is pressed
        /// If idle, the hookshot will fire
        /// If connected, the hookshot will begin pulling
        /// If Pulling, the hookshot will detach and go to idle
        /// </summary>
        /// <param name="target"></param>
        public void Trigger(Vector2 target)
        {
            switch (_hookState)
            {
                case State.Idle:
                    {
                        _direction = Vector2.Normalize(target - _handlePosition);
                        HookState = State.Fired;
                        break;
                    }
                case State.Connected:
                    {
                        HookState = State.Pulling;
                        break;
                    }
                case State.Pulling:
                    {
                        HookState = State.Idle;
                        break;
                    }
            }
        }

        public void CheckCollision(PhysicalSprite sprite)
        {
            //only check collisions if fired
            if (_hookState != State.Fired)
            {
                return;
            }

                if ( sprite.SpriteLifeState == PhysicalSprite.LifeState.Living
                    && _hookPosition.Y >= sprite.Top 
                    && _hookPosition.Y <= sprite.Bottom 
                    && _hookPosition.X <= sprite.RightSide 
                    && _hookPosition.X >= sprite.LeftSide
                    ) 
                {
                    _hookedSprite = sprite;
                    HookState = State.Connected;
                }
        }

        public void LoadContent(ContentManager content)
        {
            _hookTexture = content.Load<Texture2D>("hook_claw");
            _linkTexture = content.Load<Texture2D>("hook_link");
        }

        public void Draw(SpriteBatch sb)
        {
            if (_hookState != State.Idle)
            {
                sb.Draw(_hookTexture, _hookPosition, Color.White);
                Vector2 nextLinkPosition = _handlePosition;
                Vector2 chainDirection = Vector2.Normalize(_hookPosition - _handlePosition);
                int numLinksToDraw = (int)((_hookPosition - _handlePosition).Length() / DISTANCE_PER_LINK);
                for (int i = 0; i < numLinksToDraw; i++)
                {
                    nextLinkPosition += chainDirection * DISTANCE_PER_LINK;
                    sb.Draw(_linkTexture, nextLinkPosition, Color.White);
                }
            }
        }

    }
}
