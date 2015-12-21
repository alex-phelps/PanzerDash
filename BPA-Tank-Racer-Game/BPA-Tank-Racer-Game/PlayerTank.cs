using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    public class PlayerTank : Tank
    {
        private KeyboardState oldState = Keyboard.GetState();

        public PlayerTank(ContentManager content, TankBaseType baseType, TankGunType gunType) 
            : base(content, baseType, gunType)
        {
            position = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);
            accel = 0.1f;
            maxSpeed = 2;
        }

        public override void Update()
        {

            KeyboardState newState = Keyboard.GetState();

            //Player input from keyboard
            if (newState.IsKeyDown(Keys.W))
            {
                speed += accel;

                if (speed > maxSpeed)
                    speed = maxSpeed;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                speed -= accel;

                if (speed < -maxSpeed)
                    speed = -maxSpeed;
            }
            if (newState.IsKeyDown(Keys.A))
            {
                rotation -= 0.05f;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                rotation += 0.05f;
            }
            if (newState.IsKeyDown(Keys.Left))
            {
                gunRotation -= 0.03f;
            }
            if (newState.IsKeyDown(Keys.Right))
            {
                gunRotation += 0.03f;
            }
            if (newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
            {
                if (speed < -0.02f) // Not 0 here to fix any rounding errors
                    speed += 2 * 0.12f;
                else if (speed > 0.02f) //Not 0 here to fix any rounding errors
                    speed -= 2 * 0.12f;
                else speed = 0;
            }

            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                Shoot();
            }

            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;

            oldState = newState;
            base.Update();
        }
    }
}
