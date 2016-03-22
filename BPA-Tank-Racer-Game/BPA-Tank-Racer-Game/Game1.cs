using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace PanzerDash
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Screen currentScreen;
        MenuScreen menuScreen;
        OptionsScreen optionsScreen;
        GameScreen gameScreen;

        //Default window size
        public static int WindowWidth = 800;
        public static int WindowHeight = 480;

        public static SoundEffect bumpFX;
        public static SoundEffect explodeFX;
        public static SoundEffect powerUpFX;
        public static SoundEffect selectFX;
        public static SoundEffect shootFX;
        public static SoundEffect winFX;
        public static SoundEffect loseFX;
        public static SoundEffect countdownFX;
        public static SoundEffect goFX;
        public static Song gameMusic;

        public static float effectVolume = 0.5f;
        public static float musicVolume = 0.3f;

        //Save Data
        public static Color backGroundColor = Color.CornflowerBlue; //Default color
        public static int levelsUnlocked;
        public static bool hasRainbowBase;
        public static bool hasRainbowGun;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;

            graphics.ApplyChanges();

            LoadSaveData();

            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all the content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Sound Content
            bumpFX = Content.Load<SoundEffect>("Sounds\\BumpNoise");
            explodeFX = Content.Load<SoundEffect>("Sounds\\ExplosionSound");
            powerUpFX = Content.Load<SoundEffect>("Sounds\\PowerUp");
            selectFX = Content.Load<SoundEffect>("Sounds\\SelectSound");
            shootFX = Content.Load<SoundEffect>("Sounds\\ShootSound");
            winFX = Content.Load<SoundEffect>("Sounds\\WinSound");
            loseFX = Content.Load<SoundEffect>("Sounds\\YouLose");
            countdownFX = Content.Load<SoundEffect>("Sounds\\countdownSound");
            goFX = Content.Load<SoundEffect>("Sounds\\goSound");
            gameMusic = Content.Load<Song>("Sounds\\GameMusic");

            currentScreen = new PublishScreen(Content, new EventHandler(PublishScreenEvent));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Sound Updates
            if (SoundEffect.MasterVolume != effectVolume)
                SoundEffect.MasterVolume = effectVolume;

            if (MediaPlayer.Volume != musicVolume)
                MediaPlayer.Volume = musicVolume;

            //Update the current screen
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Temp background color
            GraphicsDevice.Clear(backGroundColor);

            //Begin Spritebatch
            spriteBatch.Begin();

            //Draw the current screen
            currentScreen.Draw(spriteBatch);

            //End Spritebatch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void MenuScreenEvent(object sender, EventArgs e)
        {
            if (menuScreen.selectedButton == 0) // Play Now
            {
                currentScreen = new GameScreen(Content, new EventHandler(GameScreenEvent));
            }
            else if (menuScreen.selectedButton == 1) // Tutorial
            {
                currentScreen = new TutorialScreen(Content, new EventHandler(GameScreenEvent));
            }
            else if (menuScreen.selectedButton == 2) // Career
            {
                currentScreen = new CareerModeScreen(Content, new EventHandler(CareerModeScreenEvent));
            }
            else if (menuScreen.selectedButton == 3) // Free Mode
            {
                //New freemode screen
                currentScreen = new FreeModeScreen(Content, new EventHandler(FreeModeScreenEvent));
            }
            else if (menuScreen.selectedButton == 4) // Options
            {
                //New Options Screen
                optionsScreen = new OptionsScreen(Content, new EventHandler(OptionsScreenEvent));
                currentScreen = optionsScreen;
            }
            else Environment.Exit(0); // Quit
        }

        private void FreeModeScreenEvent(object sender, EventArgs e)
        {
            FreeModeScreen freeModeScreen = (FreeModeScreen)currentScreen;

            //If there is a game ready to start
            if (freeModeScreen.gameReady)
            {
                currentScreen = new GameScreen(Content, new EventHandler(GameScreenEvent), freeModeScreen.level,
                    freeModeScreen.bulletHandler, freeModeScreen.playerTank, freeModeScreen.enemyTank, false);
            }
            else currentScreen = menuScreen;
        }

        private void CareerModeScreenEvent(object sender, EventArgs e)
        {
            CareerModeScreen careerModeScreen = (CareerModeScreen)currentScreen;

            //If there is a game ready to start
            if (careerModeScreen.gameReady)
            {
                currentScreen = new GameScreen(Content, new EventHandler(GameScreenEvent), careerModeScreen.level,
                    careerModeScreen.bulletHandler, careerModeScreen.playerTank, careerModeScreen.enemyTank,
                    careerModeScreen.unlockContent);
            }
            else currentScreen = menuScreen;
        }

        private void OptionsScreenEvent(object sender, EventArgs e)
        {
            if (optionsScreen.selectedButton == 0) // Sound
            {
                currentScreen = new SoundScreen(Content, new EventHandler(SoundScreenEvent));
            }
            else if (optionsScreen.selectedButton == 1) // Color
            {
                currentScreen = new ColorScreen(Content, new EventHandler(ColorScreenEvent));
            }
            else if (optionsScreen.selectedButton == 2) // Credits
            {
                currentScreen = new CreditsScreen(Content, new EventHandler(CreditsScreenEvent));
            }
            else if (optionsScreen.selectedButton == 3) //Reset
            {
                currentScreen = new ResetScreen(Content, new EventHandler(ResetScreenEvent));
            }
            else if (optionsScreen.selectedButton == 4) // Back
            {
                currentScreen = menuScreen;
            }
        }

        private void ColorScreenEvent(object sender, EventArgs e)
        {
            Save(); //Saves the background screen color
            currentScreen = optionsScreen;
        }

        private void SoundScreenEvent(object sender, EventArgs e)
        {
            currentScreen = optionsScreen;
        }

        private void CreditsScreenEvent(object sender, EventArgs e)
        {
            currentScreen = optionsScreen;
        }

        private void PublishScreenEvent(object sender, EventArgs e)
        {
            //Create a new menuscreen
            menuScreen = new MenuScreen(Content, new EventHandler(MenuScreenEvent));
            currentScreen = menuScreen;
        }

        private void GameScreenEvent(object sender, EventArgs e)
        {
            gameScreen = (GameScreen)currentScreen;

            //If the game is not over, then the user paused the game
            if (!gameScreen.gameOver)
                currentScreen = new PauseScreen(Content, new EventHandler(PauseScreenEvent));
            else currentScreen = menuScreen;
        }

        private void PauseScreenEvent(object sender, EventArgs e)
        {
            PauseScreen screen = (PauseScreen)currentScreen;

            if (screen.selectedButton == 0)
            {
                MediaPlayer.Resume();
                currentScreen = gameScreen;
            }
            else if (screen.selectedButton == 1)
            {
                optionsScreen = new OptionsScreen(Content, new EventHandler(PausedOptionsScreenEvent));
                currentScreen = optionsScreen;
            }
            else if (screen.selectedButton == 2)
            {
                currentScreen = menuScreen;
            }
        }

        private void PausedOptionsScreenEvent(object sender, EventArgs e)
        {
            OptionsScreen screen = (OptionsScreen)currentScreen;

            if (optionsScreen.selectedButton == 0) // Sound
            {
                currentScreen = new SoundScreen(Content, new EventHandler(SoundScreenEvent));
            }
            else if (optionsScreen.selectedButton == 1) // Color
            {
                currentScreen = new ColorScreen(Content, new EventHandler(ColorScreenEvent));
            }
            else if (optionsScreen.selectedButton == 2) // Credits
            {
                currentScreen = new CreditsScreen(Content, new EventHandler(CreditsScreenEvent));
            }
            else if (optionsScreen.selectedButton == 3) //Reset
            {
                currentScreen = new ResetScreen(Content, new EventHandler(ResetScreenEvent));
            }
            else if (optionsScreen.selectedButton == 4) // Back
            {
                currentScreen = new PauseScreen(Content, new EventHandler(PauseScreenEvent));
            }
        }

        private void ResetScreenEvent(object sender, EventArgs e)
        {
            ResetScreen resetScreen = (ResetScreen)currentScreen;

            if (resetScreen.selectedButton == 0)
                currentScreen = optionsScreen;
            else if (resetScreen.selectedButton == 1)
            {
                backGroundColor = Color.CornflowerBlue;
                levelsUnlocked = 0;
                hasRainbowBase = false;
                hasRainbowGun = false;

                //Save the resets
                Save();

                currentScreen = menuScreen;
            }
        }

        /// <summary>
        /// Checks if two game objects are colliding
        /// </summary>
        /// <param name="objA">An object to check collision for</param>
        /// <param name="objB">An object to check collision against</param>
        /// <returns></returns>
        public static bool IntersectPixels(GameObject objA, GameObject objB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = objA.transformMatrix * Matrix.Invert(objB.transformMatrix);

            //For each row of pixel in A
            for (int yA = 0; yA < objA.Height; yA++)
            {
                //For each pixel in that row
                for (int xA = 0; xA < objA.Width; xA++)
                {
                    //Calculate this pixel's location in B
                    Vector2 positionInB = Vector2.Transform(new Vector2(xA, yA), transformAToB);

                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    if (xB >= 0 && xB < objB.Width &&
                        yB >= 0 && yB < objB.Height)
                    {
                        //Get colors of the overlapping pixels
                        Color colorA = objA.colorData[xA + yA * objA.Width];
                        Color colorB = objB.colorData[xB + yB * objB.Width];

                        //If both pixels are not completely transparent
                        if (colorA.A * colorB.A != 0)
                        {
                            //Intersection found
                            return true;
                        }
                    }
                }
            }

            //No intersection
            return false;
        }

        /// <summary>
        /// Checks if an object is colliding with a specific color in another object
        /// </summary>
        /// <param name="objA">An object to check collision for</param>
        /// <param name="objB">An object to check for collision of the color in</param>
        /// <param name="color">The color to check collision against</param>
        /// <returns></returns>
        public static bool IntersectColor(GameObject objA, GameObject objB, Color color)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = objA.transformMatrix * Matrix.Invert(objB.transformMatrix);

            //For each row of pixel in A
            for (int yA = 0; yA < objA.Height; yA++)
            {
                //For each pixel in that row
                for (int xA = 0; xA < objA.Width; xA++)
                {
                    //Calculate this pixel's location in B
                    Vector2 positionInB = Vector2.Transform(new Vector2(xA, yA), transformAToB);

                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    if (xB >= 0 && xB < objB.Width &&
                        yB >= 0 && yB < objB.Height)
                    {
                        //Get colors of the overlapping pixels
                        Color colorA = objA.colorData[xA + yA * objA.Width];
                        Color colorB = objB.colorData[xB + yB * objB.Width];

                        //If color A is not transparent and color B is the desired color
                        if (colorA.A != 0 && colorB == color)
                        {
                            //Intersection found
                            return true;
                        }
                    }
                }
            }

            //No intersection
            return false;
        }

        /// <summary>
        /// Checks if an object is colliding with a specific set of colors in another object
        /// </summary>
        /// <param name="objA">An object to check collision for</param>
        /// <param name="objB">An object to check for collision of the color in</param>
        /// <param name="colors">List of colors to check against</param>
        /// <returns></returns>
        public static bool IntersectColor(GameObject objA, GameObject objB, List<Color> colors)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = objA.transformMatrix * Matrix.Invert(objB.transformMatrix);

            //For each row of pixel in A
            for (int yA = 0; yA < objA.Height; yA++)
            {
                //For each pixel in that row
                for (int xA = 0; xA < objA.Width; xA++)
                {
                    //Calculate this pixel's location in B
                    Vector2 positionInB = Vector2.Transform(new Vector2(xA, yA), transformAToB);

                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    if (xB >= 0 && xB < objB.Width &&
                        yB >= 0 && yB < objB.Height)
                    {
                        //Get colors of the overlapping pixels
                        Color colorA = objA.colorData[xA + yA * objA.Width];
                        Color colorB = objB.colorData[xB + yB * objB.Width];

                        //Loop through the colors provided
                        foreach (Color color in colors)
                        {
                            //If color A is not transparent and color B is the desired color
                            if (colorA.A != 0 && colorB == color)
                            {
                                //Intersection found
                                return true;
                            }
                        }
                    }
                }
            }

            //No intersection
            return false;
        }

        /// <summary>
        /// Loads save data if possible
        /// </summary>
        public static void LoadSaveData()
        {
            if (File.Exists("SaveData.txt"))
            {
                StreamReader inFile = File.OpenText("SaveData.txt");

                while (!inFile.EndOfStream)
                {
                    string text = inFile.ReadLine();
                    string[] subTexts = text.Split(new string[] { ":" }, StringSplitOptions.None);

                    if (subTexts[0] == "LevelsUnlocked")
                        levelsUnlocked = Convert.ToInt32(subTexts[1]);
                    else if (subTexts[0] == "HasRainbowBase")
                        hasRainbowBase = Convert.ToBoolean(subTexts[1]);
                    else if (subTexts[0] == "HasRainbowGun")
                        hasRainbowGun = Convert.ToBoolean(subTexts[1]);
                    else if (subTexts[0] == "ColorR")
                        backGroundColor.R = Convert.ToByte(subTexts[1]);
                    else if (subTexts[0] == "ColorG")
                        backGroundColor.G = Convert.ToByte(subTexts[1]);
                    else if (subTexts[0] == "ColorB")
                        backGroundColor.B = Convert.ToByte(subTexts[1]);
                }

                inFile.Close();
            }
        }

        //Saves game data to a file
        public static void Save() 
        {
            StreamWriter outFile = File.CreateText("SaveData.txt");

            outFile.WriteLine("LevelsUnlocked:" + levelsUnlocked);
            outFile.WriteLine("HasRainbowBase:" + hasRainbowBase);
            outFile.WriteLine("HasRainbowGun:" + hasRainbowGun);
            outFile.WriteLine("ColorR:" + backGroundColor.R);
            outFile.WriteLine("ColorG:" + backGroundColor.G);
            outFile.WriteLine("ColorB:" + backGroundColor.B);

            outFile.Close();
        }
    }
}
