using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PanzerDash
{
    public class MultiplayerControlsScreen : Screen
    {
        public MultiplayerGameScreen screen { get; private set; }

        private SpriteFont bigFont;
        private SpriteFont smallFont;
        private KeyboardState oldState;

        public MultiplayerControlsScreen(ContentManager content, MultiplayerGameScreen screen,EventHandler screenEvent)
            : base(screenEvent)
        {
            bigFont = content.Load<SpriteFont>("WinTextFont");
            smallFont = content.Load<SpriteFont>("PopupText");
            this.screen = screen;

            oldState = Keyboard.GetState();
        }

        public override void Update(GameTime gametime)
        {
            List<Keys> keysPressed = new List<Keys>();

            //Find all pressed keys
            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
            {
                keysPressed.Add(key);
            }

            //Subtract any keys that were pressed when the screen was created
            foreach (Keys key in oldState.GetPressedKeys())
            {
                keysPressed.Remove(key);
            }

            //If any key is pressed
            if (keysPressed.Count != 0)
                screenEvent.Invoke(this, new EventArgs());

            oldState = Keyboard.GetState();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //Create temp variables to hold values used repeatedly
            string text = "Controls";
            SpriteFont font = bigFont;

            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 2) - font.MeasureString(text).X / 2, 5), Color.White);

            //Player 1 controls
            text = "Player 1";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 45), Color.White);
            font = smallFont;
            text = "Forward: W";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 110), Color.White);
            text = "Backward: S";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 160), Color.White);
            text = "Turn Left: A";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 210), Color.White);
            text = "Turn Right: D";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 260), Color.White);
            text = "Turn Turret Left: F";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 310), Color.White);
            text = "Turn Turret Right: G";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 360), Color.White);
            text = "Shoot: C";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth / 4) - font.MeasureString(text).X / 2, 410), Color.White);


            //Player 2 controls
            font = bigFont;
            text = "Player 2";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 45), Color.White);
            font = smallFont;
            text = "Forward: Up";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 110), Color.White);
            text = "Backward: Down";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 160), Color.White);
            text = "Turn Left: Left";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 210), Color.White);
            text = "Turn Right: Right";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 260), Color.White);
            text = "Turn Turret Left: <";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 310), Color.White);
            text = "Turn Turret Right: >";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 360), Color.White);
            text = "Shoot: Ctrl";
            spritebatch.DrawString(font, text,
                        new Vector2((Game1.WindowWidth * 3 / 4) - font.MeasureString(text).X / 2, 410), Color.White);
        }
    }
}
