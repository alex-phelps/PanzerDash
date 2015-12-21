using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_Tank_Racer_Game
{
    /// <summary>
    /// Represents some object in the game
    /// </summary>
    public class GameObject
    {
        public Texture2D texture { get; private set; }
        public Vector2 position;
        public float rotation = 0;
        public float scale = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Color data for the pixels of this objects image
        /// </summary>
        public Color[] colorData;

        /// <summary>
        /// Matrix that represents all the transformations done on the object
        /// </summary>
        public Matrix transformMatrix
        {
            get
            {
                return
                Matrix.CreateTranslation(new Vector3(-Width / 2, -Height / 2, 0.0f)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
        }

        /// <summary>
        /// Rectangle that represents bounds of the object
        /// </summary>
        public Rectangle boundingRectangle
        {
            get
            {
                Rectangle rectangle = new Rectangle(0, 0, Width, Height);
                Matrix transform = transformMatrix;

                //Get all four corners in local space
                Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
                Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
                Vector2 botLeft = new Vector2(rectangle.Left, rectangle.Bottom);
                Vector2 botRight = new Vector2(rectangle.Right, rectangle.Bottom);

                //Transform corners into work space
                Vector2.Transform(ref topLeft, ref transform, out topLeft);
                Vector2.Transform(ref topRight, ref transform, out topRight);
                Vector2.Transform(ref botLeft, ref transform, out botLeft);
                Vector2.Transform(ref botRight, ref transform, out botRight);

                //Find minimum and maximum extents of the rectangle in world space
                Vector2 min = Vector2.Min(Vector2.Min(topLeft, topRight),
                    Vector2.Min(botLeft, botRight));
                Vector2 max = Vector2.Max(Vector2.Max(topLeft, topRight),
                    Vector2.Max(botLeft, botRight));

                return new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }


        public GameObject(Texture2D texture)
        {
            this.texture = texture;

            Width = texture.Width;
            Height = texture.Height;
            position = new Vector2(0, 0);

            colorData = new Color[Width * Height];
            texture.GetData(colorData);
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(0, 0, Width, Height);
            spriteBatch.Draw(texture, position, source, Color.White, rotation,
                new Vector2(Width / 2, Height / 2), scale, SpriteEffects.None, 1);
        }
    }
}
