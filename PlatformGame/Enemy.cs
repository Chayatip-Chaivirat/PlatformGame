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
        Platform currentPlatform;

        public List<Platform> platformList;

        private Platform myPlatform;
        private float leftBound;
        private float rightBound;


        public Enemy(Texture2D tex, Vector2 pos, int totalFrame, Vector2 frameSize, int recX, int recY, Player player) : base(totalFrame, frameSize, recX, recY)
        {
            this.tex = tex;
            this.pos = pos;
            hitBoxLive = new Rectangle((int)pos.X, (int)pos.Y, (int)frameSize.X, (int)frameSize.Y);
            objectMoving = false;
            scale = 1;
            velocity = new Vector2(200, 100);
            color = Color.White;
            maxHP = 4;
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

        //Edge detection
        private bool IsAtLeftEdge()
        {
            if (currentPlatform == null) return true;

            float leftEdge = currentPlatform.hitBoxLive.Left + 1; 
            return pos.X <= leftEdge;
        }

        private bool IsAtRightEdge()
        {
            if (currentPlatform == null) return true;

            float rightEdge = currentPlatform.hitBoxLive.Right - hitBoxLive.Width - 1; 
            return pos.X >= rightEdge;
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
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movement logic
            if (!attacking) color = Color.White;

            if (DetectedLeft(player))
            {
                TurnLeft(gameTime);
                if (!IsAtLeftEdge())
                    pos.X += velocity.X * dt;
                AttackIfReady(gameTime, player);
            }
            else if (DetectedRight(player))
            {
                TurnRight(gameTime);
                if (!IsAtRightEdge())
                    pos.X += velocity.X * dt;
                AttackIfReady(gameTime, player);
            }
            else
            {
                if (movementCode == 1) // Move right
                {
                    TurnRight(gameTime);
                    if (!IsAtRightEdge())
                        pos.X += velocity.X * dt;
                    else
                        movementCode = 2; // switch direction
                }
                else if (movementCode == 2) // Move left
                {
                    TurnLeft(gameTime);
                    if (!IsAtLeftEdge())
                        pos.X += velocity.X * dt;
                    else
                        movementCode = 1; // switch direction
                }
            }

            // Update hitbox
            hitBoxLive.Location = pos.ToPoint();

            UpdateDetectionRanges();

            base.Update(gameTime);
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
        private void UpdateDetectionRanges()
        {
            detectionRangeLeft.X = (int)pos.X - detectionRangeWidth;
            detectionRangeLeft.Y = (int)pos.Y;
            detectionRangeRight.X = (int)pos.X + tex.Width;
            detectionRangeRight.Y = (int)pos.Y;
        }
    }
}
