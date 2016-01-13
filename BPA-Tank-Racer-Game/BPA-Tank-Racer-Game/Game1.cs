using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Screen currentScreen;
        MenuScreen menuScreen;

        public static int WindowWidth = 800;
        public static int WindowHeight = 480;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;

            graphics.ApplyChanges();
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
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuScreen = new MenuScreen(Content, new EventHandler(MenuScreenEvent));

            currentScreen = menuScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
            if (menuScreen.selectedButton == 0)
            {
                currentScreen = new GameScreen(Content, new EventHandler(GameScreenEvent), 1);
            }
            else if (menuScreen.selectedButton == 1)
            {

            }
            else if (menuScreen.selectedButton == 2)
            {

            }
            else if (menuScreen.selectedButton == 3)
            {

            }
            else if (menuScreen.selectedButton == 4)
            {

            }
            else Environment.Exit(0);
        }

        private void GameScreenEvent(object sender, EventArgs e)
        {
            currentScreen = menuScreen;
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
        /// Checks if an object is colliding with a specific collor in another object
        /// </summary>
        /// <param name="objA">An object to check collision for</param>
        /// <param name="objB">An object to check for collision of the color in</param>
        /// <param name="color">The collor to check collision against</param>
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
    }
}
