using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlatformGame
{
    internal class Player : Moveable
    {
        private float collisionInterval = 1.0f;

        Enemy enemy;
        Platform platform;
        public Player(Texture2D tex, Vector2 pos, int totalFrame, Vector2 frameSize, int recX, int recY) :base(totalFrame, frameSize, recX, recY)
        {
            this.tex = tex;
            this.pos = pos;
            hitBoxLive = new Rectangle( (int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200,100);
            color = Color.White;
            maxHP = 10;
            attackHitBox = Rectangle.Empty;
            baseAttack = 3;
            currentCD = 0.0f;
            normalAttackCD = 1f;
        }

        public void CollidingWithPlatform(Platform platform)
        {
            hitBoxLive.X = (int)pos.X;
            hitBoxLive.Y = (int)pos.Y;

            if (platform == null) return;
            if (!hitBoxLive.Intersects(platform.hitBoxLive))
                return;

            Rectangle intersection = Rectangle.Intersect(this.hitBoxLive, platform.hitBoxLive);

            // Vertical
            if (intersection.Height < intersection.Width)
            {
                if (velocity.Y > 0) // From top
                {
                    pos.Y -= intersection.Height;
                    isOnGround = true;
                    velocity.Y = 0;
                }

                else if (velocity.Y < 0) // From bottom
                {
                    pos.Y += intersection.Height;
                    velocity.Y = 0;
                }
            }

            // Horizontal
            else
            {
                if (hitBoxLive.Top >= platform.hitBoxLive.Top + 10)
                {
                    if (velocity.X > 0)
                    {
                        pos.X -= intersection.Width;
                    }
                    else if (velocity.X < 0)
                    {
                        pos.X += intersection.Width;
                    }
                    velocity.X = 0;
                }
            }

            // Update hitbox so it matches new position
            hitBoxLive.Location = pos.ToPoint();
            // Location refers to the top-left corner of the rectangle
            // ToPoint() converts Vector2 to Point (int)
        }

        public override void Update(GameTime gameTime)
        {
            objectMoving = false;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isOnGround)
            {
                velocity.Y += 500f * dt;
                pos.Y += velocity.Y * dt;
            }
            if (pos.Y >= 400) // temp solution for not having a platform
            {
                pos.Y = 400;
                isOnGround = true;
                velocity.Y = 0;
            }
            if (!attacking)
            {
                srcRec = new Rectangle(0, 0, (int)frameSize.X, (int)frameSize.Y);
            }
            if (attacking)
            {
                srcRec = new Rectangle((int) frameSize.X, 120, (int)frameSize.X, (int)frameSize.Y);
            }

            if (currentCD > 0)
            {
                currentCD -= dt;
            }
            if (currentCD > 0)
            {
                attacking = false;
            }

            if (PlayerKeyReader.KeyPressedAndHold(Keys.A) || PlayerKeyReader.KeyPressedAndHold(Keys.Left))
            {
                TurnLeft(gameTime);
                pos.X += velocity.X * dt;
            }
            else if (PlayerKeyReader.KeyPressedAndHold(Keys.D) || PlayerKeyReader.KeyPressedAndHold(Keys.Right))
            {
                TurnRight(gameTime);
                pos.X += velocity.X * dt;
            }
            else if (PlayerKeyReader.KeyPressedAndHold(Keys.F) || PlayerKeyReader.LeftClick() && currentCD <= 0)
            {
                // Attack logic
                // Don't forget attack CD
                NormalAttack(gameTime, enemy);
                currentCD = normalAttackCD;
            }
            else
            {
                velocity.X = 0;
                //srcRec = new Rectangle(0, 0, (int)frameSize.X, (int)frameSize.Y);
            }
            if (PlayerKeyReader.KeyPressedAndHold(Keys.Space) && isOnGround)
            {
                Jump(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
