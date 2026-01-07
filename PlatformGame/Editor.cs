using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace PlatformGame
{
    internal class Editor
    {
        List<CollidableObject> gameObjectList;
        bool isSaved;
        const int tileSize = 50;
        const int enemySize = 48;
        const int playerSize = 40;
        Player player;
        public bool IsSaved
        {
            get { return isSaved; } 
        }

        public Editor()
        {
            gameObjectList = new List<CollidableObject>();
            isSaved = false;
        }
        public void Update()
        {
            int x = (PlayerKeyReader.mouseState.X / tileSize) * tileSize;
            int y = (PlayerKeyReader.mouseState.Y / tileSize) * tileSize;

            if (PlayerKeyReader.KeyPressed(Keys.P)) // Make a platform
            {
                Platform platform = new Platform(new Rectangle(x, y, tileSize, tileSize), false);
                gameObjectList.Add(platform);
            }
            else if(PlayerKeyReader.KeyPressed(Keys.M)) //Player's spawn point
            {
               player = new Player(TextureManager.allLinkTex, new Vector2((int)x,(int)y), 8, new Vector2(playerSize,playerSize), 0, 0);
                gameObjectList.Add(player);
            }
            else if (PlayerKeyReader.KeyPressed(Keys.E)) // Make an enemy
            {
                Enemy enemy = new Enemy(TextureManager.allLinkTex, new Vector2(x, y+tileSize-enemySize), 1, new Vector2(enemySize, enemySize), 0, 161, player);
                gameObjectList.Add(enemy);
            }
            else if (PlayerKeyReader.KeyPressed(Keys.G)) // Goal point
            {
                Platform platform = new Platform(new Rectangle(x, y, tileSize, tileSize), true);
                gameObjectList.Add(platform);
            }
            else if (PlayerKeyReader.KeyPressed(Keys.S)) // Save to file
            {
                JsonFileHandler.WriteToJsonFile("level_editor.json", gameObjectList);
                isSaved = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(CollidableObject go in gameObjectList)
            {
                go.Draw(spriteBatch);
            }
        }
    }
}
