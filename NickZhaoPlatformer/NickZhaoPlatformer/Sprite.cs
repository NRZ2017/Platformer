using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickZhaoPlatformer
{
    class Sprite
    {
        public Vector2 Position;
        public Texture2D Image;
        public Color Tint;
        public Rectangle SourceRectangle;
        public float Rotation;
        public Vector2 Origin;
        public float Scale;
        public SpriteEffects Effect;
        public float LayerDepth;

        public virtual Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale));
            }
        }

        /// <summary>
        /// Default Constructor with default Values
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>
        public Sprite(Vector2 position, Texture2D image, Color tint)
        {
            this.Position = position;
            this.Image = image;
            this.Tint = tint;
            SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            Rotation = 0;
            Origin = Vector2.Zero;
            Scale = 1;
            Effect = SpriteEffects.None;
            LayerDepth = 0;
        }

        /// <summary>
        /// Origin Center
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>
        /// <param name="Scale"></param>
        public Sprite(Vector2 position, Texture2D image, Color tint, float scale)
            : this(position, image, tint)
        {
            Scale = scale;
            Origin = new Vector2(image.Width / 2, image.Height / 2);
        }

        public Sprite(Vector2 position, Texture2D image, Color tint, float scale, float rotation)
            : this(position, image, tint)
        {
            Scale = scale;
            Origin = new Vector2(image.Width / 2, image.Height / 2);
            Rotation = rotation;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Image, Position, SourceRectangle, Tint, Rotation, Origin, Scale, Effect, LayerDepth);
        }
    }
}
