using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlatformGame
{
    internal class Player : Moveable
    {
        public int lives = 3;
        private float collisionInterval = 1.0f;
        private float currentCD = .5f;

        public Player(Texture2D tex, Vector2 pos, int TotalFrame, Vector2 frameSize) :base(TotalFrame, frameSize)
        {
            this.tex = tex;
            this.pos = pos;
            hitBoxLive = new Rectangle( (int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200,100);
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            objectMoving = false;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isOnGround)
            {
                velocity.Y += 200f * dt;
                pos.Y += velocity.Y * dt;
            }
            if (pos.Y >= 400)
            {
                pos.Y = 400;
                isOnGround = true;
                velocity.Y = 0;
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
