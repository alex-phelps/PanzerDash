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
        desert,
        jungle,
        urban,
        snow,
        red,
        rainbow
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
        private Texture2D bulletExplosionTexture;
        
        private TankBase tankBase;
        private TankGun tankGun;

        private  TimeSpan oldTime;

        public Vector2 velocity;
        public float speed;
        public float accel;
        public float maxSpeed;
        public float rotSpeed;
        public float maxRotSpeed;
        public float rotAccel;
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
            bulletExplosionTexture = content.Load<Texture2D>("BulletExplosion");
            
            //Temp
            maxRotSpeed = 0.05f;
            rotAccel = 0.0025f;


            //Assign base
            if (baseType == TankPartType.red) //Red
            {
                tankBase = new TankBase(content.Load<Texture2D>("RedTankBase"));

                accel = 0.04f;
                maxSpeed = 3.3f;
            }
            else if (baseType == TankPartType.desert) //Desert
            {
                tankBase = new TankBase(content.Load<Texture2D>("desertTankBase"));

                accel = 0.03f;
                maxSpeed = 2.5f;
            }
            else if (baseType == TankPartType.snow) //Snow
            {
                tankBase = new TankBase(content.Load<Texture2D>("SnowTankBase"));

                accel = 0.06f;
                maxSpeed = 2.2f;
            }
            else if (baseType == TankPartType.urban) //Urban
            {
                tankBase = new TankBase(content.Load<Texture2D>("UrbanTankBase"));

                accel = 0.025f;
                maxSpeed = 1.6f;
            }
            else if (baseType == TankPartType.jungle) //Jungle
            {
                tankBase = new TankBase(content.Load<Texture2D>("JungleTankBase"));

                accel = 0.15f;
                maxSpeed = 2.8f;
            }
            else if (baseType == TankPartType.rainbow) //RAINBOW
            {
                tankBase = new TankBase(content.Load<Texture2D>("RainbowTankBase"));

                accel = 0.4f;
                maxSpeed = 8;

                maxRotSpeed = 0.25f;
                rotAccel = 0.005f;
            }
            else //Basic
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));

                accel = 0.1f;
                maxSpeed = 2;

                maxRotSpeed = 0.05f;
                rotAccel = 0.0025f;
            }


            //Assign gun
            if (gunType == TankPartType.red) //Red
            {
                tankGun = new TankGun(content.Load<Texture2D>("RedTankGun"));

                baseCooldown = 2;
                gunDamage = 2;
                bulletSpeed = 12;
            } 
            else if (gunType == TankPartType.desert) //Desert
            {
                tankGun = new TankGun(content.Load<Texture2D>("desertTankGun"));

                baseCooldown = 6;
                gunDamage = 7;
                bulletSpeed = 6;
            }
            else if (gunType == TankPartType.snow) //Snow
            {
                tankGun = new TankGun(content.Load<Texture2D>("SnowTankGun"));

                baseCooldown = 4;
                gunDamage = 5;
                bulletSpeed = 9;
            }
            else if (gunType == TankPartType.urban) //Urban
            {
                tankGun = new TankGun(content.Load<Texture2D>("UrbanTankGun"));

                baseCooldown = 8;
                gunDamage = 12;
                bulletSpeed = 4;
            }
            else if (gunType == TankPartType.jungle) //Jungle
            {
                tankGun = new TankGun(content.Load<Texture2D>("JungleTankGun"));

                baseCooldown = 3;
                gunDamage = 4;
                bulletSpeed = 15;
            }
            else if (gunType == TankPartType.rainbow) //RAINBOW
            {
                tankGun = new TankGun(content.Load<Texture2D>("RainbowTankGun"));

                baseCooldown = 0;
                gunDamage = 20;
                bulletSpeed = 20;
            }
            else //Basic
            {
                tankGun = new TankGun(content.Load<Texture2D>("basicTankGun"));

                baseCooldown = 5;
                gunDamage = 5;
                bulletSpeed = 7;
            }

            //Set GameObject texture
            texture = tankBase.texture;

            currentCooldown = baseCooldown;
        }

        public override void Update(GameTime gametime)
        {
            if (speed > maxSpeed)
                speed = maxSpeed;
            else if (speed < -maxSpeed)
                speed = -maxSpeed;

            if (rotSpeed > maxRotSpeed)
                rotSpeed = maxRotSpeed;
            else if (rotSpeed < -maxRotSpeed)
                rotSpeed = -maxRotSpeed;

            //If rot > 360 degrees, reset it
            rotation %= (float)(Math.PI * 2);
            gunRotation %= (float)(Math.PI * 2);

            rotation += rotSpeed;
            gunRotation += rotSpeed;

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
                bulletHandler.NewBullet(new Bullet(bulletTexture, bulletExplosionTexture,
                    bulletVelocity, position, gunRotation, gunDamage, bulletSpeed, this));

                currentCooldown = baseCooldown;
            }
        }
    }
}
