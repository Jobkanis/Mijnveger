using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mijnveger
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public enum Platform { windows, android }

    public class Game1 : Game
    {
        public Platform currentplatform;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Game_Manager game_manager;
        Drawvisitor drawvisitor;
        iInputhandler inputhandler;
        SpriteFont arial;

        Texture2D emptyTexture;
        Texture2D Black_transparent_Texture;
        Texture2D White_transparent_Texture;

        public int width;
        public int height;

        public Game1(Platform currentplatform, int width, int height)
        {
            this.currentplatform = currentplatform;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.width = width;
            this.height = height;
            graphics.PreferredBackBufferWidth = this.width;
            graphics.PreferredBackBufferHeight = this.height;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            emptyTexture = Content.Load<Texture2D>("white_pixel");
            Black_transparent_Texture = Content.Load<Texture2D>("Black_transparant");
            White_transparent_Texture = Content.Load<Texture2D>("White_transparant");
            arial = Content.Load<SpriteFont>("Arial");

            this.drawvisitor = new Drawvisitor(spriteBatch, this.currentplatform, width, height, emptyTexture, Black_transparent_Texture, White_transparent_Texture, arial);
            if (currentplatform == Platform.windows)
            {
                this.inputhandler = new WindowsInputHandler();
            }
            else
            {
                this.inputhandler = new AndroidInputHandler();
            }
            this.game_manager = new Game_Manager(currentplatform, drawvisitor, inputhandler);
        }

        protected override void UnloadContent()
        {
        }


        float updatetimenow = 0;
        float updateprevtime = 0;
        float updatetimedifference = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here


            updateprevtime = updatetimenow;
            updatetimenow = (float)gameTime.TotalGameTime.TotalMilliseconds;
            updatetimedifference = (updatetimenow - updateprevtime) / 1000;
            if (game_manager != null)
            {
                game_manager.Update((updatetimedifference));
                if (game_manager.Exit == true)
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        float drawprevtime = 0;
        float drawtimenow = 0;
        float drawtimedifference = 0;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            drawprevtime = updatetimenow;
            drawtimenow = (float)gameTime.TotalGameTime.TotalMilliseconds;
            drawtimedifference = (updatetimenow - updateprevtime) / 1000;

            // TODO: Add your drawing code here
            if (game_manager != null)
            {
                game_manager.Draw(drawtimedifference);
            }
            base.Draw(gameTime);
        }
    }

}
