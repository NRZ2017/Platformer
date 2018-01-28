using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NickZhaoPlatformer
{
    class Button : Sprite
    {
        public Button(Vector2 position, Texture2D image, Color tint, float scale) : base(position, image, tint, scale)
        {
           
        }
        public bool Pressed()
        {
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && Hitbox.Intersects(new Rectangle(ms.Position,new Point(1))))
            {
                return true;
            }
            return false;
        }
    }
}
