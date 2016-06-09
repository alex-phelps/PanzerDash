using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PanzerDash
{
    public class Camera
    {
        public Matrix transform;
        Vector2 center;

        public Camera()
        {
        }

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X - Game1.WindowWidth / 4, position.Y - Game1.WindowHeight / 2);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
