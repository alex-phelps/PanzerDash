using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Game state for the main game
    /// </summary>
    public class GameScreen : Screen
    {
        private PlayerTank playerTank;
        private AITank enemyTank;
        private Background background;
        private BulletHandler bulletHandler;
        private Texture2D cooldownBar;

        public GameScreen(ContentManager content, EventHandler screenEvent, int level)
            : base(screenEvent)
        {
            cooldownBar = content.Load<Texture2D>("CooldownBar");
            bulletHandler = new BulletHandler();

            Vector2 levelSize = new Vector2(3200, 3200);
            Vector2 startPosInImage;

            //Create player
            playerTank = new PlayerTank(content, bulletHandler, TankPartType.red, TankPartType.red);

            //Create Enemy
            enemyTank = new AITank(content, bulletHandler, TankPartType.basic,
                TankPartType.basic, new Vector2(Game1.WindowWidth / 2 + 100, Game1.WindowHeight / 2));

            if (level == 2) // Level 2
            {
                startPosInImage = new Vector2(0, 0); // Temp
            }
            else // Level 1
            {
                background = new Background(content.Load<Texture2D>("Level1"));
                startPosInImage = new Vector2(654, 2478);
            }

            background.position.X = (levelSize.X / 2 - startPosInImage.X) + Game1.WindowWidth / 2;
            background.position.Y = (levelSize.Y / 2 - startPosInImage.Y) + Game1.WindowHeight / 2;

        }

        public override void Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                screenEvent.Invoke(this, new EventArgs());

            //Update enemy AI tank
            if (!enemyTank.SteerAi(background) && !enemyTank.SteerAi(playerTank))
            {
                if (enemyTank.rotSpeed < -0.05f) // Not 0 here to fix any rounding errors
                    enemyTank.rotSpeed += 0.03f;
                else if (enemyTank.rotSpeed > 0.05f) //Not 0 here to fix any rounding errors
                    enemyTank.rotSpeed -= 0.03f;
                else enemyTank.rotSpeed = 0;
            }
            enemyTank.Update(gametime);

            playerTank.Update(gametime);
            bulletHandler.Update(gametime);

            //Move objects based on the player's tank's movement
            background.position -= playerTank.velocity;
            bulletHandler.MoveBullets(-playerTank.velocity);
            enemyTank.position -= playerTank.velocity;

            //Check if player is colliding with the color black in the background
            if (Game1.IntersectColor(playerTank, background, new Color(0, 0, 0))) 
            {
                //Undo tank movement
                background.position += playerTank.velocity;
                bulletHandler.MoveBullets(playerTank.velocity);
                enemyTank.position += playerTank.velocity;

                //Undo player's rotation
                playerTank.rotation -= playerTank.rotSpeed;

                //Set player's speed variables to 0;
                playerTank.speed = 0;
                playerTank.rotSpeed = 0;
            }

            foreach (Bullet bullet in bulletHandler.bullets)
            {
                if (Game1.IntersectColor(bullet, background, new Color(0, 0, 0)))
                {
                    bulletHandler.Destroy(bullet);
                }
            }

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            background.Draw(spritebatch);
            enemyTank.Draw(spritebatch);
            playerTank.Draw(spritebatch);
            bulletHandler.Draw(spritebatch);

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
