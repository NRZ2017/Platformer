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

        public Vector2 velocity;

        public float gravity = 0.2f; //the closer jumpForce and gravity the slower the jump
        public float jumpForce = -10;  //the further jumpForce and gravity the faster the jump or fall
        public float ground;
        bool isAir = false;
        float screenHeight;
        TimeSpan frameRate = TimeSpan.FromMilliseconds(1000 / 30);
        TimeSpan animTimer = TimeSpan.Zero;
        int JumpCount = 0;

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


        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(Position.X - Origin.X * Scale), (int)(Position.Y - Origin.Y * Scale) + 2, (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale));
            }
        }

        public Dictionary<States, Animation> dictionary;
        public Player(Vector2 position, Texture2D image, Color tint, float screenHeight) 
            : base(position, image, tint)
        {
            dictionary = new Dictionary<States, Animation>();
            velocity = new Vector2(0, 0);
            //ground = screenHeight - 500;
            this.screenHeight = screenHeight;
            Scale = 0.5f;

            ground = screenHeight - 70;

            //origin
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height);

        }

        bool runOnce = true;
        KeyboardState LastState = Keyboard.GetState();
        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height);

            KeyboardState ks = Keyboard.GetState();
            Position += velocity;

            if (ks.IsKeyDown(Keys.Right))
            {
                CurrentState = Player.States.Run;
                CurrentDirection = Player.Direction.Right;
                velocity.X = 5;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                CurrentState = Player.States.Death;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                CurrentState = Player.States.Run;

                velocity.X = -5;
                CurrentDirection = Player.Direction.Left;
            }
            else
            {
                CurrentState = Player.States.Idle;
                velocity.X = 0;
            }

            if (!isAir && ks.IsKeyDown(Keys.Up))
            {
                velocity.Y += jumpForce;
                Position += velocity;
                isAir = true;
            }
            else if (isAir && ks.IsKeyDown(Keys.Up) && !LastState.IsKeyDown(Keys.Up) && JumpCount == 0)
            {
                velocity.Y = jumpForce;
                Position += velocity;
                isAir = true;
                JumpCount++;
            }
            
            if (Position.Y < ground)
            {
                isAir = true;
            }
            else
            {
                isAir = false;
                velocity.Y = 0;
                Position.Y = ground;
                JumpCount = 0;
            }

            if (isAir)
            {
                CurrentState = Player.States.Jump;
                velocity.Y += gravity;
            }


            LastState = ks;
            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
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
