using ParticleLife.Game;
using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Sim
{
    public static class SimRenderer
    {
        public static float ParticleSize = 2f;
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
                GroupColors[i] = Tools.HsvToRgb((byte)(i * byteSteps), 255, 255);
            }
        }

        public static bool UseWrapping = false;

        public static void Update()
        {
            bool doScreenCheck = true;
            Vector2 LeftCornerScreen = Surface.MainCamera.WorldToScreen(new Vector2(0, 0));
            Vector2 RightCornerScreen = Surface.MainCamera.WorldToScreen(new Vector2(SimManager.Bounds[2], SimManager.Bounds[3]));

            float simWidth = SimManager.Bounds[2];
            float simHeight = SimManager.Bounds[3];

            for (int i = 0; i < SimManager.PX.Length; i++)
            {
                Vector2 particlePos = new Vector2(SimManager.PX[i], SimManager.PY[i]);

                // If wrapping is enabled, include adjacent regions, otherwise only render the original position
                Vector2[] wrappedPositions = UseWrapping
                    ? new Vector2[]
                    {
                particlePos,                                   // Original position
                new Vector2(particlePos.X - simWidth, particlePos.Y),  // Wrapped to the left
                new Vector2(particlePos.X + simWidth, particlePos.Y),  // Wrapped to the right
                new Vector2(particlePos.X, particlePos.Y - simHeight), // Wrapped to the top
                new Vector2(particlePos.X, particlePos.Y + simHeight), // Wrapped to the bottom
                new Vector2(particlePos.X - simWidth, particlePos.Y - simHeight), // Top-left corner
                new Vector2(particlePos.X + simWidth, particlePos.Y - simHeight), // Top-right corner
                new Vector2(particlePos.X - simWidth, particlePos.Y + simHeight), // Bottom-left corner
                new Vector2(particlePos.X + simWidth, particlePos.Y + simHeight)  // Bottom-right corner
                    }
                    : new Vector2[] { particlePos }; // Only the original position if wrapping is disabled

                foreach (var wrappedPos in wrappedPositions)
                {
                    Vector2 screenPos = Surface.MainCamera.WorldToScreen(wrappedPos);

                    // Skip if the wrapped position is outside the screen bounds
                    if (screenPos.X < 0 || screenPos.Y < 0 || screenPos.X > Raylib.GetScreenWidth() || screenPos.Y > Raylib.GetScreenHeight())
                    {
                        continue;
                    }

                    // Adjust rendering mode (pixel or circle) based on the scale
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

                    // Render the particle
                    if (DrawPixels)
                    {
                        Raylib.DrawPixelV(screenPos, GroupColors[SimManager.Group[i]]);
                    }
                    else
                    {
                        Raylib.DrawCircleV(screenPos, Surface.MainCamera.Scale(ParticleSize), GroupColors[SimManager.Group[i]]);
                    }
                }
            }
        }
    }
}
