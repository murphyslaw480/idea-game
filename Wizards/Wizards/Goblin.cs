using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Wizards
{
    class Goblin : MeleeEnemy
    {
        const float GOBLIN_MASS = 20.0f;
        const float GOBLIN_DECELERATION = 5.0f; 
        const float GOBLIN_MAX_SPEED = 300.0f;
        const float GOBLIN_MOVEMENT_FORCE = 15000.0f;
        const float GOBLIN_SCALE = 0.2f;
        const string GOBLIN_ASSETNAME = "goblin";

        public Goblin(Vector2 startPosition)
            :base(GOBLIN_MASS, GOBLIN_DECELERATION, GOBLIN_MAX_SPEED, GOBLIN_MOVEMENT_FORCE)
        {
            Position = startPosition;
        } 
        
        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, GOBLIN_ASSETNAME);
            Scale = GOBLIN_SCALE;
        }
    }
}
