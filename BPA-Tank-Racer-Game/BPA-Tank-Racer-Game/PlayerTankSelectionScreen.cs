using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PanzerDash
{
    /// <summary>
    /// Game state for the player's tank selection screen
    /// </summary>
    public class PlayerTankSelectionScreen : TankSelectionScreen
    {
        public Texture2D logo;

        public PlayerTankSelectionScreen(ContentManager content, EventHandler screenEvent, bool isCareerMode) 
            : base(content, screenEvent, isCareerMode)
        {
            logo = content.Load<Texture2D>("PlayerTankSelectionLogo");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(logo, new Vector2(Game1.WindowWidth / 2, 25), new Rectangle(0, 0,
                logo.Width, logo.Height), Color.White, 0, new Vector2(logo.Width / 2, logo.Height / 2),
                1, SpriteEffects.None, 1f);

            base.Draw(spritebatch);
        }
    }
}
