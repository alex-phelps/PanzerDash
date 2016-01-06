using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public class BulletHandler
    {
        List<Bullet> bullets;

        public BulletHandler()
        {
            bullets = new List<Bullet>();
        }

        public void NewBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Moves all the bullets in this bullet handler by a specified amount
        /// </summary>
        /// <param name="shiftVar">Vector2 to move the bullets by</param>
        public void MoveBullets(Vector2 shiftVar)
        {
            foreach (Bullet bullet in bullets) 
            {
                bullet.position += shiftVar;
            }
        }
    }
}
