using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using System.Linq;
using System;
using System.Collections.Generic;

namespace NickZhaoPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Animation run;
        Animation jump;
        Animation idle;
        Animation death;
        Texture2D Background;
        bool IsDead = false;
        SpriteFont font;
        Button button;
        Random gen;
 

        List<SolidPlatform> solidPlatforms = new List<SolidPlatform>();
        List<Platform> platforms = new List<Platform>();
        List<Spikes> spikes = new List<Spikes>();
        Texture2D singlePixel;
        Sprite Door1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

        }

        /// Sprite : basic
        /// Animation : list of rectangles
        /// Player : Sprite, enum of states, dictionary(state, animation)
        /// Plateform : Sprite
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(new Vector2(100, 1020), Content.Load<Texture2D>("sheet"), Color.White, 
                GraphicsDevice.Viewport.Height);
            run = new Animation();
            jump = new Animation();
            idle = new Animation();
            death = new Animation();
            platforms = new List<Platform>();
            solidPlatforms = new List<SolidPlatform>();
            spikes = new List<Spikes>();
            player.dictionary.Add(Player.States.Run, run);
            player.dictionary.Add(Player.States.Jump, jump);
            player.dictionary.Add(Player.States.Idle, idle);
            player.dictionary.Add(Player.States.Death, death);
            button = new Button(new Vector2(969, 677), Content.Load<Texture2D>("RetryButton"), Color.White, 1f);
            Door1 = new Sprite(new Vector2(1875, 181), Content.Load<Texture2D>("Doorz"), Color.White, 1f);
            // spikes.Add(new Spikes(new Vector2(600, 700), Content.Load<Texture2D>("Spikes_in_Sonic_the_Hedgehog_4"), Color.White, 0.5f));

            Levels.Level1(platforms, spikes, solidPlatforms, Content.Load<Texture2D>("MyPlatforms"), 
                Content.Load<Texture2D>("Spikes_in_Sonic_the_Hedgehog_4"), Content.Load<Texture2D>("BackTrump"));


            base.Initialize();


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            XDocument doc = XDocument.Load("output.xml");
            foreach (XElement element in doc.Root.Descendants())
            {
                int x = (int.Parse)(element.Attribute("x").Value);
                int y = (int.Parse)(element.Attribute("y").Value);
                int h = (int.Parse)(element.Attribute("h").Value);
                int w = (int.Parse)(element.Attribute("w").Value);
                if (element.Attribute("n").Value.Contains("Running"))
                {
                    run.frames.Add(new Rectangle(x, y, w, h));
                }

                else if (element.Attribute("n").Value.Contains("Jump"))
                {
                    jump.frames.Add(new Rectangle(x, y, w, h));
                }
                else if (element.Attribute("n").Value.Contains("Idle"))
                {
                    idle.frames.Add(new Rectangle(x, y, w, h));
                }
                else if (element.Attribute("n").Value.Contains("Death"))
                {
                    death.frames.Add(new Rectangle(x, y, w, h));
                }

            }

            singlePixel = new Texture2D(GraphicsDevice, 1, 1);
            singlePixel.SetData(new Color[] { Color.White });
            Background = Content.Load<Texture2D>("BackTrump");
            font = Content.Load<SpriteFont>("Font1");
            gen = new Random();
            // player.dictionary.Add(Player.States.Run, run.frames);
            //Factory Function
            //loop through doc.Root's children and load the sprites into the correct animation

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                if (button.Pressed())
                {
                    player.Position = new Vector2(100, 1020);
                    IsDead = false;
                }
                return;
            }
            MouseState ms = Mouse.GetState();


            KeyboardState ks = Keyboard.GetState();

            Window.Title = $"X: {ms.X}, Y: {ms.Y}";

            if (ms.LeftButton == ButtonState.Pressed)
            {
                player.Position = new Vector2(ms.X, ms.Y);
                player.velocity.Y = 0;
            }

            if (ks.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            player.Update(gameTime);
            bool onPlatform = false;
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].Collidable && player.Hitbox.Intersects(platforms[i].Hitbox))
                {
                    onPlatform = true;
                    Vector2 diff = platforms[i].Position - player.Position;
                    float angle = MathHelper.ToDegrees((float)Math.Atan2(diff.Y, diff.X));



                    if (angle < -15 && angle > -180 + 15) //also we are moving UP
                    {

                    }
                    else if (angle > 15 && angle < 180 - 15 && player.velocity.Y > 0) //also we are moving DOWN
                    {
                        //set the top of the platform to our new ground
                        player.velocity.Y = 0;
                        player.CurrentState = Player.States.Idle;
                        player.Position.Y = platforms[i].Top;
                        player.ground = platforms[i].Top;
                    }
                    else if (
                        player.velocity.Y != 0 && //shrink or grow angle to remove this line
                        Math.Abs(player.Position.X - platforms[i].Position.X) > (platforms[i].Hitbox.Width / 2) &&
                        ((player.CurrentDirection == Player.Direction.Left && player.Position.X > platforms[i].Position.X) ||
                        (player.CurrentDirection == Player.Direction.Right && player.Position.X < platforms[i].Position.X))
                        )
                    {
                        player.velocity.X = 0;
                    }
                }

            }

            for (int q = 0; q < solidPlatforms.Count; q++)
            {
                if (player.Hitbox.Intersects(solidPlatforms[q].Hitbox))
                {
                    onPlatform = true;
                    Vector2 diff = solidPlatforms[q].Position - player.Position;
                    float angle = MathHelper.ToDegrees((float)Math.Atan2(diff.Y, diff.X));
                    if (angle < -15 && angle > -180 + 15 && player.velocity.Y < 0) //also we are moving UP
                    {
                        player.velocity.Y = 0;
                    }
                    if (angle > 15 && angle < 180 - 15 && player.velocity.Y > 0)
                    {
                        player.velocity.Y = 0;
                        player.CurrentState = Player.States.Idle;
                        player.Position.Y = solidPlatforms[q].Top;
                        player.ground = solidPlatforms[q].Top;
                    }
                    else if (player.velocity.Y != 0 && //shrink or grow angle to remove this line
                        Math.Abs(player.Position.X - solidPlatforms[q].Position.X) > (solidPlatforms[q].Hitbox.Width / 2) &&
                        ((player.CurrentDirection == Player.Direction.Left && player.Position.X > solidPlatforms[q].Position.X) ||
                        (player.CurrentDirection == Player.Direction.Right && player.Position.X < solidPlatforms[q].Position.X))
                        )
                    {
                        player.velocity.X = 0;

                    }

                }

            }

            if (!onPlatform)
            {
                player.ground = GraphicsDevice.Viewport.Height - 20; //only run this line if you don't intersect with ANY platform
            }

            for (int i = 0; i < spikes.Count; i++)
            {
                if (player.Hitbox.Intersects(spikes[i].Hitbox) && spikes[i].Collidable)
                {
                    IsDead = true;
                }
                else
                {

                }
            }

            if (Door1.Hitbox.Intersects(player.Hitbox))
            {
                platforms = new List<Platform>();
                solidPlatforms = new List<SolidPlatform>();
                spikes = new List<Spikes>();
                Levels.Level2(platforms, spikes, solidPlatforms, Content.Load<Texture2D>("MyPlatforms"),
                    Content.Load<Texture2D>("Spikes_in_Sonic_the_Hedgehog_4"), Content.Load<Texture2D>("BackTrump"), gen, GraphicsDevice);
            }
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].Hitbox.Intersects(player.Hitbox))
                    {
                    platforms[i].Visible = true;
                }
            }
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(Background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            player.Draw(spriteBatch);
            if (player.CurrentDirection == Player.Direction.Left)
            {
                player.Effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                player.Effect = SpriteEffects.None;
            }
            for (int j = 0; j < platforms.Count; j++)
            {
                if (platforms[j].Visible)
                {
                    platforms[j].Draw(spriteBatch);
                }

            }
            for (int z = 0; z < solidPlatforms.Count; z++)
            {
                if (solidPlatforms[z].Visible)
                {
                    solidPlatforms[z].Draw(spriteBatch);
                }
            }


            for (int i = 0; i < spikes.Count; i++)
            {
                if (spikes[i].Visible)
                {
                    spikes[i].Draw(spriteBatch);
                }
            }

            if (IsDead)
            {
                spriteBatch.DrawString(font, "Game Over. Try Again?", new Vector2(700, 340), Color.Black);
                button.Draw(spriteBatch);
            }

            Door1.Draw(spriteBatch);
            //for (int i = 0; i < solidPlatforms.Count; i++)
            //{

            //6  spriteBatch.Draw(singlePixel, button.Hitbox, Color.Blue * 0.40f);
            //}
           //  spriteBatch.Draw(singlePixel, player.Hitbox, Color.Red * 0.6f);

            //    for (int i = 0; i < spikes.Count; i++)
            //   {
            //      spriteBatch.Draw(singlePixel, spikes[i].Hitbox, Color.Red * 0.40f);
            //    }


             // spriteBatch.Draw(singlePixel, Door1.Hitbox, Color.Blue);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
