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
    public class ColorScreen : Screen
    {
        private Texture2D colorBarR, colorBarG, colorBarB;
        private Texture2D colorBarBackgroundR, colorBarBackgroundG, colorBarBackgroundB;
        private Texture2D colorBarDefault, colorBarSelected;
        private Texture2D backButton, backButtonDefault, backButtonSelected;

        private Texture2D colorPickerR, colorPickerG, colorPickerB;

        private Texture2D logo;

        private KeyboardState oldState;

        private int selectedButton = 1;

        public int colorR { get; private set; }
        public int colorG { get; private set; }
        public int colorB { get; private set; }

        public ColorScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            logo = content.Load<Texture2D>("Color");

            colorR = Game1.backGroundColor.R;
            colorG = Game1.backGroundColor.G;
            colorB = Game1.backGroundColor.B;

            backButtonDefault = content.Load<Texture2D>("Back");
            colorBarDefault = content.Load<Texture2D>("ColorBar");

            backButtonSelected = content.Load<Texture2D>("Back-Selected");
            colorBarSelected = content.Load<Texture2D>("ColorBar-Selected");

            colorBarBackgroundR = content.Load<Texture2D>("ColorBarRed");
            colorBarBackgroundG = content.Load<Texture2D>("ColorBarGreen");
            colorBarBackgroundB = content.Load<Texture2D>("ColorBarBlue");

            Texture2D colorPicker = content.Load<Texture2D>("ColorPicker");
            colorPickerR = colorPicker;
            colorPickerG = colorPicker;
            colorPickerB = colorPicker;

            backButton = backButtonDefault;
            colorBarR = colorBarSelected;
            colorBarG = colorBarDefault;
            colorBarB = colorBarDefault;
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                Game1.selectFX.Play();

                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    colorBarR = colorBarDefault;
                else if (selectedButton == 2)
                    colorBarG = colorBarDefault;
                else if (selectedButton == 3)
                    colorBarB = colorBarDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 3;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    colorBarR = colorBarSelected;
                else if (selectedButton == 2)
                    colorBarG = colorBarSelected;
                else if (selectedButton == 3)
                    colorBarB = colorBarSelected;
            }

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                Game1.selectFX.Play();

                //Set old button to not selected icon
                if (selectedButton == 0)
                    backButton = backButtonDefault;
                else if (selectedButton == 1)
                    colorBarR = colorBarDefault;
                else if (selectedButton == 2)
                    colorBarG = colorBarDefault;
                else if (selectedButton == 3)
                    colorBarB = colorBarDefault;

                selectedButton++;
                selectedButton %= 4;

                //Set new button to selected icon
                if (selectedButton == 0)
                    backButton = backButtonSelected;
                else if (selectedButton == 1)
                    colorBarR = colorBarSelected;
                else if (selectedButton == 2)
                    colorBarG = colorBarSelected;
                else if (selectedButton == 3)
                    colorBarB = colorBarSelected;
            }

            if (newState.IsKeyDown(Keys.Up))
            {
                if (selectedButton == 1)
                {
                    if (colorR < 255)
                        colorR++;
                }
                else if (selectedButton == 2)
                {
                    if (colorG < 255)
                        colorG++;
                }
                else if (selectedButton == 3)
                {
                    if (colorB < 255)
                        colorB++;
                }
            }

            if (newState.IsKeyDown(Keys.Down))
            {
                if (selectedButton == 1)
                {
                    if (colorR > 0)
                        colorR--;
                }
                else if (selectedButton == 2)
                {
                    if (colorG > 0)
                        colorG--;
                }
                else if (selectedButton == 3)
                {
                    if (colorB > 0)
                        colorB--;
                }
            }

            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter) && selectedButton == 0)
                screenEvent.Invoke(this, new EventArgs());

            //Set background color
            Game1.backGroundColor.R = (byte)colorR;
            Game1.backGroundColor.G = (byte)colorG;
            Game1.backGroundColor.B = (byte)colorB;

            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Draw logo
            spritebatch.Draw(logo, new Vector2(Game1.WindowWidth / 2, 25), new Rectangle(0, 0,
               logo.Width, logo.Height), Color.White, 0, new Vector2(logo.Width / 2, logo.Height / 2),
               1.3f, SpriteEffects.None, 1f);

            //Draw back button
            spritebatch.Draw(backButton, new Vector2(120, Game1.WindowHeight - 40), new Rectangle(0, 0,
                backButton.Width, backButton.Height), Color.White, 0, new Vector2(backButton.Width / 2, backButton.Height / 2),
                1, SpriteEffects.None, 1f);

            //Draw Color Backgrounds
            spritebatch.Draw(colorBarBackgroundR, new Vector2(Game1.WindowWidth * (1f / 5f), Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarBackgroundR.Width, colorBarBackgroundR.Height), Color.White, 0, new Vector2(colorBarBackgroundR.Width / 2, colorBarBackgroundR.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorBarBackgroundG, new Vector2(Game1.WindowWidth * 0.5f, Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarBackgroundG.Width, colorBarBackgroundG.Height), Color.White, 0, new Vector2(colorBarBackgroundG.Width / 2, colorBarBackgroundG.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorBarBackgroundB, new Vector2(Game1.WindowWidth * (4f / 5f), Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarBackgroundB.Width, colorBarBackgroundB.Height), Color.White, 0, new Vector2(colorBarBackgroundB.Width / 2, colorBarBackgroundB.Height / 2),
                1, SpriteEffects.None, 1f);

            //Draw Color Bars
            spritebatch.Draw(colorBarR, new Vector2(Game1.WindowWidth * (1f / 5f), Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarR.Width, colorBarR.Height), Color.White, 0, new Vector2(colorBarR.Width / 2, colorBarR.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorBarG, new Vector2(Game1.WindowWidth * 0.5f, Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarG.Width, colorBarG.Height), Color.White, 0, new Vector2(colorBarG.Width / 2, colorBarG.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorBarB, new Vector2(Game1.WindowWidth * (4f / 5f), Game1.WindowHeight / 2), new Rectangle(0, 0,
                colorBarB.Width, colorBarB.Height), Color.White, 0, new Vector2(colorBarB.Width / 2, colorBarB.Height / 2),
                1, SpriteEffects.None, 1f);

            //Draw Color Pickers
            spritebatch.Draw(colorPickerR, new Vector2(Game1.WindowWidth * (1f / 5f), (Game1.WindowHeight / 2 + (colorBarDefault.Height / 2 - 4)) - colorR), new Rectangle(0, 0,
                colorPickerR.Width, colorPickerR.Height), Color.White, 0, new Vector2(colorPickerR.Width / 2, colorPickerR.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorPickerG, new Vector2(Game1.WindowWidth * 0.5f, (Game1.WindowHeight / 2 + (colorBarDefault.Height / 2 - 4)) - colorG), new Rectangle(0, 0,
                colorPickerG.Width, colorPickerG.Height), Color.White, 0, new Vector2(colorPickerG.Width / 2, colorPickerG.Height / 2),
                1, SpriteEffects.None, 1f);
            spritebatch.Draw(colorPickerB, new Vector2(Game1.WindowWidth * (4f / 5f), (Game1.WindowHeight / 2 + (colorBarDefault.Height / 2 - 4)) - colorB), new Rectangle(0, 0,
                colorPickerB.Width, colorPickerB.Height), Color.White, 0, new Vector2(colorPickerB.Width / 2, colorPickerB.Height / 2),
                1, SpriteEffects.None, 1f);
        }
    }
}
