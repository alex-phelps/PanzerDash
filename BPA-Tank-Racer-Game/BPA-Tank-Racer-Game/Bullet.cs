using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PanzerDash
{
    /// <summary>
    /// Represents a bullet shot by a tank
    /// </summary>
    public class Bullet : GameObject
    {
        public Vector2 velocity;
        public double damage { get; private set; }
        public int speed { get; private set; }
        public Tank ownerTank { get; private set; }
        public bool active { get; private set; }

        public int secToDestruction = 1;

        private Texture2D explosionTexture;

        public Bullet(Texture2D texture, Texture2D explosionTexture, Vector2 velocity, Vector2 position, 
            float rotation, double damage, int speed, Tank ownerTank) 
            : base(texture)
        {
            this.velocity.X = velocity.X * speed;
            this.velocity.Y = velocity.Y * -speed;
            this.position = position;
            this.rotation = rotation;
            this.damage = damage;
            this.explosionTexture = explosionTexture;
            this.ownerTank = ownerTank;

            active = true;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }

        /// <summary>
        /// Turns the bullet into an explosion
        /// </summary>
        public void Explode()
        {
            active = false;
            texture = explosionTexture;

            velocity = Vector2.Zero;
            speed = 0;
            damage = 0; ;
        }
    }
}
