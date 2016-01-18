using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BPA_Tank_Racer_Game
{
    public class PlayerTank : Tank
    {
        private KeyboardState oldState = Keyboard.GetState();

        public PlayerTank(ContentManager content, BulletHandler bulletHandler, TankPartType baseType, TankPartType gunType) 
            : base(content, bulletHandler, baseType, gunType)
        {
            position = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);
        }

        public override void Update(GameTime gametime)
        {

            KeyboardState newState = Keyboard.GetState();

            //Player input from keyboard
            if (newState.IsKeyDown(Keys.W))
            {
                speed += accel;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                speed -= accel;
            }
            if (newState.IsKeyDown(Keys.A))
            {
                rotSpeed -= rotAccel;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                rotSpeed += rotAccel;
            }
            if (newState.IsKeyDown(Keys.Left))
            {
                gunRotation -= 0.04f;
            }
            if (newState.IsKeyDown(Keys.Right))
            {
                gunRotation += 0.04f;
            }
            if (newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
            {
                if (speed < -0.25f) // Not 0 here to fix any rounding errors
                    speed += 0.25f;
                else if (speed > 0.25f) //Not 0 here to fix any rounding errors
                    speed -= 0.25f;
                else speed = 0;
            }
            if (newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.D))
            {
                if (rotSpeed < -0.05f) // Not 0 here to fix any rounding errors
                    rotSpeed += 0.03f;
                else if (rotSpeed > 0.05f) //Not 0 here to fix any rounding errors
                    rotSpeed -= 0.03f;
                else rotSpeed = 0;
            }

            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                Shoot();
            }

            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;

            oldState = newState;

            base.Update(gametime);
        }
    }
}
