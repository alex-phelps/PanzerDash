using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Defines different types of tank bases
    /// </summary>
    public enum TankBaseType
    {
        basic
    }

    /// <summary>
    /// Defines different types of tank guns
    /// </summary>
    public enum TankGunType
    {
        basic
    }

    /// <summary>
    /// Base class for a tank
    /// </summary>
    public class Tank
    {
        TankBaseType baseType;
        TankGunType gunType;

        private Texture2D bulletTexture;
        private BulletHandler bulletHandler;
        
        private TankBase tankBase;
        private TankGun tankGun;

        private  TimeSpan oldTime;

        public Vector2 position;
        public Vector2 velocity;
        public float speed;
        public float accel;
        public float maxSpeed;
        public float rotation;
        public float gunRotation;

        public int baseCooldown;
        public int currentCooldown;

        public Tank(ContentManager content, TankBaseType baseType, TankGunType gunType)
        {
            this.baseType = baseType;
            this.gunType = gunType;

            bulletTexture = content.Load<Texture2D>("Bullet");
            bulletHandler = new BulletHandler();

            //Assign base
            if (baseType == TankBaseType.basic) //Temp; Basic
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));
            }
            else //Basic
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));
            }

            //Assign gun
            if (gunType == TankGunType.basic) //Temp; Basic
            {
                tankGun = new TankGun(content.Load<Texture2D>("basicTankGun"));

                baseCooldown = 5;
            }
            else //Basic
            {
                tankGun = new TankGun(content.Load<Texture2D>("basicTankGun"));

                baseCooldown = 5;
            }
        }

        public virtual void Update(GameTime gametime)
        {
            tankBase.position = position;
            tankBase.rotation = rotation;
            tankGun.position = position;
            tankGun.rotation = gunRotation;
            
            if (currentCooldown != 0 && gametime.TotalGameTime.TotalSeconds - 1 >= oldTime.TotalSeconds)
            {
                currentCooldown--;
                oldTime = gametime.TotalGameTime;
            }

            bulletHandler.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bulletHandler.Draw(spriteBatch);
            tankBase.Draw(spriteBatch);
            tankGun.Draw(spriteBatch);
        }

        public void Shoot()
        {
            if (currentCooldown == 0)
            {
                Vector2 bulletVelocity = new Vector2((float)Math.Sin(gunRotation) * 7, (float)Math.Cos(gunRotation) * -7);
                bulletHandler.NewBullet(new Bullet(bulletTexture, bulletVelocity, position, gunRotation));

                currentCooldown = baseCooldown;
            }
        }
    }
}
