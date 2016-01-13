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
        public List<Bullet> bullets;
        private List<Bullet> bulletsToDestroy;
        private List<Bullet> bulletsToDelete;

        private TimeSpan oldTime;

        public BulletHandler()
        {
            bullets = new List<Bullet>();
            bulletsToDestroy = new List<Bullet>();
        }

        public void NewBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public void Update(GameTime gameTime)
        {
            bulletsToDelete = new List<Bullet>();

            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
            }

            if (gameTime.TotalGameTime.TotalSeconds - 1 >= oldTime.TotalSeconds)
            {
                foreach (Bullet bullet in bulletsToDestroy)
                {
                    if (bullet.secToDestruction == 0)
                    {
                        bulletsToDelete.Add(bullet);
                    }
                    else
                    {
                        bullet.secToDestruction--;
                    }
                }

                oldTime = gameTime.TotalGameTime;
            }

            foreach (Bullet bullet in bulletsToDelete)
            {
                bulletsToDestroy.Remove(bullet);
                bullets.Remove(bullet);
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

        /// <summary>
        /// Sets a specified bullet to be destroyed
        /// </summary>
        /// <param name="bullet">Bullet to be destroyed</param>
        public void Destroy(Bullet bullet)
        {
            if (bullets.Contains(bullet))
            {
                bulletsToDestroy.Add(bullet);
                bullet.Explode();
            }
        }
    }
}
