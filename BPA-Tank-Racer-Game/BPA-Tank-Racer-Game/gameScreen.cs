using System;
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

            playerTank.Update(gametime);
            bulletHandler.Update(gametime);

            //If player is not stunned, allow it to move
            if (!playerTank.isStunned)
            {
                //Move objects based on the player's tank's movement
                background.position -= playerTank.velocity;
                bulletHandler.MoveBullets(-playerTank.velocity);
                enemyTank.position -= playerTank.velocity;
            }

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

            //Check if player is colliding with the enemy tank
            //Since we have yet to update the enemy, if this is true we know the player caused the collision
            if (Game1.IntersectPixels(playerTank, enemyTank))
            {
                //Undo tank movement
                background.position += playerTank.velocity;
                bulletHandler.MoveBullets(playerTank.velocity);
                enemyTank.position += playerTank.velocity;

                //Redo tank movement on X axis
                background.position.X -= playerTank.velocity.X;
                bulletHandler.MoveBulletsX(-playerTank.velocity.X);
                enemyTank.position.X -= playerTank.velocity.X;

                //If colliding, collision comes from either X direction or both directions
                //If it was not colliding, then collision comes from Y direction or both directions
                if (Game1.IntersectPixels(playerTank, enemyTank))
                {
                    //Undo X movement
                    background.position.X += playerTank.velocity.X;
                    bulletHandler.MoveBulletsX(playerTank.velocity.X);
                    enemyTank.position.X += playerTank.velocity.X;

                    //Redo tank movement on Y axis
                    background.position.Y -= playerTank.velocity.Y;
                    bulletHandler.MoveBulletsY(-playerTank.velocity.Y);
                    enemyTank.position.Y -= playerTank.velocity.Y;

                    //If colliding, collision comes from both directions, and movement from both directions is reversed
                    //If not colliding, then undo all X direction movement but keep Y movement
                    if (Game1.IntersectPixels(playerTank, enemyTank))
                    {
                        //Undo tank movement on Y axis since colliding from both direction
                        background.position.Y += playerTank.velocity.Y;
                        bulletHandler.MoveBulletsY(playerTank.velocity.Y);
                        enemyTank.position.Y += playerTank.velocity.Y;
                    }
                }
                else //Collision from Y or both directions
                {
                    //Undo X movement
                    background.position.X += playerTank.velocity.X;
                    bulletHandler.MoveBulletsX(playerTank.velocity.X);
                    enemyTank.position.X += playerTank.velocity.X;

                    //Redo Y movement
                    background.position.Y -= playerTank.velocity.Y;
                    bulletHandler.MoveBulletsY(-playerTank.velocity.Y);
                    enemyTank.position.Y -= playerTank.velocity.Y;

                    //If colliding, collision comes from Y only
                    //If not colliding, the collision comes from both directions when combined only
                    if (Game1.IntersectPixels(playerTank, enemyTank))
                    {
                        //Undo Y (Cause of collision)
                        background.position.Y += playerTank.velocity.Y;
                        bulletHandler.MoveBulletsY(playerTank.velocity.Y);
                        enemyTank.position.Y += playerTank.velocity.Y;
                        //Redo X (Not cause of collision)
                        background.position.X -= playerTank.velocity.X;
                        bulletHandler.MoveBulletsX(-playerTank.velocity.X);
                        enemyTank.position.X -= playerTank.velocity.X;
                    }
                    else //Collision from both sides when combined only
                    {
                        //Undo Y movement
                        background.position.Y += playerTank.velocity.Y;
                        bulletHandler.MoveBulletsY(playerTank.velocity.Y);
                        enemyTank.position.Y += playerTank.velocity.Y;
                    }
                }

                //Either way, collision is enemy's fault, so reset his movement and rotation speeds

                //Undo enemy's rotation
                playerTank.rotation -= playerTank.rotSpeed;

                //Set enemy's speed variables to 0;
                playerTank.speed = 0;
                playerTank.rotSpeed = 0;
            }

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
            //enemyTank.ShootAi(playerTank);

            //Check if player is colliding with the enemy tank
            //Since we have checked if the player has caused any collision (and dealt with it) we know the enemy is causing a collision here
            if (Game1.IntersectPixels(playerTank, enemyTank))
            {
                //Undo tank's movement
                enemyTank.position -= enemyTank.velocity;

                //Redo tank movement on X axis
                enemyTank.position.X += enemyTank.velocity.X;

                //If colliding, collision comes from either X direction or both directions
                //If it was not colliding, then collision comes from Y direction or both directions
                if (Game1.IntersectPixels(playerTank, enemyTank))
                {
                    //Undo X movement
                    enemyTank.position.X -= enemyTank.velocity.X;

                    //Redo tank movement on Y axis
                    enemyTank.position.Y += enemyTank.velocity.Y;

                    //If colliding, collision comes from both directions, and movement from both directions is reversed
                    //If not colliding, then undo all X direction movement but keep Y movement
                    if (Game1.IntersectPixels(playerTank, enemyTank))
                    {
                        //Undo tank movement on Y axis since colliding from both direction
                        enemyTank.position.Y -= enemyTank.velocity.Y;
                    }
                }
                else //Collision from Y or both directions
                {
                    //Undo X movement
                    enemyTank.position.X -= enemyTank.velocity.X;

                    //Redo Y movement
                    enemyTank.position.Y += enemyTank.velocity.Y;

                    //If colliding, collision comes from Y only
                    //If not colliding, the collision comes from both directions when combined only
                    if (Game1.IntersectPixels(playerTank, enemyTank))
                    {
                        //Undo Y (Cause of collision)
                        enemyTank.position.Y -= enemyTank.velocity.Y;
                        //Redo X (Not cause of collision)
                        enemyTank.position.X += enemyTank.velocity.X;
                    }
                    else //Collision from both sides when combined only
                    {
                        //Undo Y movement
                        enemyTank.position.Y -= enemyTank.velocity.Y;
                    }
                }

                //Either way, collision is enemy's fault, so reset his movement and rotation speeds

                //Undo enemy's rotation
                enemyTank.rotation -= enemyTank.rotSpeed;

                //Set enemy's speed variables to 0;
                enemyTank.speed = 0;
                enemyTank.rotSpeed = 0;
            }

            foreach (Bullet bullet in bulletHandler.bullets)
            {
                if (bullet.active && Game1.IntersectColor(bullet, background, new Color(0, 0, 0)))
                {
                    bulletHandler.Destroy(bullet);
                }

                if (bullet.active && bullet.ownerTank != playerTank && Game1.IntersectPixels(bullet, playerTank))
                {
                    playerTank.stunLength = bullet.damage;
                    bulletHandler.Destroy(bullet);
                }

                if (bullet.active && bullet.ownerTank != enemyTank && Game1.IntersectPixels(bullet, enemyTank))
                {
                    enemyTank.stunLength = bullet.damage;
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
