﻿using System;
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


        //overrite the hitbox to add a offset

        public float Top
        {
            get
            {
                return Position.Y - Origin.Y * Scale + offset;
            }
        }

        private int offset = 20;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(Position.X - Origin.X * Scale), (int)(Position.Y - Origin.Y * Scale) + offset, (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale) - offset);
            }
        }

        public Platforms(Vector2 position, Texture2D image, Color tint) : base(position, image, tint)
        {
            Scale = 2;
            SourceRectangle = new Rectangle(41, 38, 132, 69);
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
        }


    }
}
