using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Sim
{
    public static class SimRenderer
    {
        public static float ParticleSize = 5f;
        public static Color[] GroupColors = new Color[0];
        public static bool DrawPixels = false;

        public static void Init(int GroupCount)
        {
            Random random = new Random();
            GroupColors = new Color[GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                GroupColors[i] = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
            }
        }

        public static void Update()
        {
            for (int i = 0; i < SimManager.PX.Length; i++)
            {
                Vector2 screenPos = Surface.MainCamera.WorldToScreen(new Vector2(SimManager.PX[i], SimManager.PY[i]));
                if (!DrawPixels)
                {
                    Raylib.DrawCircleV(screenPos, Surface.MainCamera.Scale(ParticleSize), GroupColors[SimManager.Group[i]]);
                }
                else
                {
                    Raylib.DrawPixelV(screenPos, GroupColors[SimManager.Group[i]]);
                }
            }
            //DrawAttractionMatrix();
        }

        public static void DrawAttractionMatrix()
        {
            for (int i = 0; i < SimManager.GroupCount; i++)
            {
                for (int j = 0; j < SimManager.GroupCount; j++)
                {
                    Color c;
                    if (ParticleDynamics.AttractionFactor[i, j] > 0)
                    {
                        c = new Color(0, (int)(ParticleDynamics.AttractionFactor[i, j] * 255f), 0);
                    }
                    else
                    {
                        c = new Color((int)(-ParticleDynamics.AttractionFactor[i, j] * 255f), 0, 0);
                    }
                    Raylib.DrawRectangleRec(Surface.MainCamera.TransformRectangle(new Rectangle(-500 + i * 20, -500 + j * 20, 20, 20)), c);

                }
            }
        }
    }
}
