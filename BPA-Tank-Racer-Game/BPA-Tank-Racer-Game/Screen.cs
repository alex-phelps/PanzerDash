using System;
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gametime)
        {
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spritebatch">Spritebatch object to draw objects with</param>
        public virtual void Draw(SpriteBatch spritebatch)
        {
        }
    }
}
