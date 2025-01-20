using Raylib_cs;

namespace ParticleLife.Game
{
    public static class Tools
    {
        public static Random random = new Random();

        public static int ScreenCenterX()
        {
            return Raylib.GetScreenWidth() / 2;
        }
        public static int ScreenCenterY()
        {
            return Raylib.GetScreenHeight() / 2;
        }

        /// <summary>
        /// has a percent chance off returning true
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool Chance(float percent)
        {
            return (random.Next(0, 100) < percent);
        }

        /// <summary>
        /// When called multiple times only returns true after a fixed amount of time after it was first called
        /// then is reset
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public static bool timer(ref float timer, float waitTime)
        {
            if (timer <= Raylib.GetTime())
            {
                timer = (float)Raylib.GetTime() + waitTime;
                return true;
            }
            return false;
        }

        public static Color RandomColor()
        {
            return new Color(random.Next(0, 255), random.Next(0, 255), random.Next(255, 255), 255);
        }


    }
}
