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
        public int secToDestruction = 2;

        private Texture2D explosionTexture;

        public Bullet(Texture2D texture, Texture2D explosionTexture, Vector2 velocity, Vector2 position, float rotation, int damage, int speed) 
            : base(texture)
        {
            this.velocity.X = velocity.X * speed;
            this.velocity.Y = velocity.Y * -speed;
            this.position = position;
            this.rotation = rotation;
            this.damage = damage;
            this.explosionTexture = explosionTexture;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }

        public void Explode()
        {
            texture = explosionTexture;

            velocity = Vector2.Zero;
            speed = 0;
            damage = 0; ;
        }
    }
}
