using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata.Ecma335;

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
        protected int recX;
        protected int recY;

        public int maxHP;
        public int baseAttack;

        public bool objectMoving;

        protected bool faceRight = false;
        protected bool faceLeft = false;
        protected bool attacking = false;
        public bool isOnGround;

        protected float currentCD;
        protected float normalAttackCD;
        protected Rectangle detectionRangeLeft;
        protected Rectangle detectionRangeRight;
        protected int detectionRangeWidth;
        protected int detectionRangeHeight;
        protected Rectangle attackHitBox;

        public Moveable(int totalFrame, Vector2 frameSize, int recX, int recY)
        {
            this.totalFrame = totalFrame;
            this.frameSize = frameSize;
            this.recX = recX;
            this.recY = recY;
            srcRec = new Rectangle(recX, recY, (int)frameSize.X, (int)frameSize.Y);
            isOnGround = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            hitBoxLive.X = (int)pos.X - (int)frameSize.X/2;
            hitBoxLive.Y = (int)pos.Y - (int)frameSize.Y/2;
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

        public bool DetectedLeft(Moveable other)
        {
            if (other == null)
            {
                return false;
            }
            return this.detectionRangeLeft.Intersects(other.hitBoxLive);
        }

        public bool DetectedRight(Moveable other)
        {
            if (other == null)
            {
                return false;
            }
            return this.detectionRangeRight.Intersects(other.hitBoxLive);
        }

        public bool Attacked(Moveable other)
        {
            if (other == null)
            {
                return false;
            }

            return this.attackHitBox.Intersects(other.hitBoxLive);
        }
        protected void FaceLeft()
        {
            animationFX = SpriteEffects.None;
            velocity.X = -150f;
            faceLeft = true;
            faceRight = false;
        }

        protected void FaceRight()
        {
            animationFX = SpriteEffects.FlipHorizontally;
            velocity.X = 150f;
            faceRight = true;
            faceLeft = false;
        }

        public void TurnLeft(GameTime gameTime)
        {
            objectMoving = true;
            FaceLeft();

            if (gameTime != null)
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void TurnRight(GameTime gameTime)
        {
            objectMoving = true;
            FaceRight();

            if (gameTime != null)
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Jump(GameTime gameTime)
        {
            objectMoving = true;
            velocity.Y = -400f;
            isOnGround = false;
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public virtual void NormalAttack(GameTime gameTime, Moveable other)
        {
            attacking = true;
            if (faceRight)
            {
                attackHitBox = new Rectangle((int)pos.X + hitBoxLive.Width, (int)pos.Y, tex.Width, tex.Height);
                if (Attacked(other))
                {
                    other.maxHP -= baseAttack;
                }
            }
            else if (faceLeft)
            {
                attackHitBox = new Rectangle((int)pos.X - tex.Width, (int)pos.Y, tex.Width, tex.Height);
                if (Attacked(other))
                {
                    other.maxHP -= baseAttack;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
            {
                //spriteBatch.Draw(TextureManager.wallTex, hitBoxLive, color);
                spriteBatch.Draw(tex, pos, srcRec, color);
            }
        }
    }
}
