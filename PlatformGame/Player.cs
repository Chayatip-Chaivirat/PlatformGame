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
        public List<Enemy> enemies;
        public Player(Texture2D tex, Vector2 pos, int totalFrame, Vector2 frameSize, int recX, int recY) : base(totalFrame, frameSize, recX, recY)
        {
            this.tex = tex;
            this.pos = pos;
            hitBoxLive = new Rectangle((int)pos.X, (int)pos.Y, (int) frameSize.X, (int) frameSize.Y);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200, 100);
            color = Color.White;
            maxHP = 10;
            attackHitBox = Rectangle.Empty;
            baseAttack = 3;
            currentCD = 0.0f;
            normalAttackCD = 1f;
        }

        public void CollidingWithPlatform(Platform platform)
        {

            if (platform == null) return;
            if (!hitBoxLive.Intersects(platform.hitBoxLive))
                return;

            // Update hitbox so it matches new position
            hitBoxLive.Location = pos.ToPoint();
            // Location refers to the top-left corner of the rectangle
            // ToPoint() converts Vector2 to Point (int)

            Rectangle intersection = Rectangle.Intersect(this.hitBoxLive, platform.hitBoxLive);

            // Vertical
            if (intersection.Height < intersection.Width)
            {
                if (velocity.Y > 0) // From top
                {
                    pos.Y = platform.hitBoxLive.Top - hitBoxLive.Height;
                    isOnGround = true;
                    velocity.Y = 0;
                }

                else if (velocity.Y < 0) // From bottom
                {
                    pos.Y = platform.hitBoxLive.Bottom + hitBoxLive.Height;
                    velocity.Y = 0;
                }
            }

            // Horizontal
            else
            {
                if (velocity.X > 0) // Moving right
                {
                    pos.X = platform.hitBoxLive.Left - hitBoxLive.Width;
                    velocity.X = 0;
                }
                else if (velocity.X < 0) // Moving left
                {
                    pos.X = platform.hitBoxLive.Right + hitBoxLive.Width;
                    velocity.X = 0;
                }
            }
            hitBoxLive.Location = pos.ToPoint();
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

            if (!attacking)
            {
                srcRec = new Rectangle(0, 0, (int)frameSize.X, (int)frameSize.Y);
            }
            if (attacking)
            {
                srcRec = new Rectangle((int)frameSize.X, 120, (int)frameSize.X, (int)frameSize.Y);
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
                foreach (Enemy enemy in enemies)
                {
                    if (enemies != null)
                    {
                        foreach (Enemy e in enemies)
                        {
                            NormalAttack(gameTime, e);
                        }
                    }
                    currentCD = normalAttackCD;
                }
            }
            else
            {
                velocity.X = 0;
            }
            if (PlayerKeyReader.KeyPressedAndHold(Keys.Space) && isOnGround)
            {
                Jump(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
