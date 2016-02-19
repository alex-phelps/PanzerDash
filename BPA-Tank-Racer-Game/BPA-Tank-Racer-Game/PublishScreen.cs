﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public class PublishScreen : Screen
    {
        private Background logo;

        public PublishScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            logo = new Background(content.Load<Texture2D>("PublishCredits"));
        }

        public override void Update(GameTime gametime)
        {
            if (gametime.TotalGameTime.TotalSeconds >= 4 || Keyboard.GetState().IsKeyDown(Keys.Enter))
                screenEvent.Invoke(this, new EventArgs());
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            logo.Draw(spritebatch);
        }
    }
}
