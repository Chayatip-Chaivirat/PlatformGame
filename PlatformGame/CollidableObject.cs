using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlatformGame
{
    abstract class CollidableObject
    {
        protected Texture2D tex;
        public Vector2 pos;
        public Rectangle hitBoxLive;
        protected Color color;
        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void CollisionHandler(List<CollidableObject> listName)
        {
            listName.Remove(this); //this refers to the object in the class this method is used in
        }
    }
}
