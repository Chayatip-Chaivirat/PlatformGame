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

        List<Platform> platformList = new List<Platform>();
        List<Enemy> enemyList = new List<Enemy>();

        GameObjectHandler handler;
        Player player;
        Vector2 frameSize = new Vector2(40, 40);

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
        }
        public void ReadPlatformFromFile(string fileName)
        {
            List<Rectangle> platformRectList = JsonFileHandler.AllInOneRecList(fileName, "platforms");
            foreach (Rectangle rec in platformRectList)
            {
                Platform platform = new Platform(rec);
                platformList.Add(platform);
            }
        }

        public void ReadEnemiesFromFile(string fileName)
        {
            List<Rectangle> enemyRecList = JsonFileHandler.AllInOneRecList(fileName, "enemies");

            foreach (Rectangle rec in enemyRecList)
            {
                Enemy enemy = new Enemy(TextureManager.allLinkTex, new Vector2(rec.X, rec.Y), 1,new Vector2(rec.Width, rec.Height), 0, 161,player);

                enemyList.Add(enemy);
                handler.objects.Add(enemy); 
            }
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            handler = new GameObjectHandler();
            TextureManager.Textures(Content);

            ReadPlatformFromFile("level_1-1.json");
            ReadEnemiesFromFile("level_1-1.json");

            Rectangle playerRec = JsonFileHandler.AllInOneRec("level_1-1.json", "player");
            player = new Player(TextureManager.allLinkTex,new Vector2(playerRec.X, playerRec.Y),8,frameSize,0,0);
            handler.objects.Add(player);

            foreach (Platform p in platformList)
            {
                handler.objects.Add(p);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PlayerKeyReader.Update();

            handler.Update(gameTime);

            player.isOnGround = false;

            player.Animation(gameTime);

            foreach (Platform p in platformList)
            {
                player.CollidingWithPlatform(p);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            handler.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
