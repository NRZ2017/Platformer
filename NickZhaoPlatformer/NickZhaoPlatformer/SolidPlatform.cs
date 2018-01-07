using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NickZhaoPlatformer
{
    class SolidPlatform : Platform
    {
        public override float Top
        {
            get
            {
                return Hitbox.Y ;
            }
        }

        public override Rectangle Hitbox
        {
            get
            {
                int Ox = base.Hitbox.Width / 2;
                int Oy = base.Hitbox.Height / 2;

                int Cx = base.Hitbox.X + base.Hitbox.Width / 2;
                int Cy = base.Hitbox.Y + base.Hitbox.Height / 2;

                int Rx = Cx - Oy;
                int Ry = Cy + Ox;

                return new Rectangle(Rx, Ry - base.Hitbox.Width + 20, base.Hitbox.Height, base.Hitbox.Width);
            }
        }

        public SolidPlatform(Vector2 position, Texture2D image, Color tint, float scale) : base(position, image, tint, scale)
        {
            SourceRectangle = new Rectangle(401, 272, 140, 56);
            offset = 0;
            Rotation = (float)Math.PI /2;
        }
    }
}
