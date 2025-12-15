using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlatformGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Rectangle screenWidth;

        Camera camera;
        Viewport viewport;

        Player player;
        Vector2 frameSize = new Vector2(40,40);

        Enemy enemy;
        List<Enemy> enemyList = new List<Enemy>();

        Platform platform;
        List<Platform> platforms = new List<Platform>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            screenWidth = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            viewport = GraphicsDevice.Viewport;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Textures(Content);
            camera = new Camera(viewport);

            player = new Player(TextureManager.allLinkTex, new Vector2(200,400), 8, frameSize, 0,0);

            enemy = new Enemy(TextureManager.allLinkTex, new Vector2(250, 400), 1, frameSize,0,161);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PlayerKeyReader.Update();
            //camera.SetPosition(player.pos - new Vector2(player.hitBoxLive.Width, player.hitBoxLive.Height));
            player.Update(gameTime);

            if (player.objectMoving)
            {
                player.Animation(gameTime);
            }

            enemy.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(/*SpriteSortMode.Deferred, null, null, null, null, null, camera.Transform*/);
            player.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
