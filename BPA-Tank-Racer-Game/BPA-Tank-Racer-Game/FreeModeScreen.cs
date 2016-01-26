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
    public class FreeModeScreen : Screen
    {
        private Screen currentScreen;
        private PlayerSelectionScreen playerSelectionScreen;

        public bool gameReady { get; private set; }

        public FreeModeScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            gameReady = false;

            playerSelectionScreen = new PlayerSelectionScreen(content, new EventHandler(PlayerSelectionScreenEvent));

            currentScreen = playerSelectionScreen;
        }

        public override void Update(GameTime gametime)
        {
            currentScreen.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            currentScreen.Draw(spritebatch);
        }

        private void PlayerSelectionScreenEvent(object sender, EventArgs e)
        {
            if (playerSelectionScreen.selectedButton == 0) //Back
                screenEvent.Invoke(this, new EventArgs());
            else if (playerSelectionScreen.selectedButton == 3) // Confirm
            { } // Temp
        }
    }
}
