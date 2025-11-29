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

        // protected Vector2 objectDirection;
        protected Vector2 velocity;
        // protected Vector2 objectDestination;

        public bool objectMoving;
        protected bool isOnGround;

        public Moveable(int totalFrame, Vector2 frameSize)
        {
            this.totalFrame = totalFrame;
            this.frameSize = frameSize;
            srcRec = new Rectangle(0,0, (int) frameSize.X,(int) frameSize.Y);
            origin = new Vector2((int)frameSize.X / 2, (int)frameSize.Y / 2);
            isOnGround = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            hitBoxLive.X = (int)pos.X;
            hitBoxLive.Y = (int)pos.Y;
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
            objectMoving = true;
            animationFX = SpriteEffects.None;
            rotation = 0;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            rotation = MathHelper.ToRadians(0);
            //ChangeDirection(new Vector2(-1, 0));
        }

        public void TurnRight(GameTime gameTime)
        {
            objectMoving = true;
            animationFX = SpriteEffects.FlipHorizontally;
            rotation = 0;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            rotation = MathHelper.ToRadians(0);
            // ChangeDirection(new Vector2(-1, 0));
        }

        public void Jump(GameTime gameTime)
        {
            objectMoving = true;
            velocity.Y = -200f;
            isOnGround = false;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
            {
                spriteBatch.Draw(tex, pos, srcRec, Color.White, rotation, origin, scale, animationFX, 1);
            }
        }
    }
}
