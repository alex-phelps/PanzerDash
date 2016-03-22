using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        private bool gameWon = false;

        private string endText;
        private Color endTextColor;
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

            player1.position += player1.velocity;
            player1.Update(gametime);
            player2.position += player2.velocity;
            player2.Update(gametime);

            camera1.Update(player1.position);
            camera2.Update(player2.position);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.End();

            graphicsDevice.Viewport = leftView;
            DrawSprites(spritebatch, camera1);

            graphicsDevice.Viewport = rightView;
            DrawSprites(spritebatch, camera2);

            graphicsDevice.Viewport = defaultView;

            spritebatch.Begin();
        }

        private void DrawSprites(SpriteBatch spritebatch, Camera camera)
        {
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
        }

        /// <summary>
        /// Main setup for the class
        /// </summary>
        private void Setup(ContentManager content)
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
