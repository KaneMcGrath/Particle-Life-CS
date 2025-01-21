using KaneUI7;
using KaneUI7.Foundation;
using KaneUI7.Panels;
using ParticleLife.Sim;
using Raylib_cs;

namespace ParticleLife.Game
{
    public static class KaneGameManager
    {

        public static string VersionString = "Particle Life (0.2)";

        public static RGBA ForeGround = new RGBA(44, 44, 44, 255);
        public static RGBA Title = new RGBA(44, 44, 44, 255);

        public static Font DefaultFont = new Font();

        private static Color ClearColor = new Color(21, 21, 21, 21);
        public static bool ClearColorToggle = true;

        public static Window RuntimeOptionsWindow = new Window(RuntimeOptionsWindowUpdate, new Rect(100, 100, 300, 600), "Runtime Options") { };
        public static Window SetupWindow = new Window(SetupWindowUpdate, new Rect(500, 100, 300, 600), "Setup") { };
        public static Window AttractionMatrixDisplay = new Window(AttractionMatrixDisplayUpdate, new Rect(900, 100, 600, 630), "Attraction Matrix") { ShowPanel = false };

        private static int SetupParticleCount = 4000;
        private static int SetupGroupCount = 8;

        private static void AttractionMatrixDisplayUpdate()
        {
            int length = ParticleDynamics.AttractionMatrix.GetLength(0);
            float size = 580 / (length + 1);
            float halfSize = size / 2;
            //Particle Key
            //  Top
            for (int i = 0; i < length; i++)
            {
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 10 + ((1 + i) * size) + halfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 10 + halfSize), halfSize, SimRenderer.GroupColors[i]);
            }
            //  Side
            for (int i = 0; i < length; i++)
            {
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 10 + halfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 10 + ((1 + i) * size) + halfSize), halfSize, SimRenderer.GroupColors[i]);
            }
            //Matrix
            for (int i = 0; i < SimManager.GroupCount; i++)
            {
                for (int j = 0; j < SimManager.GroupCount; j++)
                {
                    Color c;
                    if (ParticleDynamics.AttractionMatrix[i, j] > 0)
                    {
                        c = new Color(0, (int)(ParticleDynamics.AttractionMatrix[i, j] * 255f), 0);
                    }
                    else
                    {
                        c = new Color((int)(-ParticleDynamics.AttractionMatrix[i, j] * 255f), 0, 0);
                    }
                    Raylib.DrawRectangle((int)(AttractionMatrixDisplay.ContentRect.X + 10 + size + (size * i)), (int)(AttractionMatrixDisplay.ContentRect.Y + 10 + size + (size * j)), (int)size, (int)size, c);

                }
            }
        }


        private static void SetupWindowUpdate()
        {
            KaneUI.Label(SetupWindow.IndexToRect(0), "Particle Count   Current [" + SimManager.VX.Length + "]");
            SetupParticleCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(1, 4, 1, 3), SetupParticleCount, 1, 100000);
            KaneUI.Label(SetupWindow.IndexToRect(1, 4, 0), SetupParticleCount.ToString());
            KaneUI.Label(SetupWindow.IndexToRect(3), "Group Count");
            SetupGroupCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(4), SetupGroupCount, 1, 100);


            if (KaneUI.Button(SetupWindow.IndexToRect(5), "New Board"))
            {
                SimManager.Init(SetupParticleCount, SetupGroupCount);
            }
        }

        private static void RuntimeOptionsWindowUpdate()
        {
            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(0), "Delta Time");
            SimManager.dt = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(1), SimManager.dt, 0.0001f, 0.01f);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(2), "Max Radius");
            ParticleDynamics.MaxRadius = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(3), ParticleDynamics.MaxRadius, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(4), "Force Multiplier");
            ParticleDynamics.ForceMultiplier = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(5), ParticleDynamics.ForceMultiplier, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(6), "Friction");
            SimManager.Friction = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(7), SimManager.Friction, 0, 1);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(8), "Particle Size");
            SimRenderer.ParticleSize = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(9), SimRenderer.ParticleSize, 1, 100);

            SimRenderer.DrawPixels = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(10), SimRenderer.DrawPixels, "Draw Pixels");

            ClearColorToggle = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(12), ClearColorToggle, "Clear Color");
            if (KaneUI.Button(RuntimeOptionsWindow.IndexToRect(13), "Show Attraction Matrix"))
            {
                AttractionMatrixDisplay.ShowPanel = true;
            }
            //FrameRecorder.Recording = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(13), FrameRecorder.Recording, "Record Frames");
            if (KaneUI.Button(RuntimeOptionsWindow.IndexToRect(14), "Fullscreen"))
            {
                Raylib.SetWindowSize(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0));
                Raylib.ToggleFullscreen();
            }
        }

        public static void Init()
        {
            DefaultFont = Raylib.LoadFontEx(Environment.CurrentDirectory + "\\Fonts\\OpenSans_SemiCondensed-Bold.ttf", 64, null, 0);

            Raylib.SetWindowState(ConfigFlags.ResizableWindow);
            Surface.Init();
            InputManager.Init();
            Konsole.Init();
            Konsole.Log(VersionString);

            SimManager.Init(1000, 8);

        }

        public static void Update()
        {
            InputManager.Update();
            Time.Update();
            if (ClearColorToggle)
            {
                Raylib.ClearBackground(ClearColor);
            }

            Surface.Update();
            SimManager.Update();
            SimRenderer.Update();

            if (!RuntimeOptionsWindow.ShowPanel)
            {
                if (KaneUI.Button(new Rect(0, 0, 200, 30), "Runtime Options"))
                {
                    RuntimeOptionsWindow.minimize = false;
                    RuntimeOptionsWindow.ShowPanel = true;
                    PanelManager.BringToFront(RuntimeOptionsWindow);
                }
            }
            if (!SetupWindow.ShowPanel)
            {
                if (KaneUI.Button(new Rect(200, 0, 200, 30), "Setup"))
                {
                    SetupWindow.minimize = false;
                    SetupWindow.ShowPanel = true;
                    PanelManager.BringToFront(SetupWindow);
                }
            }

            Konsole.Update();
            PanelManager.UpdatePanels();
            PopNotification.Update();
            KaneUI.Label(new Rect(10, Screen.Height - 40, 200, 20), "FPS: " + Raylib.GetFPS());
            KaneUI.Label(new Rect(10, Screen.Height - 20, 200, 20), "FrameTime: " + Raylib.GetFrameTime());

        }
    }
}
