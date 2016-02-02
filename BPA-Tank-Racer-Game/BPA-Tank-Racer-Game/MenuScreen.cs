using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Game state for the main menu
    /// </summary>
    public class MenuScreen : Screen
    {
        private Texture2D logo;

        private Texture2D playNowButton, playNowButtonDefault, playNowButtonSelected;
        private Texture2D careerModeButton, careerModeButtonDefault, careerModeButtonSelected;
        private Texture2D freeModeButton, freeModeButtonDefault, freeModeButtonSelected;
        private Texture2D optionsButton, optionsButtonDefault, optionsButtonSelected;
        private Texture2D quitButton, quitButtonDefault, quitButtonSelected;
        private Texture2D tutorialButton, tutorialButtonDefault, tutorialButtonSelected;

        private KeyboardState oldState = Keyboard.GetState();

        public int selectedButton { get; private set; }

        public MenuScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            //Load all title text images

            logo = content.Load<Texture2D>("Logo");

            playNowButtonDefault = content.Load<Texture2D>("PlayNow");
            tutorialButtonDefault = content.Load<Texture2D>("Tutorial");
            careerModeButtonDefault = content.Load<Texture2D>("CareerMode");
            freeModeButtonDefault = content.Load<Texture2D>("FreeMode");
            optionsButtonDefault = content.Load<Texture2D>("Options");
            quitButtonDefault = content.Load<Texture2D>("Quit");

            playNowButtonSelected = content.Load<Texture2D>("PlayNow-Selected");
            tutorialButtonSelected = content.Load<Texture2D>("Tutorial-Selected");
            careerModeButtonSelected = content.Load<Texture2D>("CareerMode-Selected");
            freeModeButtonSelected = content.Load<Texture2D>("FreeMode-Selected");
            optionsButtonSelected = content.Load<Texture2D>("Options-Selected");
            quitButtonSelected = content.Load<Texture2D>("Quit-Selected");

            playNowButton = playNowButtonSelected;
            tutorialButton = tutorialButtonDefault;
            careerModeButton = careerModeButtonDefault;
            freeModeButton = freeModeButtonDefault;
            optionsButton = optionsButtonDefault;
            quitButton = quitButtonDefault;

            //Default selected button
            selectedButton = 0;
        }


        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            //Keyboard logic
            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                //Change selected button

                if (selectedButton == 0)
                    playNowButton = playNowButtonDefault;
                else if (selectedButton == 1)
                    tutorialButton = tutorialButtonDefault;
                else if (selectedButton == 2)
                    careerModeButton = careerModeButtonDefault;
                else if (selectedButton == 3)
                    freeModeButton = freeModeButtonDefault;
                else if (selectedButton == 4)
                    optionsButton = optionsButtonDefault;
                else if (selectedButton == 5)
                    quitButton = quitButtonDefault;

                if (selectedButton == 5)
                    selectedButton = 0;
                else selectedButton++;

                if (selectedButton == 0)
                    playNowButton = playNowButtonSelected;
                else if (selectedButton == 1)
                    tutorialButton = tutorialButtonSelected;
                else if (selectedButton == 2)
                    careerModeButton = careerModeButtonSelected;
                else if (selectedButton == 3)
                    freeModeButton = freeModeButtonSelected;
                else if (selectedButton == 4)
                    optionsButton = optionsButtonSelected;
                else if (selectedButton == 5)
                    quitButton = quitButtonSelected;
            }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                //Change selected button

                if (selectedButton == 0)
                    playNowButton = playNowButtonDefault;
                else if (selectedButton == 1)
                    tutorialButton = tutorialButtonDefault;
                else if (selectedButton == 2)
                    careerModeButton = careerModeButtonDefault;
                else if (selectedButton == 3)
                    freeModeButton = freeModeButtonDefault;
                else if (selectedButton == 4)
                    optionsButton = optionsButtonDefault;
                else if (selectedButton == 5)
                    quitButton = quitButtonDefault;

                if (selectedButton == 0)
                    selectedButton = 5;
                else selectedButton--;

                if (selectedButton == 0)
                    playNowButton = playNowButtonSelected;
                else if (selectedButton == 1)
                    tutorialButton = tutorialButtonSelected;
                else if (selectedButton == 2)
                    careerModeButton = careerModeButtonSelected;
                else if (selectedButton == 3)
                    freeModeButton = freeModeButtonSelected;
                else if (selectedButton == 4)
                    optionsButton = optionsButtonSelected;
                else if (selectedButton == 5)
                    quitButton = quitButtonSelected;
            }
            
            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                screenEvent.Invoke(this, new EventArgs());

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Draw logo
            spritebatch.Draw(logo, new Vector2(Game1.WindowWidth / 2, 50), new Rectangle(0, 0, logo.Width, logo.Height),
                Color.White, 0, new Vector2(logo.Width / 2, logo.Height / 2), 1, SpriteEffects.None, 1f);

            //Draw selection buttons
            spritebatch.Draw(playNowButton, new Vector2(Game1.WindowWidth / 2, 150), new Rectangle(0, 0, playNowButton.Width, playNowButton.Height),
                Color.White, 0, new Vector2(playNowButton.Width / 2, playNowButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(tutorialButton, new Vector2(Game1.WindowWidth / 2, 200), new Rectangle(0, 0, tutorialButton.Width, tutorialButton.Height),
                Color.White, 0, new Vector2(tutorialButton.Width / 2, tutorialButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(careerModeButton, new Vector2(Game1.WindowWidth / 2, 250), new Rectangle(0, 0, careerModeButton.Width, careerModeButton.Height),
                Color.White, 0, new Vector2(careerModeButton.Width / 2, careerModeButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(freeModeButton, new Vector2(Game1.WindowWidth / 2, 300), new Rectangle(0, 0, freeModeButton.Width, freeModeButton.Height),
                Color.White, 0, new Vector2(freeModeButton.Width / 2, freeModeButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(optionsButton, new Vector2(Game1.WindowWidth / 2, 350), new Rectangle(0, 0, optionsButton.Width, optionsButton.Height),
                Color.White, 0, new Vector2(optionsButton.Width / 2, optionsButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(quitButton, new Vector2(Game1.WindowWidth / 2, 400), new Rectangle(0, 0, quitButton.Width, quitButton.Height),
                Color.White, 0, new Vector2(quitButton.Width / 2, quitButton.Height / 2), 1, SpriteEffects.None, 1f);
        }
    }
}
