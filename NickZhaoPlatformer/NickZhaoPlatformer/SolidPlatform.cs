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
        public SolidPlatform(Vector2 position, Texture2D image, Color tint, float scale) : base(position, image, tint, scale)
        {
            SourceRectangle = new Rectangle(401, 272, 140, 56);
            offset = 0;
        }
    }
}
