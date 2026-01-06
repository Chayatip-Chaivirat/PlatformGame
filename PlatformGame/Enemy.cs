using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PlatformGame
{
    internal class Enemy : Moveable
    {
        Random random = new Random();
        int movementCode;
        Player player;
        public List<Platform> platformList;

        private Platform myPlatform;
        private float leftBound;
        private float rightBound;

        public Enemy(Texture2D tex, Vector2 pos, int totalFrame, Vector2 frameSize, int recX, int recY, Player player) : base(totalFrame, frameSize, recX, recY)
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
            detectionRangeWidth = tex.Width * 2;
            detectionRangeHeight = tex.Height;
            detectionRangeRight = new Rectangle((int)pos.X + tex.Width, (int)pos.Y, detectionRangeWidth, tex.Height);
            detectionRangeLeft = new Rectangle((int)pos.X - detectionRangeWidth, (int)pos.Y, detectionRangeWidth, tex.Height);
            movementCode = random.Next(1, 3); // From 1 to 2
            baseAttack = 3;
            currentCD = 0.0f;
            normalAttackCD = 3f;

            this.player = player;
            isOnGround = true;

            //FaceRight();

        }
        public void AssignPlatform(List<Platform> platforms)
        {
            foreach (Platform p in platforms)
            {
                bool xOverlap = hitBoxLive.Right > p.hitBoxLive.Left && hitBoxLive.Left < p.hitBoxLive.Right;

                bool abovePlatform = pos.Y + hitBoxLive.Height <= p.hitBoxLive.Top + 20;

                if (xOverlap && abovePlatform)
                {
                    myPlatform = p;
                    leftBound = p.hitBoxLive.Left;
                    rightBound = p.hitBoxLive.Right - hitBoxLive.Width;
                    return;
                }
            }
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
        //public override void Update(GameTime gameTime)
        //{
        //    // Update hitbox so it matches new position
        //    hitBoxLive.Location = pos.ToPoint();

        //    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    currentCD += dt;

        //    // Update detection ranges
        //    detectionRangeLeft.X = (int)pos.X - detectionRangeWidth;
        //    detectionRangeLeft.Y = (int)pos.Y;

        //    detectionRangeRight.X = (int)pos.X + tex.Width;
        //    detectionRangeRight.Y = (int)pos.Y;

        //    // Move
        //    pos.X += velocity.X * dt;

        //    // Clamp to platform
        //    if (pos.X <= leftBound)
        //    {
        //        pos.X = leftBound;
        //        TurnRight(gameTime);
        //    }
        //    else if (pos.X >= rightBound)
        //    {
        //        pos.X = rightBound;
        //        TurnLeft(gameTime);
        //    }


        //    //Movement and Attack Logic

        //    if (!attacking) { color = Color.White; }

        //    if (DetectedLeft(player))
        //    {
        //        TurnLeft(gameTime);
        //        pos.X += velocity.X * dt;
        //        if (currentCD >= normalAttackCD)
        //        {
        //            NormalAttack(gameTime, player);
        //            currentCD = 0.0f;
        //            color = Color.Red; // For testing
        //        }
        //    }
        //    else if (DetectedRight(player))
        //    {
        //        TurnRight(gameTime);
        //        pos.X += velocity.X * dt;
        //        if (currentCD >= normalAttackCD)
        //        {
        //            NormalAttack(gameTime, player);
        //            currentCD = 0.0f;
        //            color = Color.Red; // For testing
        //        }
        //    }

        //    base.Update(gameTime);
        //}

        public override void Update(GameTime gameTime)
        {
            hitBoxLive.Location = pos.ToPoint();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            pos.X += velocity.X * dt;

            if (pos.X <= leftBound)
            {
                pos.X = leftBound;
                TurnRight(gameTime);
            }
            else if (pos.X >= rightBound)
            {
                pos.X = rightBound;
                TurnLeft(gameTime);
            }

            // Player detection and chasing
            if (DetectedLeft(player) && pos.X > leftBound)
            {
                TurnLeft(gameTime);
            }
            else if (DetectedRight(player) && pos.X < rightBound)
            {
                TurnRight(gameTime);
            }


            base.Update(gameTime);
        }

    }
}
