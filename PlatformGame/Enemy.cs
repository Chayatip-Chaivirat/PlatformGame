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
        Random random = new Random();
        int movementCode;
        Player player;
        Platform currentPlatform;

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
        }

        // Set the current platform the enemy is on
        public void SetPlatform(Platform platform)
        {
            currentPlatform = platform;
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
        //    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    currentCD += dt;

        //    // Update detection ranges
        //    detectionRangeLeft.X = (int)pos.X - detectionRangeWidth;
        //    detectionRangeLeft.Y = (int)pos.Y;

        //    detectionRangeRight.X = (int)pos.X + tex.Width;
        //    detectionRangeRight.Y = (int)pos.Y;

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
        //    else
        //    {

        //        //for testing
        //        //if (pos.X <= patrollRangeLeft)
        //        //{
        //        //    movementCode = 1; // Move right
        //        //}
        //        //else if (pos.X >= patrollRangeRight)
        //        //{
        //        //    movementCode = 2; // Move left
        //        //}

        //        //if (movementCode == 1)
        //        //{
        //        //    TurnRight(gameTime);
        //        //    pos.X += velocity.X * dt;
        //        //}
        //        //else if (movementCode == 2)
        //        //{
        //        //    TurnLeft(gameTime);
        //        //    pos.X += velocity.X * dt;
        //        //}
        //        velocity.X = 0;
        //    }
        //    base.Update(gameTime);
        //}

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentCD += dt;

            // Update detection ranges
            detectionRangeLeft.X = (int)pos.X - detectionRangeWidth;
            detectionRangeLeft.Y = (int)pos.Y;
            detectionRangeRight.X = (int)pos.X + tex.Width;
            detectionRangeRight.Y = (int)pos.Y;

            // Movement logic
            if (!attacking) color = Color.White;

            if (DetectedLeft(player))
            {
                TurnLeft(gameTime);
                MoveWithPlatformCheck(dt);
                AttackIfReady(gameTime, player);
            }
            else if (DetectedRight(player))
            {
                TurnRight(gameTime);
                MoveWithPlatformCheck(dt);
                AttackIfReady(gameTime, player);
            }
            else
            {
                velocity.X = 0; // idle
            }

            base.Update(gameTime);
        }

        // Move but stop at platform edges
        private void MoveWithPlatformCheck(float dt)
        {
            if (currentPlatform == null) return;

            float nextPosX = pos.X + velocity.X * dt;

            // Platform boundaries
            float leftEdge = currentPlatform.hitBoxLive.Left;
            float rightEdge = currentPlatform.hitBoxLive.Right - hitBoxLive.Width;

            // Clamp movement to platform
            if (nextPosX < leftEdge) nextPosX = leftEdge;
            if (nextPosX > rightEdge) nextPosX = rightEdge;

            pos.X = nextPosX;
        }

        // Handle attack cooldown
        private void AttackIfReady(GameTime gameTime, Moveable target)
        {
            if (currentCD >= normalAttackCD)
            {
                NormalAttack(gameTime, target);
                currentCD = 0.0f;
                color = Color.Red; // for testing
            }
        }

    }
}
