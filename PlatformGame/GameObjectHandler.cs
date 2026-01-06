using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlatformGame
{
    class GameObjectHandler
    {
        public List<CollidableObject> objects = new();

        public void Update(GameTime gameTime)
        {
            foreach (var obj in objects)
            {
                if (obj is Moveable m)
                    m.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obj in objects)
            {
                obj.Draw(spriteBatch);
            }
        }
    }

}
