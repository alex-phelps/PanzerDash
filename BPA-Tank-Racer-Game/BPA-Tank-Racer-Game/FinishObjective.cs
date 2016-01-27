using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class FinishObjective : GameObject
    {
        public double baseHealth = 7;
        public double playerHealth;
        public double enemyHealth;
        private Texture2D healthBar;

        public FinishObjective(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("TempThing"))
        {
            this.position = position;
            healthBar = content.Load<Texture2D>("FinishHealthBar");

            playerHealth = baseHealth;
            enemyHealth = baseHealth;
        }

        public void DrawHUD(SpriteBatch spritebatch)
        {
            Rectangle healthBarSource = new Rectangle(0, 0,
                (int)(healthBar.Width * ((float)playerHealth / (float)baseHealth)), healthBar.Height);

            spritebatch.Draw(healthBar, new Vector2(Game1.WindowWidth / 2, 30),
                healthBarSource, Color.White, 0, new Vector2(healthBar.Width / 2, healthBar.Height / 2),
                1, SpriteEffects.None, 1);
        }
    }
}
