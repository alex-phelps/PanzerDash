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
        public int damage { get; private set; }
        public int speed { get; private set; }

        public Bullet(Texture2D texture, Vector2 velocity, Vector2 position, float rotation, int damage, int speed) 
            : base(texture)
        {
            this.velocity.X = velocity.X * speed;
            this.velocity.Y = velocity.Y * -speed;
            this.position = position;
            this.rotation = rotation;
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }
    }
}
