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

        private int selectedButton;

        public OptionsScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedButton = 0;


        }

        public override void Update(GameTime gametime)
        {
            KeyboardState newState = Keyboard.GetState();



            oldState = newState;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            
        }
    }
}
