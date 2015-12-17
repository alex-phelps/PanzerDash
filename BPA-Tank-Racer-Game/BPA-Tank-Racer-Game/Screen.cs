using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Base class for screens / gamestates
    /// </summary>
    public class Screen
    {
        protected EventHandler screenEvent;

        public Screen(EventHandler screenEvent)
        {
            this.screenEvent = screenEvent;
        }

        public virtual void Update(GameTime gametime)
        {
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
        }
    }
}
