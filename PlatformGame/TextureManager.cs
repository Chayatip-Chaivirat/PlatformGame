using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformGame
{
    internal class TextureManager
    {
        public static Texture2D grassTex;
        public static Texture2D allLinkTex;
        public static Texture2D wallTex;

        public static void Textures (ContentManager content)
        {
            grassTex = content.Load<Texture2D>("grass");
            allLinkTex = content.Load<Texture2D>("Link_all");
            wallTex = content.Load<Texture2D>("wall");
        }
    }
}
