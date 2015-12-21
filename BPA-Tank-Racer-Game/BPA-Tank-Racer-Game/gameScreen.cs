using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Game state for the main game
    /// </summary>
    public class GameScreen : Screen
    {
        private PlayerTank playerTank;
        private Background background;

        private KeyboardState oldState = Keyboard.GetState();

        public GameScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            playerTank = new PlayerTank(content, TankBaseType.basic, TankGunType.basic);

            background = new Background(content.Load<Texture2D>("TempBackground"));
        }

        public override void Update(GameTime gametime)
        {
            playerTank.Update();
            background.position -= playerTank.velocity;

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            background.Draw(spritebatch);
            playerTank.Draw(spritebatch);

            base.Draw(spritebatch);
        }
    }
}
