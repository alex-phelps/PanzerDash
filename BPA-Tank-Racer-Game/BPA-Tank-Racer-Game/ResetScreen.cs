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
    public class ResetScreen : Screen
    {
        private Texture2D logo;
        private Texture2D yesButton, yesButtonDefault, yesButtonSelected;
        private Texture2D noButton, noButtonDefault, noButtonSelected;

        private KeyboardState oldState;

        public int selectedButton { get; private set; }

        public ResetScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 0;
            oldState = Keyboard.GetState();

            logo = content.Load<Texture2D>("ResetQuestion");

            yesButtonDefault = content.Load<Texture2D>("Yes");
            noButtonDefault = content.Load<Texture2D>("No");

            yesButtonSelected = content.Load<Texture2D>("Yes-Selected");
            noButtonSelected = content.Load<Texture2D>("No-Selected");

            yesButton = yesButtonDefault;
            noButton = noButtonSelected;
        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                if (selectedButton == 0)
                    noButton = noButtonDefault;
                else if (selectedButton == 1)
                    yesButton = yesButtonDefault;

                selectedButton--;
                if (selectedButton < 0)
                    selectedButton = 1;

                if (selectedButton == 0)
                    noButton = noButtonSelected;
                else if (selectedButton == 1)
                    yesButton = yesButtonSelected;
            }

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                if (selectedButton == 0)
                    noButton = noButtonDefault;
                else if (selectedButton == 1)
                    yesButton = yesButtonDefault;

                selectedButton++;
                selectedButton %= 2;

                if (selectedButton == 0)
                    noButton = noButtonSelected;
                else if (selectedButton == 1)
                    yesButton = yesButtonSelected;
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

            //Draw buttons
            spritebatch.Draw(noButton, new Vector2(Game1.WindowWidth / 3, Game1.WindowHeight / 2), new Rectangle(0, 0, noButton.Width, noButton.Height),
                Color.White, 0, new Vector2(noButton.Width / 2, noButton.Height / 2), 1, SpriteEffects.None, 1f);
            spritebatch.Draw(yesButton, new Vector2(Game1.WindowWidth * (2f / 3f), Game1.WindowHeight / 2), new Rectangle(0, 0, yesButton.Width, yesButton.Height),
                Color.White, 0, new Vector2(yesButton.Width / 2, yesButton.Height / 2), 1, SpriteEffects.None, 1f);
        }
    }
}
