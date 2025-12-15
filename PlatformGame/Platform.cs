using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace PlatformGame
{
    internal class Platform : CollidableObject
    {
        public Platform(Rectangle hitbox)
        {
            this.tex = TextureManager.wallTex;
            color = Color.White;
            this.hitBoxLive = hitbox;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, hitBoxLive, color);
        }
    }
}
