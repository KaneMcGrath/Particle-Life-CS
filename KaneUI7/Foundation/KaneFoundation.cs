using KaneText7;
using ParticleLife.Game;
using Raylib_cs;
using System.Numerics;

namespace KaneUI7.Foundation
{
    //Interop Layer for all KaneUI elements, should be the only class needed to modify to port
    public static class KaneFoundation
    {
        public static bool IsLeftClickDown()
        {
            return Raylib.IsMouseButtonPressed(MouseButton.Left);
        }

        public static bool IsLeftClick()
        {
            return Raylib.IsMouseButtonDown(MouseButton.Left);
        }

        public static bool IsLeftClickUp()
        {
            return Raylib.IsMouseButtonReleased(MouseButton.Left);
        }

        public static bool IsRightClickDown()
        {
            return Raylib.IsMouseButtonPressed(MouseButton.Right);
        }

        public static bool IsAnyMouseButtonDown()
        {
            return Raylib.IsMouseButtonPressed(MouseButton.Left | MouseButton.Right | MouseButton.Middle);
        }

        public static void DrawRect(Rect rectangle, RGBA c)
        {
            Raylib.DrawRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, new Color(c.R, c.G, c.B, c.A));
        }

        public static void DrawText(XY position, string message, int fontSize)
        {
            Raylib.DrawTextEx(KaneGameManager.DefaultFont, message, new Vector2(position.X, position.Y), fontSize, 0, Color.White);
        }

        public static void DrawText(XY position, string message, int fontSize, RGBA color)
        {
            Raylib.DrawTextEx(KaneGameManager.DefaultFont, message, new Vector2(position.X, position.Y), fontSize, 0, new Color(color.R, color.G, color.B, color.A));
        }

        public static XY MeasureText(string text, int fontSize)
        {
            Vector2 vec = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text, fontSize, 0);
            return new XY((int)vec.X, (int)vec.Y);
        }

        public static int GetMouseX()
        {
            return Raylib.GetMouseX();
        }

        public static int GetMouseY()
        {
            return Raylib.GetMouseY();
        }

        public static int GetScreenWidth()
        {
            return Raylib.GetScreenWidth();
        }

        public static int GetScreenHeight()
        {
            return Raylib.GetScreenHeight();
        }

        public static bool IsWindowResized()
        {
            return Raylib.IsWindowResized();
        }

        public static MouseCursor CurrentCursor = MouseCursor.Default;
        public static void SetMouseCursor(MouseCursor mouseCursor)
        {
            if (CurrentCursor != mouseCursor)
            {
                CurrentCursor = mouseCursor;
                Raylib.SetMouseCursor(CurrentCursor);
            }
        }

        public static string EditableText(Rect rect, string text, int fontSize)
        {
            return KaneText.EditableText(rect, new Vector2(rect.X + 2, rect.Y + 2), text, fontSize);
        }
        public static string EditableText(Rect rect, XY Position, string text, int fontSize)
        {
            return KaneText.EditableText(rect, new Vector2(Position.X, Position.Y), text, fontSize);
        }
    }
}
