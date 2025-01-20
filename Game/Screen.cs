using Raylib_cs;

namespace ParticleLife.Game
{
    public static class Screen
    {
        public static int Width
        {
            get { return Raylib.GetScreenWidth(); }
        }

        public static int Height
        {
            get { return Raylib.GetScreenHeight(); }
        }

        public static int CenterX
        {
            get { return Raylib.GetScreenWidth() / 2; }
        }

        public static int CenterY
        {
            get { return Raylib.GetScreenHeight() / 2; }
        }
    }
}
