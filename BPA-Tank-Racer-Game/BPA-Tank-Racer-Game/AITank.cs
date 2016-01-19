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
            if (!isStunned)
            {
                speed += accel;

                velocity.X = (float)Math.Sin(rotation) * speed;
                velocity.Y = (float)Math.Cos(rotation) * -speed;
            }
            
            base.Update(gametime);

            if (!isStunned)
                position += velocity;
        }

        public bool SteerAi(Background background)
        {
            if (!isStunned)
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

            return false;
        }

        public bool SteerAi(Tank enemy)
        {
            if (!isStunned)
            {
                Matrix transformTankToLevel = transformMatrix * Matrix.Invert(enemy.transformMatrix);

                //Create points around this tank to check the position of the enemy in relation to us

                //3 points in front of this tank
                Vector2 positionInEnemyL = Vector2.Transform(new Vector2(-8, -Height / 2), transformTankToLevel);
                Vector2 positionInEmenyM = Vector2.Transform(new Vector2(Width / 2, -Height), transformTankToLevel);
                Vector2 positionInEnemyR = Vector2.Transform(new Vector2(8 + Width, -Height / 2), transformTankToLevel);
                //2 points on either side of this tank
                Vector2 positionInEnemySideL = Vector2.Transform(new Vector2(-10, Height / 2), transformTankToLevel);
                Vector2 positionInEnemySideR = Vector2.Transform(new Vector2(10 + Width, Height / 2), transformTankToLevel);

                int xL = (int)Math.Round(positionInEnemyL.X);
                int yL = (int)Math.Round(positionInEnemyL.Y);
                int xM = (int)Math.Round(positionInEmenyM.X);
                int yM = (int)Math.Round(positionInEmenyM.Y);
                int xR = (int)Math.Round(positionInEnemyR.X);
                int yR = (int)Math.Round(positionInEnemyR.Y);
                int xSL = (int)Math.Round(positionInEnemySideL.X);
                int ySL = (int)Math.Round(positionInEnemySideL.Y);
                int xSR = (int)Math.Round(positionInEnemySideR.X);
                int ySR = (int)Math.Round(positionInEnemySideR.Y);

                bool LInObj = false;
                bool MInObj = false;
                bool RInObj = false;
                bool SLInObj = false;
                bool SRInObj = false;

                //See if the enemy is in any of those points
                if (xL >= 0 && xL <= enemy.Width && yL >= 0 && yL <= enemy.Height)
                    LInObj = true;
                if (xM >= 0 && xM <= enemy.Width && yM >= 0 && yM <= enemy.Height)
                    MInObj = true;
                if (xR >= 0 && xR <= enemy.Width && yR >= 0 && yR <= enemy.Height)
                    RInObj = true;
                if (xSL >= 0 && xSL <= enemy.Width && ySL >= 0 && ySL <= enemy.Height)
                    SLInObj = true;
                if (xSR >= 0 && xSR <= enemy.Width && ySR >= 0 && ySR <= enemy.Height)
                    SRInObj = true;


                //Dodge based on where the enemy has been found
                if ((!LInObj && RInObj) || SRInObj)
                {
                    rotSpeed -= rotAccel;
                    return true;
                }
                else if (LInObj && !RInObj || SLInObj)
                {
                    rotSpeed += rotAccel;
                    return true;
                }
                else if ((!LInObj && !RInObj && MInObj) || (LInObj && RInObj))
                {
                    rotSpeed -= rotAccel;
                }
            }

            return false;
        }

        public void ShootAi(Tank targetTank)
        {
            if (!isStunned)
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
}
