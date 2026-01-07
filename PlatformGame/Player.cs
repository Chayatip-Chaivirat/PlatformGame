using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            hitBoxLive = new Rectangle((int)pos.X, (int)pos.Y,40,40);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200, 100);
            color = Color.White;
            maxHP = 10;
            attackHitBox = Rectangle.Empty;
            baseAttack = 3;
            currentCD = 0.0f;
            normalAttackCD = 0.5f;
        }

        public void CollidingWithPlatform(Platform platform)
        {
            if (platform == null) return;

            hitBoxLive.Location = pos.ToPoint(); // Location refers to the top-left corner of the rectangle
            // ToPoint() converts Vector2 to Point (int)

            if (!hitBoxLive.Intersects(platform.hitBoxLive))
                return;

            Rectangle intersection = Rectangle.Intersect(hitBoxLive, platform.hitBoxLive);

            // Vertical collision
            if (intersection.Height < intersection.Width)
            {
                if (velocity.Y > 0) // falling
                {
                    pos.Y = platform.hitBoxLive.Top - hitBoxLive.Height;
                    velocity.Y = 0;
                    isOnGround = true;
                }
                else if (velocity.Y < 0) // jumping up
                {
                    pos.Y = platform.hitBoxLive.Bottom;
                    velocity.Y = 0;
                }
            }
            else // Horizontal collision
            {
                if (velocity.X > 0)
                {
                    pos.X = platform.hitBoxLive.Left - hitBoxLive.Width;
                }
                else if (velocity.X < 0)
                {
                    pos.X = platform.hitBoxLive.Right;
                }
                velocity.X = 0;
            }

            // sync again after correction
            hitBoxLive.Location = pos.ToPoint();
        }


        private Rectangle GetAttackHitBox()
        {
            int width = (int)frameSize.X; 
            int height = (int)frameSize.Y;
            if (faceRight)
            { 
                    return new Rectangle((int)pos.X + hitBoxLive.Width, (int)pos.Y, width, height); 
            }
            else
            {
                return new Rectangle((int)pos.X - width, (int)pos.Y, width, height); 
            }
        }

        // Ensure the player stays within the screen bounds
        public void ClampToScreen(Viewport viewport)
        {
            pos.X = MathHelper.Clamp(pos.X,0,viewport.Width - hitBoxLive.Width);

            pos.Y = MathHelper.Clamp( pos.Y, 0,viewport.Height - hitBoxLive.Height);

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
            else if (PlayerKeyReader.KeyPressedAndHold(Keys.F) || PlayerKeyReader.LeftClick())
            {
                if (currentCD <= 0)
                {
                    attacking = true;
                    attackHitBox = GetAttackHitBox(); 

                    Enemy closest = null;
                    float closestDistance = float.MaxValue;

                    foreach (Enemy e in enemies)
                    {
                        if (e == null || e.maxHP <= 0) continue;

                        if (attackHitBox.Intersects(e.hitBoxLive))
                        {
                            float distance = Vector2.Distance(this.pos, e.pos); // Calculate distance to enemy
                            if (distance < closestDistance) // Find the closest enemy
                            {
                                closestDistance = distance; // Update closest distance
                                closest = e;
                            }
                        }
                    }

                    if (closest != null) // If an enemy was hit
                    {
                        closest.maxHP -= baseAttack;
                    }

                    currentCD = normalAttackCD; // reset cooldown
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
