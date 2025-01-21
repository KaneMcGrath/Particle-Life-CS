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

        public static Window RuntimeOptionsWindow = new Window(RuntimeOptionsWindowUpdate, new Rect(100, 100, 300, 600), "Runtime Options") { ShowPanel = true };
        public static Window SetupWindow = new Window(SetupWindowUpdate, new Rect(500, 100, 300, 600), "Setup") { ShowPanel = true };

        private static int SetupParticleCount = 1000;
        private static int SetupGroupCount = 8;

        private static void SetupWindowUpdate()
        {
            KaneUI.Label(SetupWindow.IndexToRect(0), "Particle Count");
            SetupParticleCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(1), SetupParticleCount, 1, 3000);
            if (KaneUI.Button(SetupWindow.IndexToRect(2, 3, 2), "1000"))
            {
                SetupParticleCount = 1000;
            }
            KaneUI.Label(SetupWindow.IndexToRect(3), "Group Count");
            SetupGroupCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(4), SetupGroupCount, 1, 100);


            if (KaneUI.Button(SetupWindow.IndexToRect(5), "Reset"))
            {
                SimManager.Init(SetupParticleCount, SetupGroupCount);
            }
        }

        private static void RuntimeOptionsWindowUpdate()
        {
            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(0), "Delta Time");
            SimManager.dt = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(1), SimManager.dt, 0.0001f, 0.01f);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(3), "Max Radius");
            ParticleDynamics.MaxRadius = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(4), ParticleDynamics.MaxRadius, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(6), "Force Multiplier");
            ParticleDynamics.ForceMultiplier = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(7), ParticleDynamics.ForceMultiplier, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(9), "Friction");
            SimManager.Friction = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(10), SimManager.Friction, 0, 1);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(12), "Particle Size");
            SimRenderer.ParticleSize = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(13), SimRenderer.ParticleSize, 1, 100);
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
            Raylib.ClearBackground(ClearColor);

            Surface.Update();
            SimManager.Update();
            SimRenderer.Update();

            if (!RuntimeOptionsWindow.ShowPanel)
            {
                RuntimeOptionsWindow.ShowPanel = true;
            }
            if (!SetupWindow.ShowPanel)
            {
                SetupWindow.ShowPanel = true;
            }

            Konsole.Update();
            PanelManager.UpdatePanels();
            PopNotification.Update();
            KaneUI.Label(new Rect(10, 10, 200, 20), "FPS: " + Raylib.GetFPS());
            KaneUI.Label(new Rect(10, 30, 200, 20), "FrameTime: " + Raylib.GetFrameTime());

        }
    }
}
