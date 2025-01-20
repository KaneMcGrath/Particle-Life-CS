using KaneUI7;
using KaneUI7.Foundation;
using ParticleLife.Sim;
using Raylib_cs;

namespace ParticleLife.Game
{
    public static class KaneGameManager
    {

        public static string VersionString = "Particle Life (0.1)";

        public static RGBA ForeGround = new RGBA(44, 44, 44, 255);
        public static RGBA Title = new RGBA(44, 44, 44, 255);

        public static Font DefaultFont = new Font();

        private static Color ClearColor = new Color(21, 21, 21, 21);

        public static void Init()
        {
            DefaultFont = Raylib.LoadFontEx(Environment.CurrentDirectory + "\\Fonts\\OpenSans_SemiCondensed-Bold.ttf", 64, null, 0);

            Raylib.SetWindowState(ConfigFlags.ResizableWindow);
            Surface.Init();
            InputManager.Init();
            Konsole.Init();
            Konsole.Log(VersionString);

            SimManager.SetupSim(1000, 8);

        }

        public static void Update()
        {
            InputManager.Update();
            Time.Update();
            Raylib.ClearBackground(ClearColor);

            Surface.Update();
            SimManager.Update();

            Konsole.Update();
            PanelManager.UpdatePanels();
            PopNotification.Update();
        }
    }
}
