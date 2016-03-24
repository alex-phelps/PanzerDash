using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PanzerDash
{
    public class MultiplayerGameScreen : Screen
    {
        private GraphicsDevice graphicsDevice;

        private Viewport defaultView;
        private Viewport leftView;
        private Viewport rightView;

        private Camera camera1;
        private Camera camera2;

        private Texture2D border;

        private PlayerTank player1;
        private PlayerTank player2;
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

        private int winner = 0;
        private SpriteFont winTextFont;
        private SpriteFont winSubFont;
        private SpriteFont winTextMultiplayerFont;

        private SpriteFont countdownFont;
        private TimeSpan countdownOldTime;
        private int countdown = 6;

        private int level;

        private SoundEffectInstance bumpFX;
        private SoundEffectInstance explodeFX;
        private SoundEffectInstance winFX;
        private SoundEffectInstance loseFX;
        private SoundEffectInstance powerUpFX;
        private SoundEffectInstance countdownFX;
        private SoundEffectInstance goFX;

        public MultiplayerGameScreen(GraphicsDevice graphicsDevice, ContentManager content, EventHandler screenEvent, int level, 
            BulletHandler bulletHandler, PlayerTank player1, PlayerTank player2)
            : base(screenEvent)
        {
            this.graphicsDevice = graphicsDevice;
            this.bulletHandler = bulletHandler;
            this.player1 = player1;
            this.player2 = player2;
            this.level = level;

            random = new Random();

            defaultView = graphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width /= 2;
            rightView.Width /= 2;
            rightView.X = leftView.Width;

            camera1 = new Camera();
            camera2 = new Camera();

            Setup(content);
            SoundInit();
        }

        public override void Update(GameTime gametime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                screenEvent.Invoke(this, new EventArgs());

            if ((gameActive || firstUpdate) && !gameOver)
            {
                bulletHandler.Update(gametime);

                player1.Update(gametime);
                if (!player1.isStunned)
                    player1.position += player1.velocity;
                player2.Update(gametime);
                if (!player2.isStunned)
                    player2.position += player2.velocity;

                camera1.Update(player1.position);
                camera2.Update(player2.position);

                //Check if player1 is colliding with the color black in the background
                if (level == 4)
                {
                    //City Specific collision colors
                    if (Game1.IntersectColor(player1, background, new List<Color>
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
                        player1.position -= player1.velocity;

                        //Undo player1's rotation
                        player1.rotation -= player1.rotSpeed;

                        //Set player1's speed variables to 0;
                        player1.speed = 0;
                        player1.rotSpeed = 0;

                        if (bumpFX.State != SoundState.Playing)
                            bumpFX.Play();
                    }

                }
                else if (Game1.IntersectColor(player1, background, new Color(0, 0, 0)))
                {
                    //Undo tank movement
                    player1.position -= player1.velocity;

                    //Undo player1's rotation
                    player1.rotation -= player1.rotSpeed;

                    //Set player1's speed variables to 0
                    player1.speed = 0;
                    player1.rotSpeed = 0;

                    if (bumpFX.State != SoundState.Playing)
                        bumpFX.Play();
                }


                //Check if player2 is colliding with the color black in the background
                if (level == 4)
                {
                    //City Specific collision colors
                    if (Game1.IntersectColor(player2, background, new List<Color>
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
                        player2.position -= player2.velocity;

                        //Undo player2's rotation
                        player2.rotation -= player2.rotSpeed;

                        //Set player2's speed variables to 0;
                        player2.speed = 0;
                        player2.rotSpeed = 0;

                        if (bumpFX.State != SoundState.Playing)
                            bumpFX.Play();
                    }

                }
                else if (Game1.IntersectColor(player2, background, new Color(0, 0, 0)))
                {
                    //Undo tank movement
                    player2.position -= player2.velocity;

                    //Undo player2's rotation
                    player2.rotation -= player2.rotSpeed;

                    //Set player2's speed variables to 0;
                    player2.speed = 0;
                    player2.rotSpeed = 0;

                    if (bumpFX.State != SoundState.Playing)
                        bumpFX.Play();
                }

                //Check if player1 is colliding with the player2 tank
                //Since we have yet to update player2, if this is true we know player1 caused the collision
                if (Game1.IntersectPixels(player1, player2))
                {
                    if (bumpFX.State != SoundState.Playing)
                        bumpFX.Play();

                    //Undo tank movement
                    player1.position -= player1.velocity;

                    //Redo tank movement on X axis
                    player1.position.X += player1.velocity.X;

                    //If colliding, collision comes from either X direction or both directions
                    //If it was not colliding, then collision comes from Y direction or both directions
                    if (Game1.IntersectPixels(player1, player2))
                    {
                        //Undo X movement
                        player1.position.X -= player1.velocity.X;

                        //Redo tank movement on Y axis
                        player1.position.Y += player1.velocity.Y;

                        //If colliding, collision comes from both directions, and movement from both directions is reversed
                        //If not colliding, then undo all X direction movement but keep Y movement
                        if (Game1.IntersectPixels(player1, player2))
                        {
                            //Undo tank movement on Y axis since colliding from both direction
                            player1.position.Y -= player1.velocity.Y;
                        }
                    }
                    else //Collision from Y or both directions
                    {
                        //Undo X movement
                        player1.position.X -= player1.velocity.X;

                        //Redo Y movement
                        player1.position.Y += player1.velocity.Y;

                        //If colliding, collision comes from Y only
                        //If not colliding, the collision comes from both directions when combined only
                        if (Game1.IntersectPixels(player1, player2))
                        {
                            //Undo Y (Cause of collision)
                            player1.position.Y -= player1.velocity.Y;
                            //Redo X (Not cause of collision)
                            player1.position.X += player1.velocity.X;
                        }
                        else //Collision from both sides when combined only
                        {
                            //Undo Y movement
                            player1.position.Y -= player1.velocity.Y;
                        }
                    }

                    //Either way, collision is player1's fault, so reset his movement and rotation speeds

                    //Undo player1's rotation
                    player1.rotation -= player1.rotSpeed;

                    //Set player1's speed variables to 0;
                    player1.speed = 0;
                    player1.rotSpeed = 0;
                }

                //Check if player1 is colliding with the player2 tank
                //Since we have yet to update player2, if this is true we know player1 caused the collision
                if (Game1.IntersectPixels(player2, player1))
                {
                    if (bumpFX.State != SoundState.Playing)
                        bumpFX.Play();

                    //Undo tank movement
                    player2.position -= player2.velocity;

                    //Redo tank movement on X axis
                    player2.position.X += player2.velocity.X;

                    //If colliding, collision comes from either X direction or both directions
                    //If it was not colliding, then collision comes from Y direction or both directions
                    if (Game1.IntersectPixels(player1, player1))
                    {
                        //Undo X movement
                        player2.position.X -= player2.velocity.X;

                        //Redo tank movement on Y axis
                        player2.position.Y += player2.velocity.Y;

                        //If colliding, collision comes from both directions, and movement from both directions is reversed
                        //If not colliding, then undo all X direction movement but keep Y movement
                        if (Game1.IntersectPixels(player2, player1))
                        {
                            //Undo tank movement on Y axis since colliding from both direction
                            player2.position.Y -= player2.velocity.Y;
                        }
                    }
                    else //Collision from Y or both directions
                    {
                        //Undo X movement
                        player2.position.X -= player2.velocity.X;

                        //Redo Y movement
                        player2.position.Y += player2.velocity.Y;

                        //If colliding, collision comes from Y only
                        //If not colliding, the collision comes from both directions when combined only
                        if (Game1.IntersectPixels(player2, player1))
                        {
                            //Undo Y (Cause of collision)
                            player2.position.Y -= player2.velocity.Y;
                            //Redo X (Not cause of collision)
                            player2.position.X += player2.velocity.X;
                        }
                        else //Collision from both sides when combined only
                        {
                            //Undo Y movement
                            player2.position.Y -= player2.velocity.Y;
                        }
                    }

                    //Either way, collision is player2's fault, so reset his movement and rotation speeds

                    //Undo player2's rotation
                    player2.rotation -= player2.rotSpeed;

                    //Set player2's speed variables to 0;
                    player2.speed = 0;
                    player2.rotSpeed = 0;
                }

                //Check if player1 is colliding with the finish objective
                if (Game1.IntersectPixels(player1, finishObjective))
                {
                    //Undo tank movement
                    player1.position -= player1.velocity;

                    //Undo player's rotation
                    player1.rotation -= player1.rotSpeed;

                    //Set player's speed variables to 0;
                    player1.speed = 0;
                    player1.rotSpeed = 0;
                }

                //Check if player2 is colliding with the finish objective
                if (Game1.IntersectPixels(player2, finishObjective))
                {
                    //Undo tank movement
                    player2.position -= player2.velocity;

                    //Undo player's rotation
                    player2.rotation -= player2.rotSpeed;

                    //Set player's speed variables to 0;
                    player2.speed = 0;
                    player2.rotSpeed = 0;
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

                    //Check for player1 bullets
                    if (bullet.active && bullet.ownerTank == player2)
                    {
                        if (Game1.IntersectPixels(bullet, player1))
                        {
                            if (player1.currentPowerupType != PowerUpType.shield)
                                player1.stunLength = bullet.damage;
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
                    if (bullet.active && bullet.ownerTank == player1)
                    {
                        if (Game1.IntersectPixels(bullet, player2))
                        {
                            if (player2.currentPowerupType != PowerUpType.shield)
                                player2.stunLength = bullet.damage;
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
                    if (Game1.IntersectPixels(player1, powerup))
                    {
                        player1.CollectPowerUp(powerup);
                        powerUpFX.Play();
                        powerupsToRemove.Add(powerup);
                    }
                    else if (Game1.IntersectPixels(player2, powerup))
                    {
                        player2.CollectPowerUp(powerup);
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
                    winner = 1;
                }
                else if (finishObjective.enemyHealth <= 0)
                {
                    winFX.Play();
                    MediaPlayer.Stop();
                    gameOver = true;
                    winner = 2;
                }

                if (firstUpdate)
                    firstUpdate = false;
            }
            else if (gameOver)
            {
                //After game, before screen change

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    screenEvent.Invoke(this, new EventArgs());
            }
            else
            {
                //Before Game; Countdown
                if (countdown <= 0)
                {
                    goFX.Play();
                    MediaPlayer.Play(Game1.gameMusic);
                    gameActive = true;
                }
                else if (gametime.TotalGameTime.TotalSeconds - 1 >= countdownOldTime.TotalSeconds)
                {
                    countdown--;
                    if (countdown != 0)
                        countdownFX.Play();
                    countdownOldTime = gametime.TotalGameTime;
                }
            }

            base.Update(gametime);
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.End();

            //Left side view
            graphicsDevice.Viewport = leftView;
            DrawSprites(spritebatch, camera1);

            //Right side view
            graphicsDevice.Viewport = rightView;
            DrawSprites(spritebatch, camera2);

            //Default view
            graphicsDevice.Viewport = defaultView;
            spritebatch.Draw(border, new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2),
                new Rectangle(0, 0, border.Width, border.Height), Color.White, 0,
                new Vector2(border.Width / 2, border.Height / 2), 1, SpriteEffects.None, 0);

            //Draw HUDs

            finishObjective.DrawHUDMultiplayer(spritebatch);

            //Draw player1 HUD

            //Create a rectangle representing how much of the bars should be shown
            Rectangle cooldownSource = new Rectangle(0, 0, cooldownBar.Width,
                (int)(cooldownBar.Height * ((float)player1.currentCooldown / (float)player1.baseCooldown)));
            Rectangle powerupSource = new Rectangle(0, 0, powerupBar.Width,
                (int)(powerupBar.Height * ((float)player1.powerupTime / (float)player1.basePowerupTime)));

            //Draw cooldownBar
            spritebatch.Draw(cooldownBar, new Vector2(30, (Game1.WindowHeight / 2) + (cooldownBar.Height / 2) +
                cooldownBar.Height - (int)(cooldownBar.Height * ((float)player1.currentCooldown / (float)player1.baseCooldown))),
                cooldownSource, Color.White, 0, new Vector2(cooldownBar.Width / 2, cooldownBar.Height / 2), 1, SpriteEffects.None, 1);
            //Draw Powerup cooldown bar
            spritebatch.Draw(powerupBar, new Vector2(Game1.WindowWidth / 2 - 30, (Game1.WindowHeight / 2) + (powerupBar.Height / 2) +
                powerupBar.Height - (int)(powerupBar.Height * ((float)player1.powerupTime / (float)player1.basePowerupTime))),
                powerupSource, Color.White, 0, new Vector2(powerupBar.Width / 2, powerupBar.Height / 2), 1, SpriteEffects.None, 1);

            //Draw player2 HUD

            //Create a rectangle representing how much of the bars should be shown
            cooldownSource = new Rectangle(0, 0, cooldownBar.Width,
                (int)(cooldownBar.Height * ((float)player2.currentCooldown / (float)player2.baseCooldown)));
            powerupSource = new Rectangle(0, 0, powerupBar.Width,
                (int)(powerupBar.Height * ((float)player2.powerupTime / (float)player2.basePowerupTime)));

            //Draw cooldownBar
            spritebatch.Draw(cooldownBar, new Vector2(Game1.WindowWidth - 30, (Game1.WindowHeight / 2) + (cooldownBar.Height / 2) +
                cooldownBar.Height - (int)(cooldownBar.Height * ((float)player2.currentCooldown / (float)player2.baseCooldown))),
                cooldownSource, Color.White, 0, new Vector2(cooldownBar.Width / 2, cooldownBar.Height / 2), 1, SpriteEffects.None, 1);
            //Draw Powerup cooldown bar
            spritebatch.Draw(powerupBar, new Vector2(Game1.WindowWidth / 2 + 30, (Game1.WindowHeight / 2) + (powerupBar.Height / 2) +
                powerupBar.Height - (int)(powerupBar.Height * ((float)player2.powerupTime / (float)player2.basePowerupTime))),
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

            if (gameOver)
            {
                string player1Text;
                Color player1Color;
                string player2Text;
                Color player2Color;
                if (winner == 1)
                {
                    player1Text = "Congratulations!\nYou won!";
                    player1Color = Color.Lime;
                    player2Text = "You lost!\nToo bad";
                    player2Color = Color.Red;
                }
                else
                {
                    player2Text = "Congratulations!\nYou won!";
                    player2Color = Color.Lime;
                    player1Text = "You lost!\nToo bad";
                    player1Color = Color.Red;
                }

                spritebatch.DrawString(winTextMultiplayerFont, player1Text, new Vector2(
                    Game1.WindowWidth / 4 - winTextMultiplayerFont.MeasureString(player1Text).X / 2, Game1.WindowHeight / 3
                    - winTextMultiplayerFont.MeasureString(player1Text).Y / 2), player1Color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);


                spritebatch.DrawString(winTextMultiplayerFont, player2Text, new Vector2(
                    Game1.WindowWidth * 3 / 4 - winTextMultiplayerFont.MeasureString(player2Text).X / 2, Game1.WindowHeight / 3
                    - winTextMultiplayerFont.MeasureString(player2Text).Y / 2), player2Color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                spritebatch.DrawString(winSubFont, "> Press Enter to Continue <", new Vector2(
                    Game1.WindowWidth / 2 - winSubFont.MeasureString("> Press Enter to Continue <").X / 2,
                    (2 * Game1.WindowHeight) / 3 - winSubFont.MeasureString("> Press Enter to Continue <").Y / 2),
                    Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
        }

        private void DrawSprites(SpriteBatch spritebatch, Camera camera)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, camera.transform);

            background.Draw(spritebatch);
            finishObjective.Draw(spritebatch);
            foreach (Powerup powerup in powerups)
            {
                powerup.Draw(spritebatch);
            }
            player1.Draw(spritebatch);
            player2.Draw(spritebatch);
            bulletHandler.Draw(spritebatch);

            spritebatch.End();
            spritebatch.Begin();
        }

        /// <summary>
        /// Main setup for the class
        /// </summary>
        private void Setup(ContentManager content)
        {
            //Main Setup

            border = content.Load<Texture2D>("SplitScreenBorder");
            cooldownBar = content.Load<Texture2D>("CooldownBar");
            countdownFont = content.Load<SpriteFont>("CountdownFont");
            powerupBar = content.Load<Texture2D>("PowerupBar");
            winTextFont = content.Load<SpriteFont>("WinTextFont");
            winTextMultiplayerFont = content.Load<SpriteFont>("WinTextMultiplayerFont");
            winSubFont = content.Load<SpriteFont>("WinSubFont");
            powerupSpawns = new List<Vector2>();
            powerups = new List<Powerup>();
            powerupsToRemove = new List<Powerup>();

            Vector2 levelSize = new Vector2(3200, 3200);
            Vector2 startPosInImage;
            Vector2 finishPosInImage;

            if (level == 2) // Desert
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
            else if (level == 3) // Snow
            {
                background = new Background(content.Load<Texture2D>("Level3"));
                startPosInImage = new Vector2(576, 942);
                finishPosInImage = new Vector2(2120, 1015);

                //Powerup spawn locations in the image
                powerupSpawns.Add(new Vector2(1408, 995));
                powerupSpawns.Add(new Vector2(1456, 2002));
                powerupSpawns.Add(new Vector2(1938, 1809));
            }
            else if (level == 4) // City
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
            else if (level == 5) // Mesa
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
            else if (level == 6) // Jungle
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

            //Set background position
            background.position.X = (levelSize.X / 2 - startPosInImage.X) + Game1.WindowWidth / 2;
            background.position.Y = (levelSize.Y / 2 - startPosInImage.Y) + Game1.WindowHeight / 2;

            //Set finish position
            finishObjective = new FinishObjective(content, new Vector2(
                ((finishPosInImage.X - levelSize.X / 2) + background.position.X),
                ((finishPosInImage.Y - levelSize.Y / 2) + background.position.Y)));

            //Add powerups
            foreach (Vector2 loc in powerupSpawns)
            {
                PowerUpType type;
                if (random.Next(300) == 0)
                {
                    type = PowerUpType.rainbow;
                }
                else
                {
                    int typeInInt = random.Next(4);

                    if (typeInInt == 0)
                        type = PowerUpType.speed;
                    else if (typeInInt == 1)
                        type = PowerUpType.shield;
                    else if (typeInInt == 2)
                        type = PowerUpType.rapid;
                    else type = PowerUpType.damage;
                }

                powerups.Add(new Powerup(content, new Vector2(
                    ((loc.X - levelSize.X / 2) + background.position.X),
                    ((loc.Y - levelSize.Y / 2) + background.position.Y)), type));
            }
        }

        /// <summary>
        /// Sets up the sound for this class
        /// </summary>
        private void SoundInit()
        {
            bumpFX = Game1.bumpFX.CreateInstance();
            explodeFX = Game1.explodeFX.CreateInstance();
            powerUpFX = Game1.powerUpFX.CreateInstance();
            winFX = Game1.winFX.CreateInstance();
            loseFX = Game1.loseFX.CreateInstance();
            countdownFX = Game1.countdownFX.CreateInstance();
            goFX = Game1.goFX.CreateInstance();
        }
    }
}
