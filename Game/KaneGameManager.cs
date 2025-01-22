using KaneUI7;
using KaneUI7.Foundation;
using KaneUI7.Panels;
using ParticleLife.Sim;
using Raylib_cs;

namespace ParticleLife.Game
{
    public static class KaneGameManager
    {

        public static string VersionString = "Particle Life (0.3)";

        public static RGBA ForeGround = new RGBA(44, 44, 44, 255);
        public static RGBA Title = new RGBA(44, 44, 44, 255);

        public static Font DefaultFont = new Font();

        private static Color ClearColor = new Color(21, 21, 21, 21);
        public static bool ClearColorToggle = true;

        public static Constraints DefaultConstraints = new Constraints(0, 0, 30, 0);

        public static Window RuntimeOptionsWindow = new Window(RuntimeOptionsWindowUpdate, new Rect(10, 100, 300, 600), "Runtime Options", new RGBA("41555b")) { constraints = DefaultConstraints };
        public static Window SetupWindow = new Window(SetupWindowUpdate, new Rect(50, 100, 300, 600), "Setup", new RGBA("646d60")) { constraints = DefaultConstraints };
        public static Window AttractionMatrixDisplay = new Window(AttractionMatrixDisplayUpdate, new Rect(90, 100, 500, 650), "Attraction Matrix", new RGBA("59355a")) { constraints = DefaultConstraints };
        public static Window GraphicsOptionsWindow = new Window(GraphicsOptionsWindowUpdate, new Rect(130, 100, 300, 600), "Graphics Options", new RGBA("c5655c")) { constraints = DefaultConstraints };

        private static int SetupParticleCount = 5000;
        private static int SetupGroupCount = 8;
        public static bool FollowParticle = false;
        private static string FollowParticleIndex = "0";
        private static void GraphicsOptionsWindowUpdate()
        {
            if (KaneUI.Button(GraphicsOptionsWindow.IndexToRect(0).Shrink(2), "Fullscreen"))
            {
                Raylib.SetWindowSize(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0));
                Raylib.ToggleFullscreen();
            }
            KaneUI.Label(GraphicsOptionsWindow.IndexToRect(1, 6, 0, 2), "Particle Size");
            SimRenderer.ParticleSize = KaneUI.Slider(GraphicsOptionsWindow.IndexToRect(1, 6, 2, 4).Shrink(2), SimRenderer.ParticleSize, 1, 100);
            FollowParticle = KaneUI.CheckBox(GraphicsOptionsWindow.IndexToRect(2).Shrink(2), FollowParticle, "Follow Particle");
            KaneUI.Label(GraphicsOptionsWindow.IndexToRect(3, 6, 0, 2), "Index : ");
            FollowParticleIndex = KaneUI.TextField(GraphicsOptionsWindow.IndexToRect(3, 6, 2, 2).Shrink(2), FollowParticleIndex);
            if (KaneUI.Button(GraphicsOptionsWindow.IndexToRect(3, 6, 4, 2).Shrink(2), "Follow"))
            {
                if (int.TryParse(FollowParticleIndex, out int index))
                {
                    FollowParticle = Surface.FollowParticle(index);
                }
                else
                {
                    PopNotification.Message(GraphicsOptionsWindow.IndexToRect(3).GetPosition(), "Could Not Parse Textbox", new RGBA(255, 0, 0));
                }
            }
            SimRenderer.UseWrapping = KaneUI.CheckBox(GraphicsOptionsWindow.IndexToRect(4).Shrink(2), SimRenderer.UseWrapping, "Use Wrapping");
            SimRenderer.ShowSimBoundry = KaneUI.CheckBox(GraphicsOptionsWindow.IndexToRect(5).Shrink(2), SimRenderer.ShowSimBoundry, "Show Sim Boundry");
        }
        private static int SelectedMatrixIX = 0;
        private static int SelectedMatrixIY = 0;
        private static void AttractionMatrixDisplayUpdate()
        {
            int length = ParticleDynamics.AttractionMatrix.GetLength(0);
            float size = 480 / (length + 1);
            float halfSize = size / 2;
            float fixedSize = 40;
            float fixedHalfSize = 20;
            //Particle Key
            //  Top
            for (int i = 0; i < length; i++)
            {
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 5 + ((1 + i) * size) + halfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 5 + halfSize), halfSize, SimRenderer.GroupColors[i]);
            }
            //  Side
            for (int i = 0; i < length; i++)
            {
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 5 + halfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 5 + ((1 + i) * size) + halfSize), halfSize, SimRenderer.GroupColors[i]);
            }
            //Matrix
            for (int i = 0; i < SimManager.GroupCount; i++)
            {
                for (int j = 0; j < SimManager.GroupCount; j++)
                {
                    RGBA cl;
                    if (ParticleDynamics.AttractionMatrix[i, j] > 0)
                    {
                        cl = new RGBA(0, (byte)(ParticleDynamics.AttractionMatrix[i, j] * 255f), 0);
                    }
                    else
                    {
                        cl = new RGBA((byte)(-ParticleDynamics.AttractionMatrix[i, j] * 255f), 0, 0);
                    }
                    if (KaneUI.Button(new Rect(AttractionMatrixDisplay.ContentRect.X + 5 + (int)size + ((int)size * i), AttractionMatrixDisplay.ContentRect.Y + 5 + (int)size + ((int)size * j), (int)size, (int)size), "", cl))
                    {
                        SelectedMatrixIX = i;
                        SelectedMatrixIY = j;
                    }


                }
            }
            //Selected
            if (SelectedMatrixIX >= 0 && SelectedMatrixIY >= 0 && SelectedMatrixIX < ParticleDynamics.AttractionMatrix.GetLength(0) && SelectedMatrixIY < ParticleDynamics.AttractionMatrix.GetLength(1))
            {
                //left 
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 10 + fixedHalfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 500 + fixedSize + fixedHalfSize), fixedHalfSize, SimRenderer.GroupColors[SelectedMatrixIY]);
                //Top
                Raylib.DrawCircle((int)(AttractionMatrixDisplay.ContentRect.X + 10 + fixedSize + fixedHalfSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 500 + fixedHalfSize), fixedHalfSize, SimRenderer.GroupColors[SelectedMatrixIX]);
                RGBA cl;
                if (ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY] > 0)
                {
                    cl = new RGBA(0, (byte)(ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY] * 255f), 0);
                }
                else
                {
                    cl = new RGBA((byte)(-ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY] * 255f), 0, 0);
                }
                KaneBlocks.Box(new Rect((int)(AttractionMatrixDisplay.ContentRect.X + 10 + fixedSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 500 + fixedSize), (int)fixedSize, (int)fixedSize), cl);
                ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY] = KaneUI.Slider(new Rect((int)(AttractionMatrixDisplay.ContentRect.X + 10 + fixedSize + fixedSize), (int)(AttractionMatrixDisplay.ContentRect.Y + 500 + fixedSize), 400, (int)fixedSize).Shrink(4), ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY], -1, 1);
                if (KaneUI.Button(new Rect((int)(AttractionMatrixDisplay.ContentRect.X + 10 + fixedSize + fixedSize + 20f), (int)(AttractionMatrixDisplay.ContentRect.Y + 500 + fixedSize + fixedSize), 30, 30), "0"))
                {
                    ParticleDynamics.AttractionMatrix[SelectedMatrixIX, SelectedMatrixIY] = 0;
                }
            }
        }

        private static ParticleDispersionOptions ParticleDispersionOption = ParticleDispersionOptions.Random;
        private static void SetupWindowUpdate()
        {
            KaneUI.Label(SetupWindow.IndexToRect(0, 6, 0, 2), "Particle Count");
            SetupParticleCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(0, 6, 2, 4).Shrink(2), SetupParticleCount, 1, 100000);
            KaneUI.Label(SetupWindow.IndexToRect(1, 6, 0, 2), "Group Count");
            SetupGroupCount = (int)KaneUI.Slider(SetupWindow.IndexToRect(1, 6, 2, 4).Shrink(2), SetupGroupCount, 1, 100);
            KaneUI.Label(SetupWindow.IndexToRect(2, 3, 0), "Distribution");
            ParticleDispersionOption = KaneUI.EnumSelector<ParticleDispersionOptions>(SetupWindow.IndexToRect(2, 3, 1, 2).Shrink(2), ParticleDispersionOption, 100);

            if (KaneUI.Button(SetupWindow.IndexToRect(16).Shrink(2), "Randomize All"))
            {
                SimManager.Init(SetupParticleCount, SetupGroupCount, ParticleDispersionOption);
                Raylib.ClearBackground(ClearColor);
            }
        }

        private static void RuntimeOptionsWindowUpdate()
        {
            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(0, 6, 0, 2), "Delta Time");
            SimManager.dt = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(0, 6, 2, 4).Shrink(2), SimManager.dt, 0.00001f, 0.004f);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(1, 6, 0, 2), "Max Radius");
            ParticleDynamics.MaxRadius = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(1, 6, 2, 4).Shrink(2), ParticleDynamics.MaxRadius, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(2, 6, 0, 2), "Force Multiplier");
            ParticleDynamics.ForceMultiplier = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(2, 6, 2, 4).Shrink(2), ParticleDynamics.ForceMultiplier, 1, 1000);

            KaneUI.Label(RuntimeOptionsWindow.IndexToRect(3, 6, 0, 2), "Friction");
            SimManager.Friction = KaneUI.Slider(RuntimeOptionsWindow.IndexToRect(3, 6, 2, 4).Shrink(2), SimManager.Friction, 0, 1);



            //SimRenderer.DrawPixels = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(10), SimRenderer.DrawPixels, "Draw Pixels");

            ClearColorToggle = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(5).Shrink(2), ClearColorToggle, "Clear Color");
            if (KaneUI.Button(RuntimeOptionsWindow.IndexToRect(6).Shrink(2), "Show Attraction Matrix"))
            {
                AttractionMatrixDisplay.ShowPanel = true;
            }
            //FrameRecorder.Recording = KaneUI.CheckBox(RuntimeOptionsWindow.IndexToRect(13), FrameRecorder.Recording, "Record Frames");
            if (KaneUI.Button(RuntimeOptionsWindow.IndexToRect(7).Shrink(2), "Randomize Positions"))
            {
                SimManager.RandomizeParticles(SimManager.VX.Length, ParticleDynamics.AttractionMatrix.GetLength(0));
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

            SimManager.Init(5000, 8);

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
            if (!GraphicsOptionsWindow.ShowPanel)
            {
                if (KaneUI.Button(new Rect(400, 0, 200, 30), "Graphics Options"))
                {
                    GraphicsOptionsWindow.minimize = false;
                    GraphicsOptionsWindow.ShowPanel = true;
                    PanelManager.BringToFront(GraphicsOptionsWindow);
                }
            }
            Konsole.Update();
            PanelManager.UpdatePanels();
            PopNotification.Update();
            KaneUI.Label(new Rect(10, Screen.Height - 40, 200, 20), "FPS: " + Raylib.GetFPS());
            KaneUI.Label(new Rect(10, Screen.Height - 20, 200, 20), "FrameTime: " + Raylib.GetFrameTime());
            KaneUI.Label(new Rect(Screen.Width - 320, Screen.Height - 20, 300, 20), "Press ESC to exit", 24, 5);

        }
    }

    public enum ParticleDispersionOptions
    {
        Random,
        Distributed,
        Pie,
        Grid,
        Line,
        Circle
    }
}
