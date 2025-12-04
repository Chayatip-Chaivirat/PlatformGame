using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PlatformGame
{
    internal class Enemy : Moveable
    {
        private int detectionRangeWidth;
        Random random = new Random();
        int movementCode;
        Player player;
        public Enemy(Texture2D tex, Vector2 pos, int totalFrame, Vector2 frameSize) : base (totalFrame, frameSize)
        {
            this.tex = tex;
            this.pos = pos;
            hitBoxLive = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200, 100);
            color = Color.White;
            maxHP = 10;
            attackHitBox = Rectangle.Empty;
            detectionRangeWidth = tex.Width * 3;
            detectionRange = new Rectangle((int)pos.X + tex.Width /2 - detectionRangeWidth / 2, (int)pos.Y, detectionRangeWidth, tex.Height);
            movementCode = random.Next(1,3); // From 1 to 2
            baseAttack = 3;
        }

        public void ChangeMovementCode()
        {
            int newMovementCode;
            do
            {
                newMovementCode = random.Next(1, 3); //From 1 to 2
            }
            while (newMovementCode == movementCode);

            movementCode = newMovementCode;
        }
        public override void Update(GameTime gameTime)
        {
            if(Detected(player))
            {
                NormalAttack(gameTime, player);
            }
            base.Update(gameTime);
        }
    }
}
