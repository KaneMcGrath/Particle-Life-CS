using KaneUI7;
using KaneUI7.Foundation;
namespace ParticleLife.Game
{
    public static class Konsole
    {
        public static int Width = 250;
        public static int Height = 300;
        public static int Y = 70;
        public static int LineHeight = 20;
        public static List<KonsoleLine> Lines = new List<KonsoleLine>();
        public static float ConsoleTime = 5f;
        public static float ConsoleTimer = 0f;
        public static Dictionary<string, string> TrackedValues = new Dictionary<string, string>();
        public static Dictionary<string, float> TrackedTimers = new Dictionary<string, float>();
        public static int MaxLines;

        private static float TrackedTime = 0.5f;

        public static bool ShowConsole = false;

        public static void Log(string text, RGBA color)
        {
            Lines.Add(new KonsoleLine(text, color));
            ConsoleTimer = Time.RealtimeFromStartup + ConsoleTime;
        }

        public static void Log(string text)
        {
            Log(text, new RGBA(255, 255, 255, 255));
        }

        public static void Error(string text)
        {
            Log(text, new RGBA(255, 0, 0, 255));
        }

        public static void Track(string Key, string Value)
        {
            if (TrackedValues.ContainsKey(Key))
            {
                TrackedValues[Key] = Value;
                TrackedTimers[Key] = Time.RealtimeFromStartup + TrackedTime;
            }
            else
            {
                TrackedValues.Add(Key, Value);
                TrackedTimers.Add(Key, Time.RealtimeFromStartup + TrackedTime);
            }
        }

        public static void Init()
        {
            MaxLines = Height / LineHeight;
        }

        public static void Update()
        {
            ShowConsole = ConsoleTimer > Time.RealtimeFromStartup;
            TrackedUpdate();
            if (ShowConsole)
            {
                KaneFoundation.DrawRect(new Rect(Screen.Width - Width, Y, Width, Height), new RGBA(0, 0, 0, 80));
                if (Konsole.Lines.Count > 0)
                {
                    int min = Lines.Count - MaxLines;
                    if (min < 0)
                    {
                        min = 0;
                    }
                    for (int i = Lines.Count - 1; i >= min; i--)
                    {
                        KaneFoundation.DrawText(new XY(Screen.Width - Width + 10, Y + Height - (LineHeight * (Lines.Count - i))), Lines[i].Text, 16, Lines[i].Color);
                    }
                }
            }
        }


        private static int TrackedItemWidth = 60;
        private static int TrackedItemHeight = 40;
        private static void TrackedUpdate()
        {
            int i = 0;
            foreach (KeyValuePair<string, float> entry in TrackedTimers)
            {
                if (entry.Value < Time.RealtimeFromStartup)
                {
                    TrackedValues.Remove(entry.Key);
                    TrackedTimers.Remove(entry.Key);
                    break;
                }
                KaneBlocks.Box(new Rect(Screen.Width - TrackedItemWidth - (TrackedItemWidth * i), Y - 40, TrackedItemWidth, TrackedItemHeight), new RGBA(0, 0, 0, 80));
                KaneUI.Label(new Rect(Screen.Width - TrackedItemWidth - (TrackedItemWidth * i), Y - 40, TrackedItemWidth, TrackedItemHeight), entry.Key, 18);
                KaneUI.Label(new Rect(Screen.Width - TrackedItemWidth - (TrackedItemWidth * i), Y - 20, TrackedItemWidth, TrackedItemHeight), TrackedValues[entry.Key], 18);
                i++;
            }
        }

        public struct KonsoleLine
        {
            public string Text;
            public RGBA Color;
            public float MsgTime;
            public KonsoleLine(string text, RGBA color)
            {
                Text = text;
                Color = color;
                MsgTime = Time.RealtimeFromStartup;
            }
        }
    }
}
