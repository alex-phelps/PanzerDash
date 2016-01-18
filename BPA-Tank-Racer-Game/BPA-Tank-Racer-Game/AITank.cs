using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class AITank : Tank
    {
        public AITank(ContentManager content, BulletHandler bulletHandler, TankPartType baseType,
            TankPartType gunType, Vector2 startPos)
            : base(content, bulletHandler, baseType, gunType)
        {
            position = startPos;
        }

        public override void Update(GameTime gametime)
        {
            speed += accel;

            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;
            
            base.Update(gametime);

            position += velocity ;
        }

        public bool SteerAi(Background background)
        {
            Matrix transformTankToLevel = transformMatrix * Matrix.Invert(background.transformMatrix);

            Vector2 positionInLevelL = Vector2.Transform(new Vector2(-50, -Height * 4), transformTankToLevel);
            Vector2 positionInLevelR = Vector2.Transform(new Vector2(50, -Height * 4), transformTankToLevel);

            int xL = (int)Math.Round(positionInLevelL.X);
            int yL = (int)Math.Round(positionInLevelL.Y);
            int xR = (int)Math.Round(positionInLevelR.X);
            int yR = (int)Math.Round(positionInLevelR.Y);

            Color colorRight = background.colorData[xL + yL * background.Width];
            Color colorLeft = background.colorData[xR + yR * background.Width];

            //Define collision colors
            List<Color> colors = new List<Color>();
            colors.Add(new Color(0, 0, 0)); //Black
            colors.Add(new Color(61, 32, 4)); //Darkest Brown
            colors.Add(new Color(87, 46, 6)); //Darker Brown
            colors.Add(new Color(95, 54, 14)); //Medium Brown
            colors.Add(new Color(112, 70, 28)); //Light Brown
            colors.Add(new Color(126, 76, 28)); //Lightest Brown

            //Steer away from border
            foreach (Color color in colors)
            {
                if ((colorRight == color && colorLeft == color) ||
                (colorRight != color && colorLeft == color))
                {
                    rotSpeed -= rotAccel;
                    return true;
                }
                else if (colorRight == color && colorLeft != color)
                {
                    rotSpeed += rotAccel;
                    return true;
                }
            }

            return false;
        }

        public void ShootAi(Tank targetTank)
        {
            //Aim

            //Find sub pi/2 angle
            double angleToTarget = Math.Atan((position.Y - targetTank.position.Y) / (targetTank.position.X - position.X));
            angleToTarget = Math.PI / 2 - angleToTarget;

            //Find quadrent target is in and adjust angle acordingly
            if (targetTank.position.X >= position.X)
            {
                angleToTarget += Math.PI;
            }

            //Find angle from gun to target, prev was from strait up to target
            angleToTarget -= gunRotation;

            angleToTarget %= Math.PI * 2;
            
            //Adjust angle to be the nearest one in either direction
            if (angleToTarget > Math.PI)
                angleToTarget = -(Math.PI * 2 - angleToTarget);
            else if (angleToTarget < -Math.PI)
                angleToTarget = (Math.PI * 2 + angleToTarget);

            if (angleToTarget > 0)
                gunRotation -= 0.04f;
            else gunRotation += 0.04f;
            

            //Fire
            if (angleToTarget < 0.2f || angleToTarget > -0.2f)
                Shoot();
        }
    }
}
