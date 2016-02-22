
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Basic game object to hold a background
    /// </summary>
    public class Background : GameObject
    {
        public Background(Texture2D texture) 
            : base(texture)
        {
            position = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);
        }
    }
}
