using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Sim
{
    public static class SimRenderer
    {
        public static float ParticleSize = 5f;
        public static Color[] GroupColors = new Color[0];
        public static bool DrawPixels = false;
        public static bool AutoSwitch = true;

        public static void Init(int GroupCount)
        {
            Random random = new Random();
            GroupColors = new Color[GroupCount];

            int byteSteps = 255 / GroupCount;

            for (int i = 0; i < GroupCount; i++)
            {
                GroupColors[i] = HsvToRgb((byte)(i * byteSteps), 255, 255);
            }

            //for (int i = 0; i < GroupCount; i++)
            //{
            //    GroupColors[i] = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
            //}
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

        public static void Update()
        {
            for (int i = 0; i < SimManager.PX.Length; i++)
            {
                Vector2 screenPos = Surface.MainCamera.WorldToScreen(new Vector2(SimManager.PX[i], SimManager.PY[i]));
                if (screenPos.X < 0 || screenPos.X > Raylib.GetScreenWidth() || screenPos.Y < 0 || screenPos.Y > Raylib.GetScreenHeight())
                {
                    continue;
                }
                if (AutoSwitch)
                {
                    if (Surface.MainCamera.Scale(ParticleSize) < 1)
                    {
                        DrawPixels = true;
                    }
                    else
                    {
                        DrawPixels = false;
                    }
                }
                if (DrawPixels)
                {
                    Raylib.DrawPixelV(screenPos, GroupColors[SimManager.Group[i]]);
                }
                else
                {
                    Raylib.DrawCircleV(screenPos, Surface.MainCamera.Scale(ParticleSize), GroupColors[SimManager.Group[i]]);
                }
            }
            //DrawAttractionMatrix();
        }

        public static void DrawAttractionMatrix()
        {

        }
    }
}
