using Raylib_cs;

namespace ParticleLife.Game
{
    public static class Time
    {
        public static float DeltaTime = 0f;
        public static float RealtimeFromStartup = 0f;

        public static void Update()
        {
            DeltaTime = Raylib.GetFrameTime();
            RealtimeFromStartup = (float)Raylib.GetTime();
        }

        /// <summary>
        /// When called multiple times only returns true after a fixed amount of time after it was first called
        /// then is reset
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static bool Timer(float waitTime, ref float timer)
        {
            if (timer <= RealtimeFromStartup)
            {
                timer = RealtimeFromStartup + waitTime;
                return true;
            }
            return false;
        }
    }
}
