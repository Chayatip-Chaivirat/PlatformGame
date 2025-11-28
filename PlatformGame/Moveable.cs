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

        protected SpriteEffects animationFX = SpriteEffects.None;
        protected float rotation = 0;
        protected float scale;
        protected Vector2 origin;

        protected Vector2 objectDirection;
        protected float speed;
        protected Vector2 objectDestination;

        protected bool objectMoving;
        protected bool IsOnGround;

        public Moveable(int totalFrame, Vector2 frameSize)
        {
            this.totalFrame = totalFrame;
            srcRec = new Rectangle(0,0,20,40);
            this.frameSize = frameSize;
            origin = new Vector2((int)frameSize.X / 2, (int)frameSize.Y / 2);
        }

        public virtual void Update()
        {

        }

        public void Animation(GameTime gameTime)
        {
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval; frame++;
                srcRec.X = (frame % totalFrame) * (int)frameSize.X;
            }
        }

        public void TurnLeft(GameTime gameTime)
        {
            animationFX = SpriteEffects.None;
            rotation = 0;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            rotation = MathHelper.ToRadians(0);
            //ChangeDirection(new Vector2(-1, 0));
        }

        public void TurnRight(GameTime gameTime)
        {
            animationFX = SpriteEffects.FlipHorizontally;
            rotation = 0;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            rotation = MathHelper.ToRadians(0);
           // ChangeDirection(new Vector2(-1, 0));
        }

        public void Jump(GameTime gameTime)
        {
            pos.Y -= 2.5f;
            IsOnGround = false;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
            {
                Vector2 posOnTile = pos + origin;
                spriteBatch.Draw(tex, posOnTile, srcRec, Color.White, rotation, origin, scale, animationFX, 1);
            }
        }
    }
}
