using KaneUI7.Foundation;
using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;
using static KaneUI7.Foundation.KaneUtils;

namespace KaneUI7
{
    /// <summary>
    /// Raylib specific implementation to make a window look like a kaneUI window
    /// </summary>
    public static class KaneWindowDecoration
    {
        public static bool isDragging = false;
        private static int dragXOffset = 0;
        private static int dragYOffset = 0;
        public static bool isResizingWindow = false;
        private static POINT ResizePosition = new POINT();
        private static int InitialWidth = 0;
        private static int InitialHeight = 0;

        public static int minWidth = 100;
        public static int minHeight = 50;

        public static bool CanMaximize = true;
        public static bool CanResize = true;

        /// <summary>
        /// Draw the Titlebar at the top of the screen to make the actual sdl Window in the style of the KaneUIWindow
        /// optional outline to mimic the window style further
        /// use window constraints to avoid windows overlaping the titlebar
        /// </summary>
        public static void WindowDecoration(RGBA backgroundColor, RGBA TitleBarColor, string WindowTitle)
        {
            Rect rect = new Rect(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            Rect titleBarRect = new Rect(rect.X, rect.Y, rect.Width, 30);
            Rect titleBarDragRect = new Rect(rect.X, rect.Y, rect.Width - 90, 30);

            int minimizeX = rect.X + rect.Width - 60;
            if (CanMaximize)
            {
                minimizeX -= 30;
            }
            Rect minimizeButtonRect = new Rect(minimizeX, rect.Y, 30, 30);
            Rect maximiseButtonRect = new Rect(rect.X + rect.Width - 60, rect.Y, 30, 30);
            Rect closeButtonRect = new Rect(rect.X + rect.Width - 30, rect.Y, 30, 30);
            Rect ContentRect = new Rect(rect.X, rect.Y + 30, rect.Width, rect.Height - 30);

            Rect ScreenRect = new Rect(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            Rect[] outlines = KaneBlocks.TransparentOutlineRects(ScreenRect, KaneUI.DefaultOutlineThickness);
            for (int i = 0; i < 4; i++)
            {
                KaneFoundation.DrawRect(outlines[i], DefaultColors.OutsideColor);
            }
            KaneBlocks.Box(titleBarRect, TitleBarColor);
            KaneUI.Label(titleBarDragRect, WindowTitle, 24, 4);
            if (KaneUI.Button(minimizeButtonRect, "-", TitleBarColor))
            {
                Raylib.MinimizeWindow();
            }
            bool isMaximized = Raylib.IsWindowState(ConfigFlags.MaximizedWindow);
            if (CanMaximize)
            {
                if (KaneUI.Button(maximiseButtonRect, isMaximized ? "[]" : "[  ]", TitleBarColor))
                {
                    if (isMaximized)
                    {
                        Raylib.ClearWindowState(ConfigFlags.MaximizedWindow);
                    }
                    else
                    {
                        Raylib.MaximizeWindow();
                    }
                }
            }
            if (KaneUI.Button(closeButtonRect, "x", TitleBarColor))
            {
                Environment.Exit(0);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (PanelManager.IsMouseInRectAndUnoccluded(titleBarDragRect))
                {
                    if (Raylib.IsWindowState(ConfigFlags.MaximizedWindow))
                    {
                        Raylib.ClearWindowState(ConfigFlags.MaximizedWindow);

                        GetCursorPos(out POINT mousePOS);
                        int halfWidth = KaneFoundation.GetScreenWidth() / 2;
                        Raylib.SetWindowPosition(mousePOS.X - halfWidth, mousePOS.Y - 15);
                        Vector2 pos = Raylib.GetWindowPosition();
                        dragXOffset = mousePOS.X - (int)pos.X;
                        dragYOffset = mousePOS.Y - (int)pos.Y;
                    }
                    else
                    {
                        GetCursorPos(out POINT mousePOS);
                        Vector2 pos = Raylib.GetWindowPosition();
                        dragXOffset = mousePOS.X - (int)pos.X;
                        dragYOffset = mousePOS.Y - (int)pos.Y;
                    }
                    isDragging = true;
                }
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                isDragging = false;
            }
            if (isDragging)
            {
                //Vector2 pos = Raylib.GetWindowPosition();
                GetCursorPos(out POINT mousePOS);
                Raylib.SetWindowPosition(mousePOS.X - dragXOffset, mousePOS.Y - dragYOffset);  //(int)pos.Y + Raylib.GetMouseY() - dragYOffset     + (Raylib.GetMouseX() - dragXOffset)
                                                                                               //Console.WriteLine("[off:" + dragXOffset + "]{mouse:" + mousePOS.X + "} = " + (mousePOS.X - dragXOffset));
            }

            //Resize Handle
            Rect ResizeHandle1 = new Rect(KaneFoundation.GetScreenWidth() - 8, KaneFoundation.GetScreenHeight() - 20, 4, 16);
            Rect ResizeHandle2 = new Rect(KaneFoundation.GetScreenWidth() - 20, KaneFoundation.GetScreenHeight() - 8, 16, 4);
            Rect ResizeMouseHandle1 = new Rect(KaneFoundation.GetScreenWidth() - 8, KaneFoundation.GetScreenHeight() - 20, 8, 20);
            Rect ResizeMouseHandle2 = new Rect(KaneFoundation.GetScreenWidth() - 20, KaneFoundation.GetScreenHeight() - 8, 20, 8);
            KaneFoundation.DrawRect(ResizeHandle1, DefaultColors.OutsideColor);
            KaneFoundation.DrawRect(ResizeHandle2, DefaultColors.OutsideColor);

            //lazy and bad but probably works fine
            if (IsMouseInRect(ResizeMouseHandle1) || IsMouseInRect(ResizeMouseHandle2))
            {
                KaneFoundation.SetMouseCursor(MouseCursor.ResizeNwse);
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    isResizingWindow = true;
                    GetCursorPos(out ResizePosition);
                    InitialWidth = KaneFoundation.GetScreenWidth();
                    InitialHeight = KaneFoundation.GetScreenHeight();
                }
            }
            else
            {
                KaneFoundation.SetMouseCursor(MouseCursor.Default);
            }
            if (isResizingWindow)
            {
                POINT MOUSEPOS = new POINT();
                GetCursorPos(out MOUSEPOS);
                int dx = MOUSEPOS.X - ResizePosition.X;
                int dy = MOUSEPOS.Y - ResizePosition.Y;
                int newWidth = Math.Max(InitialWidth + dx, minWidth);
                int newHeight = Math.Max(InitialHeight + dy, minHeight);
                Raylib.SetWindowSize(newWidth, newHeight);
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                isResizingWindow = false;
            }
        }


        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static System.Drawing.Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }



        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator System.Drawing.Point(POINT point)
            {
                return new System.Drawing.Point(point.X, point.Y);
            }
        }
    }
}
