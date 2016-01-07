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
    /// Defines different types of tank parts
    /// </summary>
    public enum TankPartType
    {
        basic,
        red
    }

    /// <summary>
    /// Base class for a tank
    /// </summary>
    public class Tank : GameObject
    {
        TankPartType baseType;
        TankPartType gunType;

        private Texture2D bulletTexture;
        private BulletHandler bulletHandler;
        
        private TankBase tankBase;
        private TankGun tankGun;

        private  TimeSpan oldTime;

        public Vector2 velocity;
        public float speed;
        public float accel;
        public float maxSpeed;
        public float gunRotation;
        public int gunDamage;
        public int bulletSpeed;

        public int baseCooldown;
        public int currentCooldown;

        /// <summary>
        /// Creates a new tank
        /// </summary>
        /// <param name="content">A content manager object</param>
        /// <param name="bulletHandler">A BulletHandler to add bullets shot from this tank to</param>
        /// <param name="baseType">Type of tank base</param>
        /// <param name="gunType">Type of tank gun</param>
        public Tank(ContentManager content, BulletHandler bulletHandler, TankPartType baseType, TankPartType gunType)
            : base(content.Load<Texture2D>("basicTankBase"))
        {
            this.baseType = baseType;
            this.gunType = gunType;
            this.bulletHandler = bulletHandler;

            bulletTexture = content.Load<Texture2D>("Bullet");

            //Assign base
            if (baseType == TankPartType.red) //Red
            {
                tankBase = new TankBase(content.Load<Texture2D>("RedTankBase"));

                accel = 0.03f;
                maxSpeed = 3;
            }
            else //Basic
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));

                accel = 0.1f;
                maxSpeed = 2;
            }

            //Assign gun
            if (gunType == TankPartType.red) //Red
            {
                tankGun = new TankGun(content.Load<Texture2D>("RedTankGun"));

                baseCooldown = 2;
                gunDamage = 2;
                bulletSpeed = 12;
            }
            else //Basic
            {
                tankGun = new TankGun(content.Load<Texture2D>("basicTankGun"));

                baseCooldown = 5;
                gunDamage = 5;
                bulletSpeed = 7;
            }

            texture = tankBase.texture;
        }

        public override void Update(GameTime gametime)
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            tankBase.Draw(spriteBatch);
            tankGun.Draw(spriteBatch);
        }

        public void Shoot()
        {
            if (currentCooldown == 0)
            {
                Vector2 bulletVelocity = new Vector2((float)Math.Sin(gunRotation), (float)Math.Cos(gunRotation));
                bulletHandler.NewBullet(new Bullet(bulletTexture, bulletVelocity, position, gunRotation, gunDamage, bulletSpeed));

                currentCooldown = baseCooldown;
            }
        }
    }
}
