using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class FreeModeScreen : Screen
    {
        Screen currentScreen;
        PlayerSelectionScreen playerSelectionScreen;

        public FreeModeScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
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
            throw new NotImplementedException();
        }
    }
}
