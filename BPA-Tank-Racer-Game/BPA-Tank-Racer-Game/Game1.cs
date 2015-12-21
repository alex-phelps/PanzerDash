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
    }
}
