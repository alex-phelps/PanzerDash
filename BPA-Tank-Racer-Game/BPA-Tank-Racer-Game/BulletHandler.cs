using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Update()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
