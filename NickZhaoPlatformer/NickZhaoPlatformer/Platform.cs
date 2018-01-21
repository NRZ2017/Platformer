using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NickZhaoPlatformer
{
    class Platform : Sprite
    {

        public bool Collidable;

        //overrite the hitbox to add a offset

        public virtual float Top
        {
            get
            {
                return Position.Y - Origin.Y * Scale + offset;
            }
        }

        protected int offset = 20;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(Position.X - Origin.X * Scale), (int)(Position.Y - Origin.Y * Scale) + offset, (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale) - offset);
            }
        }

        public Platform(Vector2 position, Texture2D image, Color tint, float scale, bool collidable, bool visable) 
            : base(position, image, tint)
        {
            Scale = scale;
            SourceRectangle = new Rectangle(41, 38, 132, 69);
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            Collidable = collidable;
            Visible = visable;
        }


    }
}
