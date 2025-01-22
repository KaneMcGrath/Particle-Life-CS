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

        public static Color HsvToRgb(byte hue, byte saturation, byte value)
        {
            // Convert input values to float for calculations
            float h = hue / 255f * 360f;  // Hue in degrees (0-360)
            float s = saturation / 255f;  // Saturation (0-1)
            float v = value / 255f;       // Value/Brightness (0-1)

            int hi = (int)(h / 60f) % 6;
            float f = (h / 60f) - hi;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);

            float r = 0, g = 0, b = 0;

            switch (hi)
            {
                case 0:
                    r = v; g = t; b = p;
                    break;
                case 1:
                    r = q; g = v; b = p;
                    break;
                case 2:
                    r = p; g = v; b = t;
                    break;
                case 3:
                    r = p; g = q; b = v;
                    break;
                case 4:
                    r = t; g = p; b = v;
                    break;
                case 5:
                    r = v; g = p; b = q;
                    break;
            }

            // Convert normalized float (0-1) to byte (0-255) and return Color
            return new Color(
                (byte)(r * 255),
                (byte)(g * 255),
                (byte)(b * 255)
            );
        }


    }
}
