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
        private BulletHandler bulletHandler;
        private Texture2D cooldownBar;

        public GameScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            bulletHandler = new BulletHandler();

            playerTank = new PlayerTank(content, bulletHandler, TankPartType.urban, TankPartType.urban);

            background = new Background(content.Load<Texture2D>("TempBackground"));
            background.position = new Vector2(250, -250);

            cooldownBar = content.Load<Texture2D>("CooldownBar");
        }

        public override void Update(GameTime gametime)
        {
            playerTank.Update(gametime);
            bulletHandler.Update(gametime);

            //Move objects based on the player's tank's movement
            background.position -= playerTank.velocity;
            bulletHandler.MoveBullets(-playerTank.velocity);

            //Check if player is colliding with the color black in the background
            if (Game1.IntersectColor(playerTank, background, new Color(0, 0, 0))) 
            {
                background.position += playerTank.velocity;
                playerTank.speed = 0;
            }

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            background.Draw(spritebatch);
            bulletHandler.Draw(spritebatch);
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
