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
        
        TankBase tankBase;
        TankGun tankGun;

        public Vector2 position;
        public Vector2 velocity;
        public float speed;
        public float accel;
        public float maxSpeed;
        public float rotation;
        public float gunRotation;

        public Tank(ContentManager content, TankBaseType baseType, TankGunType gunType)
        {
            this.baseType = baseType;
            this.gunType = gunType;

            //Assign base
            if (baseType == TankBaseType.basic)
            {
                tankBase = new TankBase(content.Load<Texture2D>("basicTankBase"));
            }

            //Assign gun
            if (gunType == TankGunType.basic)
            {
                tankGun = new TankGun(content.Load<Texture2D>("basicTankGun"));
            }
        }

        public virtual void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tankBase.Draw(spriteBatch);
            tankGun.Draw(spriteBatch);
        }
    }
}
