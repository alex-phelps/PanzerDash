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
        private FinishObjective finishObjective;

        private bool gameActive = false;
        private bool firstUpdate = true;

        private SpriteFont countdownFont;
        private TimeSpan countdownOldTime;
        private int countdown = 6;

        public GameScreen(ContentManager content, EventHandler screenEvent, int level)
            : base(screenEvent)
        {
            cooldownBar = content.Load<Texture2D>("CooldownBar");
            countdownFont = content.Load<SpriteFont>("CountdownFont");
            bulletHandler = new BulletHandler();

            Vector2 levelSize = new Vector2(3200, 3200);
            Vector2 startPosInImage;
            Vector2 finishPosInImage;

            //Create player
            playerTank = new PlayerTank(content, bulletHandler, TankPartType.rainbow, TankPartType.snow);

            //Create Enemy
            enemyTank = new AITank(content, bulletHandler, TankPartType.red,
                TankPartType.urban, new Vector2(Game1.WindowWidth / 2 + 100, Game1.WindowHeight / 2));

            if (level == 2) // Level 2
            {
                startPosInImage = new Vector2(0, 0); // Temp
                finishPosInImage = new Vector2(0, 0); // Temp
            }
            else // Level 1
            {
                background = new Background(content.Load<Texture2D>("Level1"));
                startPosInImage = new Vector2(654, 2478);
                finishPosInImage = new Vector2(2143, 1855);
            }

            background.position.X = (levelSize.X / 2 - startPosInImage.X) + Game1.WindowWidth / 2;
            background.position.Y = (levelSize.Y / 2 - startPosInImage.Y) + Game1.WindowHeight / 2;
            
            finishObjective = new FinishObjective(content, new Vector2(
                ((finishPosInImage.X - levelSize.X / 2) + background.position.X),
                ((finishPosInImage.Y - levelSize.Y / 2) + background.position.Y)));
        }

        public override void Update(GameTime gametime)
        {
            if (gameActive || firstUpdate)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    screenEvent.Invoke(this, new EventArgs());

                playerTank.Update(gametime);
                bulletHandler.Update(gametime);

                //If player is not stunned, allow it to move
                if (!playerTank.isStunned)
                {
                    //Move objects based on the player's tank's movement
                    MoveBoard(-playerTank.velocity);
                }

                //Check if player is colliding with the color black in the background
                if (Game1.IntersectColor(playerTank, background, new Color(0, 0, 0)))
                {
                    //Undo tank movement
                    MoveBoard(playerTank.velocity);

                    //Undo player's rotation
                    playerTank.rotation -= playerTank.rotSpeed;

                    //Set player's speed variables to 0;
                    playerTank.speed = 0;
                    playerTank.rotSpeed = 0;
                }

                //Check if player is colliding with the finish objective
                if (Game1.IntersectPixels(playerTank, finishObjective))
                {
                    //Undo tank movement
                    MoveBoard(playerTank.velocity);

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
                    MoveBoard(playerTank.velocity);

                    //Redo tank movement on X axis
                    MoveBoard(-playerTank.velocity.X, 0);

                    //If colliding, collision comes from either X direction or both directions
                    //If it was not colliding, then collision comes from Y direction or both directions
                    if (Game1.IntersectPixels(playerTank, enemyTank))
                    {
                        //Undo X movement
                        MoveBoard(playerTank.velocity.X, 0);

                        //Redo tank movement on Y axis
                        MoveBoard(0, -playerTank.velocity.Y);

                        //If colliding, collision comes from both directions, and movement from both directions is reversed
                        //If not colliding, then undo all X direction movement but keep Y movement
                        if (Game1.IntersectPixels(playerTank, enemyTank))
                        {
                            //Undo tank movement on Y axis since colliding from both direction
                            MoveBoard(0, playerTank.velocity.Y);
                        }
                    }
                    else //Collision from Y or both directions
                    {
                        //Undo X movement
                        MoveBoard(playerTank.velocity.X, 0);

                        //Redo Y movement
                        MoveBoard(0, -playerTank.velocity.Y);

                        //If colliding, collision comes from Y only
                        //If not colliding, the collision comes from both directions when combined only
                        if (Game1.IntersectPixels(playerTank, enemyTank))
                        {
                            //Undo Y (Cause of collision)
                            MoveBoard(0, playerTank.velocity.Y);
                            //Redo X (Not cause of collision)
                            MoveBoard(-playerTank.velocity.X, 0);
                        }
                        else //Collision from both sides when combined only
                        {
                            //Undo Y movement
                            MoveBoard(0, playerTank.velocity.Y);
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

                if (enemyTank.CheckForFinish(finishObjective))
                    enemyTank.ShootAi(finishObjective);
                else enemyTank.ShootAi(playerTank);

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
                    //check for collision with level
                    if (bullet.active && Game1.IntersectColor(bullet, background, new Color(0, 0, 0)))
                    {
                        bulletHandler.Destroy(bullet);
                    }

                    //Check for enemy bullets
                    if (bullet.active && bullet.ownerTank != playerTank)
                    {
                        if (Game1.IntersectPixels(bullet, playerTank))
                        {
                            playerTank.stunLength = bullet.damage;
                            bulletHandler.Destroy(bullet);
                        }
                        else if (Game1.IntersectPixels(bullet, finishObjective))
                        {
                            finishObjective.enemyHealth -= bullet.damage;
                            bulletHandler.Destroy(bullet);
                        }

                    }

                    //Check for player bullets
                    if (bullet.active && bullet.ownerTank != enemyTank)
                    {
                        if (Game1.IntersectPixels(bullet, enemyTank))
                        {
                            enemyTank.stunLength = bullet.damage;
                            bulletHandler.Destroy(bullet);
                        }
                        else if (Game1.IntersectPixels(bullet, finishObjective))
                        {
                            finishObjective.playerHealth -= bullet.damage;
                            bulletHandler.Destroy(bullet);
                        }
                    }
                }

                if (finishObjective.playerHealth <= 0)
                {

                }
                else if (finishObjective.enemyHealth <= 0)
                {

                }

                if (firstUpdate)
                    firstUpdate = false;
            }
            else
            {
                //Before Game; Countdown
                if (countdown <= 0)
                    gameActive = true;
                else if (gametime.TotalGameTime.TotalSeconds - 1 >= countdownOldTime.TotalSeconds)
                {
                    countdown--;
                    countdownOldTime = gametime.TotalGameTime;
                }
            }

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            background.Draw(spritebatch);
            finishObjective.Draw(spritebatch);
            enemyTank.Draw(spritebatch);
            playerTank.Draw(spritebatch);
            bulletHandler.Draw(spritebatch);
            finishObjective.DrawHUD(spritebatch);

            //Draw HUD

            //Create a rectangle representing how much of the bar should be shown
            Rectangle cooldownSource = new Rectangle(0, 0, cooldownBar.Width,
                (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown)));
            //Draw cooldownBar
            spritebatch.Draw(cooldownBar, new Vector2(30, (Game1.WindowHeight / 2) + (cooldownBar.Height / 2) +
                cooldownBar.Height - (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown))),
                cooldownSource, Color.White, 0, new Vector2(cooldownBar.Width / 2, cooldownBar.Height / 2), 1, SpriteEffects.None, 1);


            //Draw countdown
            if (countdown > 0 && countdown < 6)
            {
                Color countdownColor;
                if (countdown > 3)
                    countdownColor = Color.Red;
                else if (countdown > 1)
                    countdownColor = Color.OrangeRed;
                else countdownColor = Color.Yellow;

                spritebatch.DrawString(countdownFont, countdown.ToString(),
                    new Vector2(Game1.WindowWidth / 2 - countdownFont.MeasureString(countdown.ToString()).X / 2,
                    Game1.WindowHeight / 2 - countdownFont.MeasureString(countdown.ToString()).Y / 2),
                    countdownColor, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
            }

            base.Draw(spritebatch);
        }

        private void MoveBoard(Vector2 vector)
        {
            background.position += vector;
            bulletHandler.MoveBullets(vector);
            enemyTank.position += vector;
            finishObjective.position += vector;
        }

        private void MoveBoard(float x, float y)
        {
            background.position.X += x;
            bulletHandler.MoveBulletsX(x);
            enemyTank.position.X += x;
            finishObjective.position.X += x;

            background.position.Y += y;
            bulletHandler.MoveBulletsY(y);
            enemyTank.position.Y += y;
            finishObjective.position.Y += y;
        }
    }
}
