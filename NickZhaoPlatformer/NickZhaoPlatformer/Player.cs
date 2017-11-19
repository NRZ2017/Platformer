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
    class Player : Sprite
    {
        public States CurrentState;
        public Direction CurrentDirection;
        private int frameIndex;
        public Vector2 speed;
        float velocity = 0;
        float gravity = 0.5f; //the closer jumpForce and gravity the slower the jump
        float jumpForce = -10;  //the further jumpForce and gravity the faster the jump or fall
        float ground = 0;
        bool isAir = false;
        public enum States
        {
            Idle,
            Run,
            Jump,
            Death
        }
        public enum Direction
        {
            Right,
            Left,
            Up,
            Down
        }
       public Dictionary<States, Animation> dictionary;
        public Player(Vector2 position, Texture2D image, Color tint) : base(position, image, tint)
        {
            dictionary = new Dictionary<States, Animation>();
            speed = new Vector2(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
           frameIndex++;
            if (frameIndex >= dictionary[CurrentState].frames.Count)
            {
                frameIndex = 0;
            }

            SourceRectangle = dictionary[CurrentState].frames[frameIndex];


            if (!isAir && ks.IsKeyDown(Keys.Up))
            {
                velocity += jumpForce;
                isAir = true;
            }

            if (isAir)
            {
                Position.Y += velocity;
                velocity += gravity;
            }

            if (Position.Y > ground)
            {
                isAir = false;
                velocity = 0;
                Position.Y = ground;
            }

            base.Update(gameTime);

        }

    }
}
