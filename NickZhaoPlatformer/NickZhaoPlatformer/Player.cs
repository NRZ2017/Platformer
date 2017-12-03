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
        public float velocity = 0;
        public float gravity = 0.2f; //the closer jumpForce and gravity the slower the jump
        public float jumpForce = -15;  //the further jumpForce and gravity the faster the jump or fall
        public float ground;
        bool isAir = false;
        float screenHeight;
        TimeSpan frameRate = TimeSpan.FromMilliseconds(1000 / 30);
        TimeSpan animTimer = TimeSpan.Zero;


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
        public Player(Vector2 position, Texture2D image, Color tint, float screenHeight) : base(position, image, tint)
        {
            dictionary = new Dictionary<States, Animation>();
            speed = new Vector2(0, 0);
            //ground = screenHeight - 500;
            this.screenHeight = screenHeight;
            Scale = 0.5f;

            ground = screenHeight - 70;

        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();


            if (!isAir && ks.IsKeyDown(Keys.Up))
            {
                velocity += jumpForce;
                isAir = true;
            }

            if (Position.Y < ground)
            {
                isAir = true;
            }

            if (isAir)
            {
                CurrentState = Player.States.Jump;
                Position.Y += velocity;
                velocity += gravity;
            }

            if (Position.Y > ground)
            {
                isAir = false;
                velocity = 0;
                Position.Y = ground;
            }

            Animate(gameTime);

            base.Update(gameTime);

        }

        public void Animate(GameTime gameTime)
        {
            animTimer += gameTime.ElapsedGameTime;
            if (animTimer > frameRate)
            {
                animTimer = TimeSpan.Zero;

                frameIndex++;

                if (frameIndex >= dictionary[CurrentState].frames.Count)
                {
                    if (CurrentState == States.Jump)
                    {
                        frameIndex = dictionary[CurrentState].frames.Count - 3;
                    }
                    else
                    {
                        frameIndex = 0;
                    }
                }
                SourceRectangle = dictionary[CurrentState].frames[frameIndex];
            }
        }

    }
}
