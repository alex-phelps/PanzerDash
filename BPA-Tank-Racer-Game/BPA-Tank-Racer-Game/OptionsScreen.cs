using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    public class OptionsScreen : Screen
    {
        private KeyboardState oldState;

        private Texture2D backButton, backButtonDefault, backButtonSelected;
        private Texture2D soundButton, soundButtonDefault, soundButtonSelected;
        private Texture2D creditsButton, creditsButtonDefault, creditsButtonSelected;
        private Texture2D colorButton, colorButtonDefault, colorButtonSelected;
        private Texture2D resetButton, resetButtonDefault, resetButtonSelected;

        private Texture2D logo;

        public int selectedButton { get; private set; }

        public OptionsScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 0;
            oldState = Keyboard.GetState();

            logo = content.Load<Texture2D>("Options");

            soundButtonDefault = content.Load<Texture2D>("Sound");
            soundButtonSelected = content.Load<Texture2D>("Sound-Selected");
            colorButtonDefault = content.Load<Texture2D>("Color");
            colorButtonSelected = content.Load<Texture2D>("Color-Selected");
            creditsButtonDefault = content.Load<Texture2D>("Credits");
            creditsButtonSelected = content.Load<Texture2D>("Credits-Selected");
            backButtonDefault = content.Load<Texture2D>("Back");
            backButtonSelected = content.Load<Texture2D>("Back-Selected");
            resetButtonDefault = content.Load<Texture2D>("Reset");
            resetButtonSelected = content.Load<Texture2D>("Reset-Selected");

            soundButton = soundButtonSelected;
            colorButton = colorButtonDefault;
            creditsButton = creditsButtonDefault;
            backButton = backButtonDefault;
            resetButton = resetButtonDefault;
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            //Keyboard logic
            if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                //Change selected button

                if (selectedButton == 0)
                    soundButton = soundButtonDefault;
                else if (selectedButton == 1)
                    colorButton = colorButtonDefault;
                else if (selectedButton == 2)
                    creditsButton = creditsButtonDefault;
                else if (selectedButton == 3)
                    resetButton = resetButtonDefault;
                else if (selectedButton == 4)
                    backButton = backButtonDefault;


                selectedButton++;
                selectedButton %= 5;

                if (selectedButton == 0)
                    soundButton = soundButtonSelected;
                else if (selectedButton == 1)
                    colorButton = colorButtonSelected;
                else if (selectedButton == 2)
                    creditsButton = creditsButtonSelected;
                else if (selectedButton == 3)
                    resetButton = resetButtonSelected;
                else if (selectedButton == 4)
                    backButton = backButtonSelected;
            }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                //Change selected button

                if (selectedButton == 0)
                    soundButton = soundButtonDefault;
                else if (selectedButton == 1)
                    colorButton = colorButtonDefault;
                else if (selectedButton == 2)
                    creditsButton = creditsButtonDefault;
                else if (selectedButton == 3)
                    resetButton = resetButtonDefault;
                else if (selectedButton == 4)
                    backButton = backButtonDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 4;

                if (selectedButton == 0)
                    soundButton = soundButtonSelected;
                else if (selectedButton == 1)
                    colorButton = colorButtonSelected;
                else if (selectedButton == 2)
                    creditsButton = creditsButtonSelected;
                else if (selectedButton == 3)
                    resetButton = resetButtonSelected;
                else if (selectedButton == 4)
                    backButton = backButtonSelected;
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
            spritebatch.Draw(soundButton, new Vector2(Game1.WindowWidth / 2, 150), new Rectangle(0, 0, soundButton.Width, soundButton.Height),
                Color.White, 0, new Vector2(soundButton.Width / 2, soundButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorButton, new Vector2(Game1.WindowWidth / 2, 200), new Rectangle(0, 0, colorButton.Width, colorButton.Height),
                Color.White, 0, new Vector2(colorButton.Width / 2, colorButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(creditsButton, new Vector2(Game1.WindowWidth / 2, 250), new Rectangle(0, 0, creditsButton.Width, creditsButton.Height),
                Color.White, 0, new Vector2(creditsButton.Width / 2, creditsButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(resetButton, new Vector2(Game1.WindowWidth / 2, 300), new Rectangle(0, 0, resetButton.Width, resetButton.Height),
                Color.White, 0, new Vector2(resetButton.Width / 2, resetButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(backButton, new Vector2(Game1.WindowWidth / 2, 350), new Rectangle(0, 0, backButton.Width, backButton.Height),
                Color.White, 0, new Vector2(backButton.Width / 2, backButton.Height / 2), 1, SpriteEffects.None, 1f);
        }
    }
}
