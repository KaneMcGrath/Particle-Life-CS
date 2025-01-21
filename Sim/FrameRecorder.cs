using Raylib_cs;

namespace ParticleLife.Sim
{
    public static class FrameRecorder
    {
        public static bool Recording = false;
        private static int FrameCount = 0;

        public static void Update()
        {
            if (Recording)
            {
                Image frame = Raylib.LoadImageFromScreen();


                FrameCount++;
            }
        }

        public static void StartRecording()
        {
            Recording = true;
        }
    }
}
