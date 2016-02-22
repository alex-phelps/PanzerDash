using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BPA_Tank_Racer_Game
{
    public class CreditsScreen : Screen
    {
        private Background credits;
        private KeyboardState oldState;

        public CreditsScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            oldState = Keyboard.GetState();
            credits = new Background(content.Load<Texture2D>("CreditsList"));
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
            credits.Draw(spritebatch);
        }
    }
}
