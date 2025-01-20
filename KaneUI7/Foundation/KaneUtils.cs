namespace KaneUI7.Foundation
{
    public static class KaneUtils
    {
        /// <summary>
        /// Returns true if the mouse pointer is currently inside the rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool IsMouseInRect(Rect rect)
        {
            return KaneFoundation.GetMouseX() > rect.X && KaneFoundation.GetMouseX() < rect.X + rect.Width && KaneFoundation.GetMouseY() > rect.Y && KaneFoundation.GetMouseY() < rect.Y + rect.Height;
        }

        public static bool IsMouseInConstraints(Constraints constraints)
        {
            return KaneFoundation.GetMouseX() > constraints.Left && KaneFoundation.GetMouseX() < KaneFoundation.GetScreenWidth() - constraints.Right && KaneFoundation.GetMouseY() > constraints.Top && KaneFoundation.GetMouseY() < KaneFoundation.GetScreenHeight() - constraints.Bottom;
        }

        public static Rect IndexToRectScreen(int i)
        {
            return new Rect(0, 30 + i * KaneUI.DefaultItemHeight, KaneFoundation.GetScreenWidth(), KaneUI.DefaultItemHeight);
        }

        public static Rect IndexToRectScreen(int i, int divisions, int n)
        {
            if (divisions < 1)
            {
                divisions = 1;
            }
            return new Rect(KaneFoundation.GetScreenWidth() / divisions * n, 30 + i * KaneUI.DefaultItemHeight, KaneFoundation.GetScreenWidth() / divisions, KaneUI.DefaultItemHeight);
        }

        public static Rect IndexToRectScreen(int i, int divisions, int n, int width)
        {
            if (divisions < 1)
            {
                divisions = 1;
            }
            return new Rect(KaneFoundation.GetScreenWidth() / divisions * n, 30 + i * KaneUI.DefaultItemHeight, width * (KaneFoundation.GetScreenWidth() / divisions), KaneUI.DefaultItemHeight);
        }
    }
}
