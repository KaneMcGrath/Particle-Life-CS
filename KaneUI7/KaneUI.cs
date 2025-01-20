using KaneUI7.Foundation;
using KaneUI7.Panels;

namespace KaneUI7
{

    //Custom immediate mode gui designed to be as immidiate mode as I can make it.
    //FlatUI7 renamed because there are like 30 other UI's named FlatUI
    //much more portable and self contained for easy reuse
    //Now Panel Based, use of panels is kept minimal, mostly for mouse click through prevention
    //And Elements in panels must be called from a delegate in the panel class
    //maybe too much voodoo for our purposes but it works pretty well
    public static class KaneUI
    {
        public static int DefaultOutlineThickness = 2;
        public static int DefaultItemHeight = 30;

        public static Panel ActivePanel = null;

        public static void SetActivePanel(Panel panel)
        {
            ActivePanel = panel;
        }

        public static void ClearActivePanel()
        {
            ActivePanel = null;
        }

        /// <summary>
        /// Draw Text with the default font
        /// Side: 0=topleft    1=topcenter    2=topright
        ///       3=midleft    4=center       5=midright 
        ///       6=bottomleft 7=bottomcenter 8=bottomright
        /// </summary>
        public static void Label(Rect rect, string message, int fontSize = 20, int side = 0, bool draw = true)
        {
            if (draw)
            {
                int x = rect.X + 2;
                int y = rect.Y + 2;
                if (side == 1) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; }
                else if (side == 2) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; }
                else if (side == 3) { y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 4) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 5) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 6) { y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                else if (side == 7) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                else if (side == 8) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                KaneFoundation.DrawText(new XY(x, y), message, fontSize);
            }
        }


        /// <summary>
        /// Draw Text with the default font
        /// Side: 0=topleft    1=topcenter    2=topright
        ///       3=midleft    4=center       5=midright 
        ///       6=bottomleft 7=bottomcenter 8=bottomright
        /// </summary>
        public static void Label(Rect rect, string message, RGBA color, int fontSize = 20, int side = 0, bool draw = true)
        {
            if (draw)
            {
                int x = rect.X;
                int y = rect.Y;
                if (side == 1) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; }
                else if (side == 2) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; }
                else if (side == 3) { y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 4) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 5) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; y = rect.Y + rect.Height / 2 - KaneFoundation.MeasureText(message, fontSize).Y / 2; }
                else if (side == 6) { y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                else if (side == 7) { x = rect.X + rect.Width / 2 - KaneFoundation.MeasureText(message, fontSize).X / 2; y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                else if (side == 8) { x = rect.X + rect.Width - KaneFoundation.MeasureText(message, fontSize).X; y = rect.Y + rect.Height - KaneFoundation.MeasureText(message, fontSize).Y; }
                KaneFoundation.DrawText(new XY(x, y), message, fontSize, color);
            }
        }

        /// <summary>
        /// Styled button, inverts colors when clicked
        /// </summary>
        public static bool Button(Rect Rect, string label, bool draw = true)
        {
            if (draw)
            {
                bool inRect = PanelManager.IsMouseInActivePanelAndRect(Rect);
                KaneBlocks.SwitchBox(Rect, inRect && KaneFoundation.IsLeftClick(), DefaultColors.ButtonColor, inRect);
                Label(Rect, label, 20, 4);
                return inRect && KaneFoundation.IsLeftClickDown();
            }
            else
            {
                return false;
            }
        }
        public static bool Button(Rect Rect, string label, RGBA insideColor, bool draw = true)
        {
            if (draw)
            {
                bool inRect = PanelManager.IsMouseInActivePanelAndRect(Rect);
                KaneBlocks.SwitchBox(Rect, inRect && KaneFoundation.IsLeftClick(), insideColor, inRect);
                Label(Rect, label, 20, 4);
                return inRect && KaneFoundation.IsLeftClickDown();
            }
            else
            {
                return false;
            }
        }
        public static bool Button(Rect Rect, string label, RGBA insideColor, RGBA outsideColor, bool draw = true)
        {
            if (draw)
            {
                bool inRect = PanelManager.IsMouseInActivePanelAndRect(Rect);
                KaneBlocks.SwitchBox(Rect, inRect && KaneFoundation.IsLeftClick(), insideColor, outsideColor, inRect);
                Label(Rect, label, 20, 4);
                return inRect && KaneFoundation.IsLeftClickDown();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Styled Check Box, inverts colors when checked.  Includes a label to the left side of the box
        /// </summary>
        public static bool CheckBox(Rect Rect, bool value, string label, bool draw = true)
        {
            if (!draw)
            {
                return value;
            }
            Rect checkRect = new Rect(Rect.X + Rect.Width - 28, Rect.Y + 2, 26, Rect.Height - 4);
            bool inRect = PanelManager.IsMouseInActivePanelAndRect(checkRect);
            KaneBlocks.SwitchBox(checkRect, value, inRect);
            if (inRect && KaneFoundation.IsLeftClickDown())
            {
                return !value;
            }
            Label(new Rect(Rect.X + 4, Rect.Y, Rect.Width, Rect.Height), label);
            return value;
        }

        /// <summary>
        /// Styled Check Box that behaves as a button, inverts colors when checked.  Includes a label to the left side of the box
        /// </summary>
        public static bool CheckButton(Rect Rect, ref bool value, string label, bool draw = true)
        {
            if (draw)
            {
                Rect checkRect = new Rect(Rect.X + Rect.Width - 28, Rect.Y + 2, 26, Rect.Height - 4);
                bool inRect = PanelManager.IsMouseInActivePanelAndRect(checkRect);
                KaneBlocks.SwitchBox(checkRect, value, inRect);
                if (inRect && KaneFoundation.IsLeftClickDown())
                {
                    value = !value;
                    return true;
                }
                Label(new Rect(Rect.X + 4, Rect.Y, Rect.Width, Rect.Height), label);
            }
            return false;
        }

        //global variables to use between all sliders as we should only be using one at a time
        //and if I can get away with not making an object for this that would be sick
        public static bool IsDraggingSlider = false;

        private static float SliderDragOffset = 0f;
        private static float SliderThickness = 10f;
        private static Rect DefaultRect = new Rect();
        private static Rect HasSlider = DefaultRect;

        /// <summary>
        /// Slider with specified min and max values
        /// </summary>
        public static float Slider(Rect Rect, float value, float minValue, float maxValue, bool draw = true)
        {
            if (!draw)
            {
                return value;
            }

            float newValue = value;
            float valueSpan = maxValue - minValue;
            float halfThickSlider = SliderThickness / 2f;
            KaneBlocks.Box(Rect);
            Rect SliderRect = new Rect((int)(Rect.X + halfThickSlider + (value - minValue) / valueSpan * (Rect.Width - SliderThickness) - halfThickSlider), Rect.Y, (int)SliderThickness, Rect.Height);
            bool inSliderRect = PanelManager.IsMouseInActivePanelAndRect(SliderRect);
            bool doWeHaveSlider = (HasSlider == Rect);
            KaneBlocks.ActiveBox(SliderRect, inSliderRect || (IsDraggingSlider && doWeHaveSlider));
            if (PanelManager.IsMouseInActivePanelAndRect(Rect))
            {
                //if (Raylib.GetMouseWheelMoveV().Y > 0f)
                //{
                //    newValue += valueSpan / 40f;
                //    if (newValue < minValue)
                //    {
                //        newValue = minValue;
                //    }
                //
                //    if (newValue > maxValue)
                //    {
                //        newValue = maxValue;
                //    }
                //}
                //if (Raylib.GetMouseWheelMoveV().Y < 0f)
                //{
                //    newValue -= valueSpan / 40f;
                //    if (newValue < minValue)
                //    {
                //        newValue = minValue;
                //    }
                //
                //    if (newValue > maxValue)
                //    {
                //        newValue = maxValue;
                //    }
                //}
                if (KaneFoundation.IsLeftClickDown())
                {
                    if (inSliderRect)
                    {
                        IsDraggingSlider = true;
                        SliderDragOffset = KaneFoundation.GetMouseX() - SliderRect.X;
                        HasSlider = Rect;
                    }
                    else
                    {

                        newValue = (KaneFoundation.GetMouseX() - (Rect.X + halfThickSlider)) / (Rect.Width - SliderThickness) * valueSpan + minValue;
                        IsDraggingSlider = true;
                        SliderDragOffset = halfThickSlider;
                        HasSlider = Rect;
                    }
                }
            }
            if (IsDraggingSlider && HasSlider == Rect)
            {
                int x = SliderRect.X;
                if (SliderRect.X > KaneFoundation.GetScreenWidth() - 100)
                {
                    x = KaneFoundation.GetScreenWidth() - 100;
                }

                Rect numberDisplay = new Rect(x, SliderRect.Y - 25, 100, 25);
                KaneBlocks.Box(numberDisplay);
                Label(numberDisplay, value.ToString(), 20, 4);
                newValue = (KaneFoundation.GetMouseX() - (Rect.X + halfThickSlider) - SliderDragOffset + halfThickSlider) / (Rect.Width - SliderThickness) * valueSpan + minValue;
                if (newValue < minValue)
                {
                    newValue = minValue;
                }

                if (newValue > maxValue)
                {
                    newValue = maxValue;
                }
            }
            if (KaneFoundation.IsLeftClickUp())
            {
                IsDraggingSlider = false;
                HasSlider = DefaultRect;
            }

            return newValue;
        }

        public static float Slider(Rect Rect, float value, float minValue, float maxValue, RGBA sliderColor, bool draw = true)
        {
            if (!draw)
            {
                return value;
            }

            float newValue = value;
            float valueSpan = maxValue - minValue;
            float halfThickSlider = SliderThickness / 2f;
            KaneBlocks.Box(Rect);
            Rect SliderRect = new Rect((int)(Rect.X + halfThickSlider + (value - minValue) / valueSpan * (Rect.Width - SliderThickness) - halfThickSlider), Rect.Y, (int)SliderThickness, Rect.Height);
            bool inSliderRect = PanelManager.IsMouseInActivePanelAndRect(SliderRect);
            bool doWeHaveSlider = (HasSlider == Rect);
            KaneBlocks.ActiveBox(SliderRect, sliderColor, inSliderRect || (IsDraggingSlider && doWeHaveSlider));
            if (PanelManager.IsMouseInActivePanelAndRect(Rect))
            {
                if (KaneFoundation.IsLeftClickDown())
                {
                    if (inSliderRect)
                    {
                        IsDraggingSlider = true;
                        SliderDragOffset = KaneFoundation.GetMouseX() - SliderRect.X;
                        HasSlider = Rect;
                    }
                    else
                    {

                        newValue = (KaneFoundation.GetMouseX() - (Rect.X + halfThickSlider)) / (Rect.Width - SliderThickness) * valueSpan + minValue;
                        IsDraggingSlider = true;
                        SliderDragOffset = halfThickSlider;
                        HasSlider = Rect;
                    }
                }
            }
            if (IsDraggingSlider && HasSlider == Rect)
            {
                int x = SliderRect.X;
                if (SliderRect.X > KaneFoundation.GetScreenWidth() - 100)
                {
                    x = KaneFoundation.GetScreenWidth() - 100;
                }

                Rect numberDisplay = new Rect(x, SliderRect.Y - 25, 100, 25);
                KaneBlocks.Box(numberDisplay);
                Label(numberDisplay, value.ToString(), 20, 4);
                newValue = (KaneFoundation.GetMouseX() - (Rect.X + halfThickSlider) - SliderDragOffset + halfThickSlider) / (Rect.Width - SliderThickness) * valueSpan + minValue;
                if (newValue < minValue)
                {
                    newValue = minValue;
                }

                if (newValue > maxValue)
                {
                    newValue = maxValue;
                }
            }
            if (KaneFoundation.IsLeftClickUp())
            {
                IsDraggingSlider = false;
                HasSlider = DefaultRect;
            }

            return newValue;
        }

        public static string TextField(Rect rect, string text, int fontSize = 20, int side = 0, bool draw = true)
        {
            if (draw)
            {
                side = 0;
                int x = rect.X;
                int y = rect.Y;
                if (side == 1) { x = rect.X + rect.Width / 2 - (KaneFoundation.MeasureText(text, fontSize).X / 2); }
                else if (side == 2) { x = rect.X + rect.Width - KaneFoundation.MeasureText(text, fontSize).X; }
                else if (side == 3) { y = rect.Y + rect.Height / 2 - (KaneFoundation.MeasureText(text, fontSize).Y / 2); }
                else if (side == 4) { x = rect.X + rect.Width / 2 - (KaneFoundation.MeasureText(text, fontSize).X / 2); ; y = rect.Y + rect.Height / 2 - (KaneFoundation.MeasureText(text, fontSize).Y / 2); }
                else if (side == 5) { x = rect.X + rect.Width - KaneFoundation.MeasureText(text, fontSize).X; y = rect.Y + rect.Height / 2 - (KaneFoundation.MeasureText(text, fontSize).Y / 2); }
                else if (side == 6) { y = rect.Y + rect.Height - KaneFoundation.MeasureText(text, fontSize).Y; }
                else if (side == 7) { x = rect.X + rect.Width / 2 - (KaneFoundation.MeasureText(text, fontSize).X / 2); y = rect.Y + rect.Height - KaneFoundation.MeasureText(text, fontSize).Y; }
                else if (side == 8) { x = rect.X + rect.Width - KaneFoundation.MeasureText(text, fontSize).X; y = rect.Y + rect.Height - KaneFoundation.MeasureText(text, fontSize).Y; }

                KaneBlocks.Box(rect, DefaultColors.TextFieldColor, DefaultColors.TextFieldOutlineColor);
                Rect Margin = new Rect(rect.X + 2, rect.Y + 2, rect.Width - 2, rect.Height - 2);
                return KaneFoundation.EditableText(Margin, new XY(x + 2, y + 2), text, fontSize);
            }
            else
            {
                return text;
            }
        }

        public static int tabs(Rect pos, string[] tabs, int index, bool top, RGBA color)
        {
            int num = tabs.Length;
            int num2 = pos.Width / num;
            RGBA[] array = new RGBA[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = color;
            }
            for (int j = 0; j < num; j++)
            {
                Rect rect;
                if (top)
                {
                    rect = new Rect(pos.X + num2 * j, pos.Y, num2, pos.Height);
                }
                else
                {
                    rect = new Rect(pos.X, pos.Y + j * 30, 100, 30);
                }
                if (index == j)
                {
                    tab(rect, top, array[j], DefaultColors.OutsideColor);
                    Label(rect, tabs[j]);
                }
                else
                {
                    KaneBlocks.Box(rect, array[j], DefaultColors.OutsideColor);
                    if (Button(rect, tabs[j]))
                    {
                        return j;
                    }
                }
            }
            return index;
        }
        public static int tabs(Rect pos, string[] tabs, int index, bool top, RGBA[] tabColors)
        {
            int num = tabs.Length;
            int num2 = pos.Width / num;
            RGBA[] array = new RGBA[num];
            for (int i = 0; i < num; i++)
            {
                if (i < tabColors.Length)
                {
                    array[i] = tabColors[i];
                }
                else
                {
                    array[i] = DefaultColors.InsideColor;
                }
            }
            for (int j = 0; j < num; j++)
            {
                Rect rect;
                if (top)
                {
                    rect = new Rect(pos.X + num2 * j, pos.Y, num2, pos.Height);
                }
                else
                {
                    rect = new Rect(pos.X, pos.Y + j * 30, 100, 30);
                }
                if (index == j)
                {
                    tab(rect, top, array[j], DefaultColors.OutsideColor);
                    Label(rect, tabs[j]);
                }
                else
                {
                    //KaneBlocks.Box(rect, array[j], DefaultColors.OutsideColor);
                    if (Button(rect, tabs[j], array[j]))
                    {
                        return j;
                    }
                }
            }
            return index;
        }
        private static void tab(Rect rect, bool top, RGBA inside, RGBA outside)
        {
            if (top)
            {
                KaneFoundation.DrawRect(new Rect(rect.X, rect.Y, rect.Width, rect.Height), outside);
                KaneFoundation.DrawRect(new Rect(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height), inside);
                return;
            }
            else
            {
                KaneFoundation.DrawRect(new Rect(rect.X - 2, rect.Y, rect.Width, rect.Height), outside);
                KaneFoundation.DrawRect(new Rect(rect.X - 2, rect.Y + 2, rect.Width - 2, rect.Height - 4), inside);
                return;
            }
        }
    }
}
