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
        private const float DISTANCE_PER_LINK = 10.0f; 
        //speed with which hook moves
        private const float SPEED = 50.0f;
        //magnitute of force with which hook pulls
        private const float HOOK_FORCE = 200.0f;

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

        public void Update(GameTime theGameTime)
        {
            //always keep handle with holder
            _handlePosition = _holderSprite.Center;
            switch(_hookState)
            {
                case State.Fired:
                    {
                        _hookPosition += _direction * SPEED;
                        break;
                    }
                case State.Connected:
                    {
                        //keep hook position at sprite
                        //MODIFY: keep track of location where hook originally hit sprite
                        _hookPosition = _hookedSprite.Center;
                        setDirection();
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
                        _hookState = State.Fired;
                        break;
                    }
                case State.Connected:
                    {
                        _hookState = State.Pulling;
                        break;
                    }
                case State.Pulling:
                    {
                        _hookPosition = _handlePosition;
                        _hookState = State.Idle;
                        break;
                    }
            }
        }

        public void CheckCollision(List<PhysicalSprite> sprites)
        {
            //only check collisions if fired
            if (_hookState != State.Connected)
            {
                return;
            }

            foreach(PhysicalSprite p in sprites)
            {
                if (_hookPosition.Y > p.Top && _hookPosition.Y <= p.Bottom && _hookPosition.X > p.LeftSide && _hookPosition.X > p.RightSide) 
                {
                    _hookedSprite = p;
                    _hookState = State.Connected;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (_hookState != State.Idle)
            {
                sb.Draw(_hookTexture, _hookPosition, Color.White);
                Vector2 nextLinkPosition = _handlePosition;
                int numLinksToDraw = (int)((_hookPosition - _handlePosition).Length() / DISTANCE_PER_LINK);
                for (int i = 0; i < numLinksToDraw; i++)
                {
                    nextLinkPosition += _direction * DISTANCE_PER_LINK;
                    sb.Draw(_linkTexture, nextLinkPosition, Color.White);
                }
            }
        }

    }
}
