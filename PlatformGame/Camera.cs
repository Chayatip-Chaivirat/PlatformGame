using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformGame
{
    internal class Camera
    {
        private Matrix transform; //Håller en transformation från position i spelvärden till position i fönstret.
        private Vector2 position; //Spelarens position
        private Viewport view; //Kamera vyn (det av världen man ser)

        public Matrix Transform //Property för transform som används i Game1
        {
            get { return transform; }
        }

        public Camera(Viewport view) //Konstruktor för att skapa en kamera.
        {
            this.view = view;
        }

        public void SetPosition(Vector2 position) //Flyttar kameran och räknar om spelarens position till "fönsterposition", dvs var på fönstret objektet skall visas.
        {
            this.position = position;
            transform = Matrix.CreateTranslation(-position.X + view.Width / 2, -position.Y + view.Height / 2, 0);
            //Methoden CreateTranslation är inbyggd i Monogame. Den har (x-,y-, och z-led) och gör om dessa till en matris som representerar positionen i för´nstret i detta fall.
        }
    }
}
