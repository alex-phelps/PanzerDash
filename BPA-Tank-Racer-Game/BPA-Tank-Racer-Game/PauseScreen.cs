using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BPA_Tank_Racer_Game
{
    public class PauseScreen : Screen
    {
        public int selectedButton { get; private set; }

        private KeyboardState oldState;

        private Texture2D logo;
        private Texture2D backButton, backButtonDefault, backButtonSelected;
        private Texture2D optionsButton, optionsButtonDefault, optionsButtonSelected;
        private Texture2D quitButton, quitButtonDefault, quitButtonSelected;

        public PauseScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            MediaPlayer.Pause();

            selectedButton = 0;
            oldState = Keyboard.GetState();

            logo = content.Load<Texture2D>("Paused");

            backButtonDefault = content.Load<Texture2D>("Back");
            backButtonSelected = content.Load<Texture2D>("Back-Selected");
            optionsButtonDefault = content.Load<Texture2D>("Options");
            optionsButtonSelected = content.Load<Texture2D>("Options-Selected");
            quitButtonDefault = content.Load<Texture2D>("Quit");
            quitButtonSelected = content.Load<Texture2D>("Quit-Selected");

            backButton = backButtonSelected;
            optionsButton = optionsButtonDefault;
            quitButton = quitButtonDefault;
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            //Keyboard logic
            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                Game1.selectFX.Play();

                //Change selected button

                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    optionsButton = optionsButtonDefault;
                else if (selectedButton == 2)
                    quitButton = quitButtonDefault;


                selectedButton++;
                selectedButton %= 3;

                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    optionsButton = optionsButtonSelected;
                else if (selectedButton == 2)
                    quitButton = quitButtonSelected;
            }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                Game1.selectFX.Play();

                //Change selected button

                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    optionsButton = optionsButtonDefault;
                else if (selectedButton == 2)
                    quitButton = quitButtonDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 2;

                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    optionsButton = optionsButtonSelected;
                else if (selectedButton == 2)
                    quitButton = quitButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                screenEvent.Invoke(this, new EventArgs());

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Draw logo
            spritebatch.Draw(logo, new Vector2(Game1.WindowWidth / 2, 30), new Rectangle(0, 0, logo.Width, logo.Height),
                Color.White, 0, new Vector2(logo.Width / 2, logo.Height / 2), 1.3f, SpriteEffects.None, 1f);

            //Draw selection buttons
            spritebatch.Draw(backButton, new Vector2(Game1.WindowWidth / 2, 200), new Rectangle(0, 0, backButton.Width, backButton.Height),
                Color.White, 0, new Vector2(backButton.Width / 2, backButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(optionsButton, new Vector2(Game1.WindowWidth / 2, 250), new Rectangle(0, 0, optionsButton.Width, optionsButton.Height),
                Color.White, 0, new Vector2(optionsButton.Width / 2, optionsButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(quitButton, new Vector2(Game1.WindowWidth / 2, 300), new Rectangle(0, 0, quitButton.Width, quitButton.Height),
                Color.White, 0, new Vector2(quitButton.Width / 2, quitButton.Height / 2), 1, SpriteEffects.None, 1f);
        }
    }
}
