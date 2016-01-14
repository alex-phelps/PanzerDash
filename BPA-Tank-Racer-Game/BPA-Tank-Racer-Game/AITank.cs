using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public bool SteerAi(GameObject gameObject)
        {
            Matrix transformTankToLevel = transformMatrix * Matrix.Invert(gameObject.transformMatrix);

            Vector2 positionInObjectL = Vector2.Transform(new Vector2(-20, -Height * 2), transformTankToLevel);
            Vector2 positionInObjectR = Vector2.Transform(new Vector2(20, -Height * 2), transformTankToLevel);

            int xL = (int)Math.Round(positionInObjectL.X);
            int yL = (int)Math.Round(positionInObjectL.Y);
            int xR = (int)Math.Round(positionInObjectR.X);
            int yR = (int)Math.Round(positionInObjectR.Y);

            bool lInObj = false;
            bool rInObj = false;

            //If point R is in gameObject's rectangle
            if (((xL >= gameObject.boundingRectangle.X) &&
                (xL <= gameObject.boundingRectangle.X + gameObject.boundingRectangle.Width)) &&
                ((yL >= gameObject.boundingRectangle.Y) &&
                (yL <= gameObject.boundingRectangle.Y + gameObject.boundingRectangle.Height)))

                lInObj = true;

            //If point L is in gameObject's rectangle
            if (((xR >= gameObject.boundingRectangle.X) &&
                (xR <= gameObject.boundingRectangle.X + gameObject.boundingRectangle.Width)) &&
                ((yR >= gameObject.boundingRectangle.Y) &&
                (yR <= gameObject.boundingRectangle.Y + gameObject.boundingRectangle.Height)))

                rInObj = true;

            //Steer around object
            if ((lInObj && rInObj) || (!lInObj && rInObj))
            {
                rotSpeed -= rotAccel;
                return true;
            }
            else if (lInObj && !rInObj)
            {
                rotSpeed += rotAccel;
                return true;
            }
            else return false;
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
    }
}
