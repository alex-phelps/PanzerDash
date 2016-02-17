using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Game state for the main game
    /// </summary>
    public class GameScreen : Screen
    {
        private const int levelCount = 5;

        private PlayerTank playerTank;
        private AITank enemyTank;
        private Background background;
        private BulletHandler bulletHandler;
        private Texture2D cooldownBar;
        private Texture2D powerupBar;
        private FinishObjective finishObjective;
        private Random random;

        private List<Vector2> powerupSpawns;
        private List<Powerup> powerups;
        private List<Powerup> powerupsToRemove;

        public bool gameOver { get; private set; }
        private bool gameActive = false;
        private bool firstUpdate = true;
        private bool gameWon = false;

        private string winText;
        private Color winTextColor;
        private SpriteFont winTextFont;
        private SpriteFont winSubFont;

        private SpriteFont countdownFont;
        private TimeSpan countdownOldTime;
        private int countdown = 6;

        private bool unlockContent;
        private int level;

        private SoundEffectInstance bumpFX;
        private SoundEffectInstance explodeFX;
        private SoundEffectInstance winFX;
        private SoundEffectInstance loseFX;
        private SoundEffectInstance powerUpFX;

        public GameScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            bulletHandler = new BulletHandler();
            random = new Random();
            unlockContent = false;

            //Random tanks
            playerTank = new PlayerTank(content, bulletHandler, RandomTankPart(), RandomTankPart());
            enemyTank = new AITank(content, bulletHandler, RandomTankPart(), RandomTankPart(),
                new Vector2(playerTank.position.X + 100, playerTank.position.Y));

            //Random Level
            int level = random.Next(1, levelCount + 1);
            this.level = level;
            Setup(content, level);
            SoundInit();
        }

        public GameScreen(ContentManager content, EventHandler screenEvent, int level,
            BulletHandler bulletHandler, PlayerTank playerTank, AITank enemyTank, bool unlockContent) 
            : base(screenEvent)
        {
            this.bulletHandler = bulletHandler;
            this.playerTank = playerTank;
            this.enemyTank = enemyTank;
            this.unlockContent = unlockContent;
            this.level = level;

            random = new Random();

            Setup(content, level);
            SoundInit();
        }

        public override void Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                screenEvent.Invoke(this, new EventArgs());

            if ((gameActive || firstUpdate) && !gameWon)
            {
                playerTank.Update(gametime);
                bulletHandler.Update(gametime);

                //If player is not stunned, allow it to move
                if (!playerTank.isStunned)
                {
                    //Move objects based on the player's tank's movement
                    MoveBoard(-playerTank.velocity);
                }

                //Check if player is colliding with the color black in the background
                if (level == 4)
                {
                    //City Specific collision colors
                    if (Game1.IntersectColor(playerTank, background, new List<Color>
                        {
                            new Color(0, 0, 0),
                            new Color(87, 87, 87),
                            new Color(162, 162, 162),
                            new Color(138, 138, 138),
                            new Color(75, 69, 66),
                            new Color(59, 69, 77),
                            new Color(58, 37, 28),
                            new Color(111, 66, 54),
                            new Color(116, 31, 9),
                            new Color(154, 123, 93)
                        }))
                    {
                        //Undo tank movement
                        MoveBoard(playerTank.velocity);

                        //Undo player's rotation
                        playerTank.rotation -= playerTank.rotSpeed;

                        //Set player's speed variables to 0;
                        playerTank.speed = 0;
                        playerTank.rotSpeed = 0;

                        if (bumpFX.State != SoundState.Playing)
                            bumpFX.Play();
                    }

                }
                else if (Game1.IntersectColor(playerTank, background, new Color(0, 0, 0)))
                {
                    //Undo tank movement
                    MoveBoard(playerTank.velocity);

                    //Undo player's rotation
                    playerTank.rotation -= playerTank.rotSpeed;

                    //Set player's speed variables to 0;
                    playerTank.speed = 0;
                    playerTank.rotSpeed = 0;

                    if (bumpFX.State != SoundState.Playing)
                        bumpFX.Play();
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

                    //Check for collision with level
                    if (level == 4)
                    {
                        if (bullet.active && Game1.IntersectColor(bullet, background, new List<Color>
                        {
                            new Color(0, 0, 0),
                            new Color(87, 87, 87),
                            new Color(162, 162, 162),
                            new Color(138, 138, 138),
                            new Color(75, 69, 66),
                            new Color(59, 69, 77),
                            new Color(58, 37, 28),
                            new Color(111, 66, 54),
                            new Color(116, 31, 9),
                            new Color(154, 123, 93)
                        }))
                        {
                            explodeFX.Play();
                            bulletHandler.Destroy(bullet);
                        }
                    }
                    else if (bullet.active && Game1.IntersectColor(bullet, background, new Color(0, 0, 0)))
                    {
                        explodeFX.Play();
                        bulletHandler.Destroy(bullet);
                    }

                    //Check for enemy bullets
                    if (bullet.active && bullet.ownerTank == enemyTank)
                    {
                        if (Game1.IntersectPixels(bullet, playerTank))
                        {
                            if (playerTank.currentPowerupType != PowerUpType.shield)
                                playerTank.stunLength = bullet.damage;
                            explodeFX.Play();
                            bulletHandler.Destroy(bullet);
                        }
                        else if (Game1.IntersectPixels(bullet, finishObjective))
                        {
                            finishObjective.enemyHealth -= bullet.damage + 0.8f; // To buff the AI a bit
                            explodeFX.Play();
                            bulletHandler.Destroy(bullet);
                        }

                    }

                    //Check for player bullets
                    if (bullet.active && bullet.ownerTank == playerTank)
                    {
                        if (Game1.IntersectPixels(bullet, enemyTank))
                        {
                            if (enemyTank.currentPowerupType != PowerUpType.shield)
                                enemyTank.stunLength = bullet.damage;
                            explodeFX.Play();
                            bulletHandler.Destroy(bullet);
                        }
                        else if (Game1.IntersectPixels(bullet, finishObjective))
                        {
                            finishObjective.playerHealth -= bullet.damage;
                            explodeFX.Play();
                            bulletHandler.Destroy(bullet);
                        }
                    }
                }

                //Check for powerup collection
                foreach (Powerup powerup in powerups)
                {
                    if (Game1.IntersectPixels(playerTank, powerup))
                    {
                        playerTank.CollectPowerUp(powerup);
                        powerUpFX.Play();
                        powerupsToRemove.Add(powerup);
                    }
                    else if (Game1.IntersectPixels(enemyTank, powerup))
                    {
                        enemyTank.CollectPowerUp(powerup);
                        powerUpFX.Play();
                        powerupsToRemove.Add(powerup);
                    }
                }

                //Remove used powerups
                foreach (Powerup powerup in powerupsToRemove)
                {
                    powerups.Remove(powerup);
                }


                if (finishObjective.playerHealth <= 0)
                {
                    winFX.Play();
                    MediaPlayer.Stop();
                    gameOver = true;
                    gameWon = true;
                    winText = "Congratulations!\nYou won!";
                    winTextColor = Color.Lime;
                }
                else if (finishObjective.enemyHealth <= 0)
                {
                    loseFX.Play();
                    MediaPlayer.Stop();
                    gameOver = true;
                    gameWon = false;
                    winText = "You lost!\nToo bad";
                    winTextColor = Color.Red;
                }

                if (firstUpdate)
                    firstUpdate = false;
            }
            else if (gameOver)
            {
                //After game, before screen change
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (unlockContent && gameWon)
                    {
                        Game1.levelsUnlocked++;
                        Game1.Save();
                    }
                    screenEvent.Invoke(this, new EventArgs());
                }
            }
            else
            {
                //Before Game; Countdown
                if (countdown <= 0)
                {
                    MediaPlayer.Play(Game1.gameMusic);
                    gameActive = true;
                }
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
            //Draw Sprites
            background.Draw(spritebatch);
            finishObjective.Draw(spritebatch);
            foreach (Powerup powerup in powerups)
            {
                powerup.Draw(spritebatch);
            }
            enemyTank.Draw(spritebatch);
            playerTank.Draw(spritebatch);
            bulletHandler.Draw(spritebatch);
            finishObjective.DrawHUD(spritebatch);


            //Draw HUD

            //Create a rectangle representing how much of the bars should be shown
            Rectangle cooldownSource = new Rectangle(0, 0, cooldownBar.Width,
                (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown)));
            Rectangle powerupSource = new Rectangle(0, 0, powerupBar.Width,
                (int)(powerupBar.Height * ((float)playerTank.powerupTime / (float)playerTank.basePowerupTime)));
            //Draw cooldownBar
            spritebatch.Draw(cooldownBar, new Vector2(30, (Game1.WindowHeight / 2) + (cooldownBar.Height / 2) +
                cooldownBar.Height - (int)(cooldownBar.Height * ((float)playerTank.currentCooldown / (float)playerTank.baseCooldown))),
                cooldownSource, Color.White, 0, new Vector2(cooldownBar.Width / 2, cooldownBar.Height / 2), 1, SpriteEffects.None, 1);
            //Draw Powerup cooldown bar
            spritebatch.Draw(powerupBar, new Vector2(Game1.WindowWidth - 30, (Game1.WindowHeight / 2) + (powerupBar.Height / 2) +
                powerupBar.Height - (int)(powerupBar.Height * ((float)playerTank.powerupTime / (float)playerTank.basePowerupTime))),
                powerupSource, Color.White, 0, new Vector2(powerupBar.Width / 2, powerupBar.Height / 2), 1, SpriteEffects.None, 1);


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

            //Draw win text
            if (gameWon)
            {
                spritebatch.DrawString(winTextFont, winText, new Vector2(
                    Game1.WindowWidth / 2 - winTextFont.MeasureString(winText).X / 2, Game1.WindowHeight / 3
                    - winTextFont.MeasureString(winText).Y / 2), winTextColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                spritebatch.DrawString(winSubFont, "> Press Enter to Continue <", new Vector2(
                    Game1.WindowWidth / 2 - winSubFont.MeasureString("> Press Enter to Continue <").X / 2,
                    (2 * Game1.WindowHeight) / 3 - winSubFont.MeasureString("> Press Enter to Continue <").Y / 2),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            base.Draw(spritebatch);
        }

        private void Setup(ContentManager content, int level)
        {
            //Main Setup

            cooldownBar = content.Load<Texture2D>("CooldownBar");
            countdownFont = content.Load<SpriteFont>("CountdownFont");
            powerupBar = content.Load<Texture2D>("PowerupBar");
            winTextFont = content.Load<SpriteFont>("WinTextFont");
            winSubFont = content.Load<SpriteFont>("WinSubFont");
            powerupSpawns = new List<Vector2>();
            powerups = new List<Powerup>();
            powerupsToRemove = new List<Powerup>();

            Vector2 levelSize = new Vector2(3200, 3200);
            Vector2 startPosInImage;
            Vector2 finishPosInImage;

            if (level == 2) // Level 2
            {
                background = new Background(content.Load<Texture2D>("Level2"));
                startPosInImage = new Vector2(2475, 2415);
                finishPosInImage = new Vector2(545, 1168);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(2600, 1570));
                powerupSpawns.Add(new Vector2(1616, 1425));
                powerupSpawns.Add(new Vector2(1422, 1833));
                powerupSpawns.Add(new Vector2(815, 1744));
            }
            else if (level == 3)
            {
                background = new Background(content.Load<Texture2D>("Level3"));
                startPosInImage = new Vector2(576, 942);
                finishPosInImage = new Vector2(2120, 1015);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(1414, 1025));
                powerupSpawns.Add(new Vector2(1456, 2002));
                powerupSpawns.Add(new Vector2(1938, 1809));
            }
            else if (level == 4)
            {
                levelSize = new Vector2(3904, 3904);

                background = new Background(content.Load<Texture2D>("Level4"));
                startPosInImage = new Vector2(735, 2054);
                finishPosInImage = new Vector2(1850, 2870);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(1215, 1300));
                powerupSpawns.Add(new Vector2(1915, 950));
                powerupSpawns.Add(new Vector2(2810, 1983));
                powerupSpawns.Add(new Vector2(2434, 2544));
                powerupSpawns.Add(new Vector2(2560, 3280));

            }
            else if (level == 5)
            {
                background = new Background(content.Load<Texture2D>("Level5"));
                startPosInImage = new Vector2(1560, 1320);
                finishPosInImage = new Vector2(1255, 630);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(2306, 1027));
                powerupSpawns.Add(new Vector2(1783, 1738));
                powerupSpawns.Add(new Vector2(2170, 2060));
                powerupSpawns.Add(new Vector2(892, 2288));
                powerupSpawns.Add(new Vector2(1088, 1032));
            }
            else if (level == 6)
            {
                background = new Background(content.Load<Texture2D>("Level6"));
                startPosInImage = new Vector2(1800, 2200);
                finishPosInImage = new Vector2(1444, 2570);

                //Powerup spawn locations in the image
            }
            else // Level 1
            {
                background = new Background(content.Load<Texture2D>("Level1"));
                startPosInImage = new Vector2(654, 2478);
                finishPosInImage = new Vector2(2143, 1855);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(704, 2582));
                powerupSpawns.Add(new Vector2(1400, 1485));
                powerupSpawns.Add(new Vector2(1285, 1024));
                powerupSpawns.Add(new Vector2(1448, 2324));
            }

            background.position.X = (levelSize.X / 2 - startPosInImage.X) + Game1.WindowWidth / 2;
            background.position.Y = (levelSize.Y / 2 - startPosInImage.Y) + Game1.WindowHeight / 2;

            finishObjective = new FinishObjective(content, new Vector2(
                ((finishPosInImage.X - levelSize.X / 2) + background.position.X),
                ((finishPosInImage.Y - levelSize.Y / 2) + background.position.Y)));

            foreach (Vector2 loc in powerupSpawns)
            {
                PowerUpType type;
                int typeInInt = random.Next(4);

                if (typeInInt == 0)
                    type = PowerUpType.speed;
                else if (typeInInt == 1)
                    type = PowerUpType.shield;
                else if (typeInInt == 2)
                    type = PowerUpType.rapid;
                else type = PowerUpType.damage;

                powerups.Add(new Powerup(content, new Vector2(
                    ((loc.X - levelSize.X / 2) + background.position.X),
                    ((loc.Y - levelSize.Y / 2) + background.position.Y)), type));
            }
        }

        private void MoveBoard(Vector2 vector)
        {
            background.position += vector;
            bulletHandler.MoveBullets(vector);
            enemyTank.position += vector;
            finishObjective.position += vector;

            foreach (Powerup powerup in powerups)
            {
                powerup.position += vector;
            }
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

            foreach (Powerup powerup in powerups)
            {
                powerup.position.X += x;
                powerup.position.Y += y;
            }
        }

        private TankPartType RandomTankPart()
        {
            int number = random.Next(1, 7);

            //No Rainbow Allowed Here
            if (number == 2)
                return TankPartType.desert;
            else if (number == 3)
                return TankPartType.jungle;
            else if (number == 4)
                return TankPartType.red;
            else if (number == 5)
                return TankPartType.snow;
            else if (number == 6)
                return TankPartType.urban;
            else return TankPartType.basic;
        }

        private void SoundInit()
        {
            bumpFX = Game1.bumpFX.CreateInstance();
            explodeFX = Game1.explodeFX.CreateInstance();
            powerUpFX = Game1.powerUpFX.CreateInstance();
            winFX = Game1.winFX.CreateInstance();
            loseFX = Game1.loseFX.CreateInstance();
        }
    }
}
