using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wizards
{
    class ExhaustParticle : Particle
    {

        public void Update(GameTime theGameTime)
        {
            //exhaust particles will
            Scale += 0.1f;
            base.Update(theGameTime);
        }
    }
}
