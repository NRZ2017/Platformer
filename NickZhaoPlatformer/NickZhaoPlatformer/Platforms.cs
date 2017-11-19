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
        public Platforms(Vector2 position, Texture2D image, Color tint) : base(position, image, tint)
        {
        }
    }
}
