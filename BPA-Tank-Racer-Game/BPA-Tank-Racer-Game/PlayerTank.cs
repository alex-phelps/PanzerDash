using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PanzerDash
{
    /// <summary>
    /// Represents a player controlled tank
    /// </summary>
    public class PlayerTank : Tank
    {
        private KeyboardState oldState = Keyboard.GetState();

        private Keys forward, backward, turnLeft, turnRight, rotateLeft, rotateRight, fire;

        public PlayerTank(ContentManager content, BulletHandler bulletHandler, TankPartType baseType, TankPartType gunType,
            Keys forward = Keys.W, Keys backward = Keys.S, Keys turnLeft = Keys.A, Keys turnRight = Keys.D,
            Keys rotateLeft = Keys.Left, Keys rotateRight = Keys.Right, Keys fire = Keys.Space)
            : base(content, bulletHandler, baseType, gunType)
        {
            position = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);

            this.forward = forward;
            this.backward = backward;
            this.turnLeft = turnLeft;
            this.turnRight = turnRight;
            this.rotateLeft = rotateLeft;
            this.rotateRight = rotateRight;
            this.fire = fire;
        }

        public override void Update(GameTime gametime)
        {
            if (!isStunned)
            {
                KeyboardState newState = Keyboard.GetState();

                //Player input from keyboard
                if (newState.IsKeyDown(forward))
                {
                    speed += accel;
                }
                if (newState.IsKeyDown(backward))
                {
                    speed -= accel;
                }
                if (newState.IsKeyDown(turnLeft))
                {
                    rotSpeed -= rotAccel;
                }
                if (newState.IsKeyDown(turnLeft))
                {
                    rotSpeed += rotAccel;
                }
                if (newState.IsKeyDown(rotateLeft))
                {
                    gunRotation -= 0.04f;
                }
                if (newState.IsKeyDown(rotateRight))
                {
                    gunRotation += 0.04f;
                }
                if (newState.IsKeyUp(forward) && newState.IsKeyUp(backward))
                {
                    if (speed < -0.25f) // Not 0 here to fix any rounding errors
                        speed += 0.25f;
                    else if (speed > 0.25f) //Not 0 here to fix any rounding errors
                        speed -= 0.25f;
                    else speed = 0;
                }
                if (newState.IsKeyUp(turnLeft) && newState.IsKeyUp(turnRight))
                {
                    if (rotSpeed < -0.05f) // Not 0 here to fix any rounding errors
                        rotSpeed += 0.03f;
                    else if (rotSpeed > 0.05f) //Not 0 here to fix any rounding errors
                        rotSpeed -= 0.03f;
                    else rotSpeed = 0;
                }

                if (newState.IsKeyDown(fire) && oldState.IsKeyUp(fire))
                {
                    Shoot();
                }

                velocity.X = (float)Math.Sin(rotation) * speed;
                velocity.Y = (float)Math.Cos(rotation) * -speed;

                oldState = newState;
            }

            base.Update(gametime);
        }
    }
}
