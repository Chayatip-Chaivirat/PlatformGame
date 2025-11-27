using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PlatformGame
{
    internal class Moveable : CollidableObject
    {
        protected int frame;
        protected int totalFrame;
        protected Vector2 frameSize;
        protected double frameTimer = 100, frameInterval = 100;
        protected Rectangle srcRec;

        //protected SpriteEffects animationFX = SpriteEffects.None;
        protected float rotation = 0;
        protected float scale;
        protected Vector2 origin;

        protected Vector2 objectDirection;
        protected float speed;
        protected bool objectMoving;
        protected Vector2 objectDestination;

        public Moveable(int totalFrame, Rectangle srcRec, Vector2 frameSize)
        {
            this.totalFrame = totalFrame;
            this.srcRec = srcRec;
            this.frameSize = frameSize;
            origin = new Vector2((int)frameSize.X / 2, (int)frameSize.Y / 2);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}
