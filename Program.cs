using ParticleLife.Game;
using ParticleLife.Sim;
using Raylib_cs;

namespace ParticleLife
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(1280, 720, "Particle Life");
            KaneGameManager.Init();
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                KaneGameManager.Update();
                Raylib.EndDrawing();
                FrameRecorder.Update();
            }
        }
    }
}
