using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        Editor editor;

        //GameState
        static GameState gameState;
        enum GameState
        {
            Starting,
            Playing,
            GameOver,
            Victory,
            Editor
        }

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
                bool isGoal = rec.X == 500 && rec.Y == 130;
                Platform platform = new Platform(rec, isGoal);
                platformList.Add(platform);
            }
        }

        public void ReadEnemiesFromFile(string fileName)
        {
            List<Rectangle> enemyRecList = JsonFileHandler.AllInOneRecList(fileName, "enemies");

            foreach (Rectangle rec in enemyRecList)
            {
                Enemy enemy = new Enemy(TextureManager.allLinkTex, new Vector2(rec.X, rec.Y), 1, new Vector2(rec.Width, rec.Height), 0, 161, player);

                // Find the platform the enemy is on
                foreach (Platform p in platformList)
                {
                    if (rec.Bottom == p.hitBoxLive.Top) // simple check: enemy is on top of platform
                    {
                        enemy.SetPlatform(p);
                        break;
                    }
                }

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

            Rectangle playerRec = JsonFileHandler.AllInOneRec("level_1-1.json", "player");
            player = new Player(TextureManager.allLinkTex, new Vector2(playerRec.X, playerRec.Y), 8, new Vector2(playerRec.Width, playerRec.Height), 0, 0);
            handler.objects.Add(player);

            foreach (Platform p in platformList)
            {
                handler.objects.Add(p);
            }
            ReadEnemiesFromFile("level_1-1.json");
            player.enemies = enemyList;
            editor = new Editor();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            PlayerKeyReader.Update();

            if (gameState == GameState.Starting)
            {
                if (PlayerKeyReader.KeyPressed(Keys.Enter) || PlayerKeyReader.KeyPressed(Keys.Space))
                {
                    gameState = GameState.Playing;
                }
                if (PlayerKeyReader.KeyPressed(Keys.R))
                {
                    gameState = GameState.Editor;
                }
            }

            if (gameState == GameState.Playing)
            {

                player.isOnGround = false;
                player.Animation(gameTime);
                player.ClampToScreen(GraphicsDevice.Viewport);

                foreach (Platform p in platformList)
                {
                    if (p.isGoal)
                    {
                        Rectangle playerBox = player.hitBoxLive;
                        Rectangle goalBox = p.hitBoxLive;

                        bool landingOnTop = player.velocity.Y > 0 && playerBox.Bottom <= goalBox.Top + 5 && playerBox.Right > goalBox.Left && playerBox.Left < goalBox.Right;

                        if (landingOnTop)
                        {
                            gameState = GameState.Victory;
                            return;
                        }
                    }

                    // Normal collision
                    player.CollidingWithPlatform(p);
                }


                handler.Update(gameTime);

                for (int i = enemyList.Count - 1; i >= 0; i--)
                {
                    if (enemyList[i].maxHP <= 0)
                    {
                        handler.objects.Remove(enemyList[i]); // also remove from handler
                        enemyList.RemoveAt(i);
                    }
                }
                if (enemyList.Count == 0)
                {
                    gameState = GameState.Victory;
                }
            }

            if (player.maxHP == 0)
            {
                gameState = GameState.GameOver;
            }

            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                if (PlayerKeyReader.KeyPressed(Keys.Enter) || PlayerKeyReader.KeyPressed(Keys.Space))
                {
                    // Restart the game
                    handler.objects.Clear();
                    platformList.Clear();
                    enemyList.Clear();
                    LoadContent();
                    gameState = GameState.Starting;
                }
            }

            if (gameState == GameState.Editor)
            {
                editor.Update();
                Window.Title = "EDITOR: " + "is saved: " + editor.IsSaved;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (gameState == GameState.Playing)
            {
                _spriteBatch.Begin();
                handler.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            if (gameState == GameState.Editor)
            {
                _spriteBatch.Begin();
                editor.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
