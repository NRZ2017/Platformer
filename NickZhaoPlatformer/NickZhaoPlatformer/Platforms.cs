using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NickZhaoPlatformer
{
    class Platforms : Sprite
    {
        public Rectangle Hitbox
            {
            get { return new Rectangle((int)Position.X, (int)Position.Y, SourceRectangle.Width, SourceRectangle.Height); }
            
            }
        public Rectangle platform;
        public Platforms(Vector2 position, Texture2D image, Color tint) : base(position, image, tint)
        {
            platform = new Rectangle(42, 38, 131, 64);
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Image, Position, platform, Tint, Rotation, Origin, Scale, Effect, LayerDepth);
        }

    }
}
