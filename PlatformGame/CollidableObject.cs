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
        protected Vector2 pos;
        protected Rectangle hitBoxLive;
        protected Rectangle attackHitBox;
        protected Rectangle detectionRange;
        protected Color color;
        public abstract void Draw(SpriteBatch spriteBatch);

        public bool Attacked(CollidableObject other)
        {
            if(other == null)
            {
                return false;
            }

            return this.attackHitBox.Intersects(other.hitBoxLive);
        }

        public bool Detected(CollidableObject other)
        {
            if (other == null)
            {
                return false;
            }

            return this.detectionRange.Intersects(other.hitBoxLive);
        }

        public virtual void CollisionHandler(List<CollidableObject> listName)
        {
            listName.Remove(this); //this refers to the object in the class this method is used in
        }
    }
}
