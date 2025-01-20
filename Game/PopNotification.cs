using KaneUI7;
using KaneUI7.Foundation;

namespace ParticleLife.Game
{
    public static class PopNotification
    {
        public static List<PopMessage> PopMessages = new List<PopMessage>();
        private static Stack<PopMessage> DequeueStack = new Stack<PopMessage>();
        private static bool NeedsDequeue = false;
        public static void MouseMessage(string message)
        {
            PopMessages.Add(new PopMessage(new XY(KaneFoundation.GetMouseX(), KaneFoundation.GetMouseY() - 15), message));
        }

        public static void MouseMessage(string message, RGBA Color)
        {
            PopMessages.Add(new PopMessage(new XY(KaneFoundation.GetMouseX(), KaneFoundation.GetMouseY() - 15), message, Color));
        }

        public static void Update()
        {
            if (PopMessages.Count > 0)
            {
                for (int i = 0; i < PopMessages.Count; i++)
                {
                    PopMessage message = PopMessages[i];
                    message.Update();

                    if (message.t >= message.Lifetime)
                    {
                        NeedsDequeue = true;
                        DequeueStack.Push(message);
                    }
                    KaneUI.Label(new Rect(message.Pos, 100, 30), message.Message, message.Color, message.Size);

                }
                if (NeedsDequeue)
                {
                    while (DequeueStack.Count > 0)
                    {
                        PopMessages.Remove(DequeueStack.Pop());
                    }
                }
            }
        }
    }

    public class PopMessage
    {
        public string Message;
        public RGBA Color;
        public int Size;
        public XY Pos;
        public XY InitPos;
        public float Lifetime;
        public float t = 0f;
        public PopMessage(XY Pos, string message, RGBA color, int size, float Lifetime)
        {
            this.Message = message;
            this.Color = color;
            this.Size = size;
            this.Pos = Pos;
            InitPos = Pos;
            this.Lifetime = Lifetime;
        }
        public PopMessage(XY Pos, string message, RGBA color, int size)
        {
            this.Message = message;
            this.Color = color;
            this.Size = size;
            this.Pos = Pos;
            InitPos = Pos;
            this.Lifetime = 4f;
        }
        public PopMessage(XY Pos, string message, RGBA color)
        {
            this.Message = message;
            this.Color = color;
            this.Size = 20;
            this.Pos = Pos;
            InitPos = Pos;
            this.Lifetime = 4f;
        }
        public PopMessage(XY Pos, string message)
        {
            this.Message = message;
            this.Color = DefaultColors.White;
            this.Size = 20;
            this.Pos = Pos;
            InitPos = Pos;
            this.Lifetime = 4f;
        }

        public void Update()
        {
            if (t < Lifetime)
            {
                t += Time.DeltaTime;
                float progress = t / Lifetime;
                //KaneUI.Label(new Rect(Pos.X, Pos.Y + 30, 60, 30), "P:" + progress, DefaultColors.Lime);
                XY targetPos = new XY(InitPos.X - 100, InitPos.Y - 200);

                Pos = new XY(InitPos.X + (int)((targetPos.X - InitPos.X) * MathF.Sqrt(progress)), (int)(InitPos.Y + ((targetPos.Y - InitPos.Y) * progress * progress)));
            }
        }


    }
}
