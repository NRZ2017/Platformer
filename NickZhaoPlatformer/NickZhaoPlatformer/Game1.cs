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

        List<SolidPlatform> solidPlatforms = new List<SolidPlatform>();
        List<Platform> platforms = new List<Platform>();

        Texture2D singlePixel;

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
            player = new Player(new Vector2(100, 1020), Content.Load<Texture2D>("sheet"), Color.White, GraphicsDevice.Viewport.Height);
            run = new Animation();
            jump = new Animation();
            idle = new Animation();
            death = new Animation();
            player.dictionary.Add(Player.States.Run, run);
            player.dictionary.Add(Player.States.Jump, jump);
            player.dictionary.Add(Player.States.Idle, idle);
            player.dictionary.Add(Player.States.Death, death);

            platforms.Add(new Platform(new Vector2(276, 912), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
            platforms.Add(new Platform(new Vector2(626, 697), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
            platforms.Add(new Platform(new Vector2(1013, 529), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
            platforms.Add(new Platform(new Vector2(1391, 280), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
            platforms.Add(new Platform(new Vector2(1728, 50), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
            solidPlatforms.Add(new SolidPlatform(new Vector2(1223, 785), Content.Load<Texture2D>("MyPlatforms"), Color.White, 1.5f));
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
            MouseState ms = Mouse.GetState();


            KeyboardState ks = Keyboard.GetState();



            Window.Title = $"X: {ms.X}, Y: {ms.Y}";

            if (ks.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

                    player.Update(gameTime);
            bool onPlatform = false;
            for (int i = 0; i < platforms.Count; i++)
            {
                if (player.Hitbox.Intersects(platforms[i].Hitbox))
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
                    else if(player.velocity.Y != 0 && //shrink or grow angle to remove this line
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
                platforms[j].Draw(spriteBatch);
            }
            for (int z = 0; z < solidPlatforms.Count; z++)
            {
                solidPlatforms[z].Draw(spriteBatch);
            }
            //spriteBatch.Draw(singlePixel, player.Hitbox, Color.Blue * 0.40f);
            // spriteBatch.Draw(singlePixel, player.Position, Color.Red);
            //for (int i = 0; i < solidPlatforms.Count; i++)
           // {
           //     spriteBatch.Draw(singlePixel, solidPlatforms[i].Hitbox, Color.Red * 0.40f);
          //  }
           

          //  spriteBatch.Draw(singlePixel, platform.Position, Color.Blue);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
