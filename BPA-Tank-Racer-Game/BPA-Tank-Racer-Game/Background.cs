using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public class Background : GameObject
    {
        public Background(Texture2D texture) 
            : base(texture)
        {
            position = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);
        }
    }
}
