using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PanzerDash
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
        rainbow,
        none //Mostly usable for unlockables
    }

    /// <summary>
    /// Base class for a tank
    /// </summary>
    public class Tank : GameObject
    {
        public TankPartType baseType { get; private set; }
        public TankPartType gunType { get; private set; }

        private Texture2D bulletTexture;
        private BulletHandler bulletHandler;
        private Texture2D bulletExplosionTexture;
        private Texture2D shieldTexture;
        
        private TankBase tankBase;
        private TankGun tankGun;

        private TimeSpan oldCooldownTime;
        private TimeSpan oldStunTime;
        private TimeSpan powerupOldTime;

        public PowerUpType currentPowerupType { get; private set; }
        private float damageMod;
        private float speedMod;
        private float rapidCooldown;
        public double powerupTime;
        public double basePowerupTime;

        public Vector2 velocity;
        public float speed;
        public float accel;
        public float maxSpeed;
        public float rotSpeed;
        public float maxRotSpeed;
        public float rotAccel;
        public float gunRotation;
        public double gunDamage;
        public int bulletSpeed;

        public double baseCooldown;
        public double currentCooldown;

        public bool isStunned;
        public double stunLength;

        private SoundEffectInstance shootFX;

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

            shootFX = Game1.shootFX.CreateInstance();

            bulletTexture = content.Load<Texture2D>("Bullet");
            bulletExplosionTexture = content.Load<Texture2D>("BulletExplosion");
            shieldTexture = content.Load<Texture2D>("Shield");
            
            maxRotSpeed = 0.05f;
            rotAccel = 0.0025f;


            //Assign base
            if (baseType == TankPartType.red) //Red
            {
                tankBase = new TankBase(content.Load<Texture2D>("RedTankBase"));

                accel = 0.02f;
                maxSpeed = 1.3f;
            }
            else if (baseType == TankPartType.desert) //Desert
            {
                tankBase = new TankBase(content.Load<Texture2D>("desertTankBase"));

                accel = 0.02f;
                maxSpeed = 1.2f;
            }
            else if (baseType == TankPartType.snow) //Snow
            {
                tankBase = new TankBase(content.Load<Texture2D>("SnowTankBase"));

                accel = 0.03f;
                maxSpeed = 1.2f;
            }
            else if (baseType == TankPartType.urban) //Urban
            {
                tankBase = new TankBase(content.Load<Texture2D>("UrbanTankBase"));

                accel = 0.02f;
                maxSpeed = 0.9f;
            }
            else if (baseType == TankPartType.jungle) //Jungle
            {
                tankBase = new TankBase(content.Load<Texture2D>("JungleTankBase"));

                accel = 0.01f;
                maxSpeed = 1.4f;
            }
            else if (baseType == TankPartType.rainbow) //RAINBOW
            {
                tankBase = new TankBase(content.Load<Texture2D>("RainbowTankBase"));

                accel = 0.3f;
                maxSpeed = 8;

                maxRotSpeed = 0.25f;
                rotAccel = 0.005f;
            }
            else //Basic
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));

                accel = 0.05f;
                maxSpeed = 1;
            }


            //Assign gun
            if (gunType == TankPartType.red) //Red
            {
                tankGun = new TankGun(content.Load<Texture2D>("RedTankGun"));

                baseCooldown = 2;
                gunDamage = 0.5;
                bulletSpeed = 11;
            } 
            else if (gunType == TankPartType.desert) //Desert
            {
                tankGun = new TankGun(content.Load<Texture2D>("desertTankGun"));

                baseCooldown = 6;
                gunDamage = 2.2;
                bulletSpeed = 9;
            }
            else if (gunType == TankPartType.snow) //Snow
            {
                tankGun = new TankGun(content.Load<Texture2D>("SnowTankGun"));

                baseCooldown = 5;
                gunDamage = 2.6;
                bulletSpeed = 7;
            }
            else if (gunType == TankPartType.urban) //Urban
            {
                tankGun = new TankGun(content.Load<Texture2D>("UrbanTankGun"));

                baseCooldown = 9;
                gunDamage = 4.2f;
                bulletSpeed = 10;
            }
            else if (gunType == TankPartType.jungle) //Jungle
            {
                tankGun = new TankGun(content.Load<Texture2D>("JungleTankGun"));

                baseCooldown = 3;
                gunDamage = 1;
                bulletSpeed = 10;
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
                gunDamage = 1.7;
                bulletSpeed = 8;
            }

            //Set GameObject texture
            texture = tankBase.texture;

            currentCooldown = baseCooldown;
        }

        public override void Update(GameTime gametime)
        {
            if (speed > maxSpeed + speedMod)
                speed = maxSpeed + speedMod;
            else if (speed < -(maxSpeed + speedMod))
                speed = -(maxSpeed + speedMod);

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

            if (stunLength == 0)
            {
                if (isStunned)
                    isStunned = false;
            }
            else if (!isStunned)
            {
                isStunned = true;
                speed = 0;
                rotSpeed = 0;
                velocity = Vector2.Zero;

                //Reset timer
                oldStunTime = gametime.TotalGameTime;
            }  

            if (!isStunned)
            {
                if (currentCooldown != 0 && gametime.TotalGameTime.TotalSeconds - 0.2 >= oldCooldownTime.TotalSeconds)
                {
                    currentCooldown = Math.Round(currentCooldown - 0.2, 2);
                    oldCooldownTime = gametime.TotalGameTime;
                }
            }

            //Update stun
            if (stunLength != 0 && gametime.TotalGameTime.TotalSeconds - 0.1 >= oldStunTime.TotalSeconds)
            {
                stunLength = Math.Round(stunLength - 0.1, 2);
                oldStunTime = gametime.TotalGameTime;
            }

            //Update powerup timer
            if (powerupTime <= 0)
            {
                //Reset powerup
                currentPowerupType = PowerUpType.none;
                damageMod = 0;
                speedMod = 0;
                rapidCooldown = 0;
            }
            else if (gametime.TotalGameTime.TotalSeconds - 0.1 >= powerupOldTime.TotalSeconds)
            {
                powerupTime = Math.Round(powerupTime - 0.1, 2);
                powerupOldTime = gametime.TotalGameTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            tankBase.Draw(spriteBatch);
            tankGun.Draw(spriteBatch);

            if (currentPowerupType == PowerUpType.shield)
            {
                Rectangle source = new Rectangle(0, 0, shieldTexture.Width, shieldTexture.Height);
                spriteBatch.Draw(shieldTexture, position, source, Color.White, 0,
                    new Vector2(shieldTexture.Width / 2, shieldTexture.Height / 2), scale, SpriteEffects.None, 1);
            }
        }

        /// <summary>
        /// Fires a bullet from this tank
        /// </summary>
        public void Shoot()
        {
            if (currentCooldown == 0)
            {
                shootFX.Play();
                Vector2 bulletVelocity = new Vector2((float)Math.Sin(gunRotation), (float)Math.Cos(gunRotation));
                bulletHandler.NewBullet(new Bullet(bulletTexture, bulletExplosionTexture,
                    bulletVelocity, position, gunRotation, gunDamage + damageMod, bulletSpeed, this));

                if (currentPowerupType != PowerUpType.rapid)
                    currentCooldown = baseCooldown;
                else currentCooldown = rapidCooldown;
            }
        }

        /// <summary>
        /// Have this tank collect a powerup
        /// </summary>
        public void CollectPowerUp(Powerup powerup)
        {
            currentPowerupType = powerup.type;

            if (powerup.type == PowerUpType.speed)
            {
                speedMod = powerup.effectPower;
                powerupTime = 1.6f;
            }
            else if (powerup.type == PowerUpType.shield)
            {
                powerupTime = 6;
            }
            else if (powerup.type == PowerUpType.rapid)
            {
                powerupTime = 4;
                rapidCooldown = powerup.effectPower;

                if (currentCooldown > rapidCooldown)
                    currentCooldown = rapidCooldown;
            }
            else if (powerup.type == PowerUpType.damage)
            {
                damageMod = powerup.effectPower;
                powerupTime = 7;
            }
            else if (powerup.type == PowerUpType.rainbow)
            {
                if (this is PlayerTank)
                {
                    Game1.hasRainbowBase = true;
                    Game1.hasRainbowGun = true;
                    Game1.Save();
                }

                return;
            }

            basePowerupTime = powerupTime;
        }
    }
}
