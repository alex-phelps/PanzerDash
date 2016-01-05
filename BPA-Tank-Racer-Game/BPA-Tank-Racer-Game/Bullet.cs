using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public class Bullet : GameObject
    {
        public Vector2 velocity;

        public Bullet(Texture2D texture, Vector2 velocity, Vector2 position, float rotation) 
            : base(texture)
        {
            this.velocity = velocity;
            this.position = position;
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }
    }
}
