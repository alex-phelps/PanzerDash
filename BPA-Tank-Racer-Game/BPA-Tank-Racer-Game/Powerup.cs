using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public enum PowerUpType
    {
        speed, 
        shield,
        rapid,
        damage,
        rainbow,
        none
    }

    public class Powerup : GameObject
    {
        public float effectPower { get; private set; }
        public PowerUpType type { get; private set; }

        public Powerup(ContentManager content, Vector2 position, PowerUpType type)
            : base(content.Load<Texture2D>("SpeedPowerUp"))
        {
            this.type = type;
            this.position = position;

            if (type == PowerUpType.speed)
            {
                //Already have speed texture
                effectPower = 2f; //Modifies maxSpeed;
            }
            else if (type == PowerUpType.shield)
            {
                texture = content.Load<Texture2D>("ShieldPowerUp");
                effectPower = 0; //Won't affect any values
            }
            else if (type == PowerUpType.rapid)
            {
                texture = content.Load<Texture2D>("RapidFirePowerUp");
                effectPower = 0.4f; //Sets tank's shot cooldown to this
            }
            else if (type == PowerUpType.rainbow)
            {
                texture = content.Load<Texture2D>("RainbowPowerup");
            }
            else // damage
            {
                texture = content.Load<Texture2D>("DamageIncreasePowerUp");
                effectPower = 2; //Adds this to tank's damage stat
            }

        }
    }
}
