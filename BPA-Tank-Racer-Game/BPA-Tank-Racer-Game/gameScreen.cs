using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Game state for the main game
    /// </summary>
    public class GameScreen : Screen
    {
        private PlayerTank playerTank;
        private Background background;

        private Texture2D cooldownBar;

        public GameScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            playerTank = new PlayerTank(content, TankBaseType.basic, TankGunType.basic);

            background = new Background(content.Load<Texture2D>("TempBackground"));

            cooldownBar = content.Load<Texture2D>("CooldownBar");
        }

        public override void Update(GameTime gametime)
        {
            playerTank.Update(gametime);
            background.position -= playerTank.velocity;

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            background.Draw(spritebatch);
            playerTank.Draw(spritebatch);

            //Draw HUD

            //Create a rectangle representing how much of the bar should be shown
            Rectangle cooldownSource = new Rectangle(0, 0, cooldownBar.Width,
                (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown)));
            //Draw cooldownBar
            spritebatch.Draw(cooldownBar, new Vector2(30, (Game1.WindowHeight / 2) + (cooldownBar.Height / 2) +
                cooldownBar.Height - (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown))),
                cooldownSource, Color.White, 0, new Vector2(cooldownBar.Width / 2, cooldownBar.Height / 2), 1, SpriteEffects.None, 1);

            base.Draw(spritebatch);
        }
    }
}
