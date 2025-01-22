using KaneUI7;
using ParticleLife.Game;
using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Sim
{
    public static class Surface
    {
        private static Color CheckerboardColor = new Color(7, 7, 7, 255);
        //private static Color CheckerboardColor2 = new Color(224, 146, 132, 255);
        private static Color CheckerboardColor2 = new Color(22, 22, 22, 255);
        private static Texture2D Checkerboard;
        private static int CheckerTextureScale = 128;
        private static int CheckerScale;
        public static Camera MainCamera = new Camera() { Position = new Vector2(2500, 2500), Zoom = 0.2f, MaxX = 6000, MinX = -1000, MaxY = 6000, MinY = -1000 };

        public static void Init()
        {
            Checkerboard = Raylib.LoadTextureFromImage(Raylib.GenImageChecked(CheckerTextureScale, CheckerTextureScale, 1, 1, CheckerboardColor, CheckerboardColor2));
            CheckerScale = CheckerTextureScale * CheckerTextureScale;
        }
        private static bool DragBackground = false;
        public static void Update()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (PanelManager.IsMouseInAnyPanel())
                {
                    DragBackground = false;
                }
                else
                {
                    DragBackground = true;
                }
            }
            if (!KaneUI.IsDraggingSlider && DragBackground)
            {
                MainCamera.CameraControls();
            }
            FollowParticleUpdate();
            //DrawBackgroundCheckerboard();
        }
        public static int FollowedParticle = 0;
        public static bool FollowParticle(int index)
        {
            if (SimManager.VX.Count() > index)
            {
                FollowedParticle = index;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void FollowParticleUpdate()
        {
            if (KaneGameManager.FollowParticle)
            {
                if (SimManager.VX.Count() > FollowedParticle)
                {
                    MainCamera.Position = new Vector2(SimManager.PX[FollowedParticle], SimManager.PY[FollowedParticle]);
                }
            }
        }

        public static int TextureDrawCount = 0;

        private static void DrawBackgroundCheckerboard()
        {
            TextureDrawCount = 0;
            Vector2 TopLeft = MainCamera.ScreenToWorld(new Vector2(0, 0));
            Vector2 BottomRight = MainCamera.ScreenToWorld(new Vector2(Screen.Width, Screen.Height));
            int Left = (int)(TopLeft.X / CheckerScale) - 1;
            int Top = (int)(TopLeft.Y / CheckerScale) - 1;
            int Right = (int)(BottomRight.X / CheckerScale) + 1;
            int Bottom = (int)(BottomRight.Y / CheckerScale) + 1;
            for (int x = Left; x < Right; x++)
            {
                for (int y = Top; y < Bottom; y++)
                {
                    DrawCell(x, y);
                }
            }
        }

        private static void DrawCell(int x, int y)
        {
            TextureDrawCount++;
            Vector2 CameraSpace = MainCamera.WorldToScreen(new Vector2(CheckerScale * x, CheckerScale * y));
            Raylib.DrawTextureEx(Checkerboard, CameraSpace, 0, MainCamera.Scale(CheckerTextureScale), Color.White);
        }
    }
}
