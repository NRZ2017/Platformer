using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using System.Linq;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// Sprite : basic
        /// Animation : list of rectangles
        /// Player : Sprite, enum of states, dictionary(state, animation)
        /// Plateform : Sprite
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(new Vector2(100, 300), Content.Load<Texture2D>("sheet"), Color.White);
            run = new Animation();
            jump = new Animation();
            idle = new Animation();
            death = new Animation();
            player.dictionary.Add(Player.States.Run, run);
            player.dictionary.Add(Player.States.Jump, jump);
            player.dictionary.Add(Player.States.Idle, idle);
            player.dictionary.Add(Player.States.Death, death);
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

                else if(element.Attribute("n").Value.Contains("Jump"))
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
            player.Position += player.speed;
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Right))
            {
                player.CurrentState = Player.States.Run;
                player.CurrentDirection = Player.Direction.Right;
                player.speed.X = 5;
            }
            else if (ks.IsKeyDown(Keys.Up))
            {
                player.CurrentState = Player.States.Jump;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                player.CurrentState = Player.States.Death;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                player.CurrentState = Player.States.Run;

                player.speed.X = -5;
                player.CurrentDirection = Player.Direction.Left;
            }
            else
            {
                player.CurrentState = Player.States.Idle;
                player.speed.X = 0;
            }
            player.Update(gameTime);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
