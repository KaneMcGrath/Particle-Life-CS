namespace KaneUI7.Foundation
{
    /// <summary>
    /// Building blocks for UI pieces
    /// Can also be used on their own for any purpose
    /// </summary>
    public static class KaneBlocks
    {
        /// <summary>
        /// Draws a static box with an outline that will make up most of the GUI elements
        /// </summary>
        /// <param name="Rect"></param>
        public static void Box(Rect Rect)
        {
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness, Rect.Y + KaneUI.DefaultOutlineThickness, Rect.Width - KaneUI.DefaultOutlineThickness * 2, Rect.Height - KaneUI.DefaultOutlineThickness * 2);
            KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
            KaneFoundation.DrawRect(insideRect, DefaultColors.InsideColor);
        }
        public static void Box(Rect Rect, RGBA insideColor)
        {
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness, Rect.Y + KaneUI.DefaultOutlineThickness, Rect.Width - KaneUI.DefaultOutlineThickness * 2, Rect.Height - KaneUI.DefaultOutlineThickness * 2);
            KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
            KaneFoundation.DrawRect(insideRect, insideColor);
        }
        public static void Box(Rect Rect, RGBA insideColor, RGBA outsideColor)
        {
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness, Rect.Y + KaneUI.DefaultOutlineThickness, Rect.Width - KaneUI.DefaultOutlineThickness * 2, Rect.Height - KaneUI.DefaultOutlineThickness * 2);
            KaneFoundation.DrawRect(Rect, outsideColor);
            KaneFoundation.DrawRect(insideRect, insideColor);
        }
        public static void Box(Rect Rect, RGBA insideColor, RGBA outsideColor, int outlineThickness)
        {
            Rect insideRect = new Rect(Rect.X + outlineThickness, Rect.Y + outlineThickness, Rect.Width - outlineThickness * 2, Rect.Height - outlineThickness * 2);
            KaneFoundation.DrawRect(Rect, outsideColor);
            KaneFoundation.DrawRect(insideRect, insideColor);
        }

        /// <summary>
        /// Thickens On Command to indicate interactivity
        /// </summary>
        public static void ActiveBox(Rect Rect, bool thicken = false)
        {
            int outline = KaneUI.DefaultOutlineThickness;
            if (thicken)
            {
                outline += 1;
            }

            Rect insideRect = new Rect(Rect.X + outline, Rect.Y + outline, Rect.Width - outline * 2, Rect.Height - outline * 2);
            KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
            KaneFoundation.DrawRect(insideRect, DefaultColors.InsideColor);
        }

        /// <summary>
        /// Thickens On Command to indicate interactivity
        /// </summary>
        public static void ActiveBox(Rect Rect, RGBA InsideColor, bool thicken = false)
        {
            int outline = KaneUI.DefaultOutlineThickness;
            if (thicken)
            {
                outline += 1;
            }

            Rect insideRect = new Rect(Rect.X + outline, Rect.Y + outline, Rect.Width - outline * 2, Rect.Height - outline * 2);
            KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
            KaneFoundation.DrawRect(insideRect, InsideColor);
        }

        /// <summary>
        /// A box that can invert its colors easily
        /// Can optionally thicken to indicate interactibility
        /// </summary>
        public static void SwitchBox(Rect Rect, bool invert, bool thicken = false)
        {
            int outlineModifier = 0;
            if (thicken)
            {
                outlineModifier = 1;
            }
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Y + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Width - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2, Rect.Height - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2);
            if (invert)
            {
                KaneFoundation.DrawRect(Rect, DefaultColors.InsideColor);
                KaneFoundation.DrawRect(insideRect, DefaultColors.OutsideColor);
            }
            else
            {
                KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
                KaneFoundation.DrawRect(insideRect, DefaultColors.InsideColor);
            }
        }
        public static void SwitchBox(Rect Rect, bool invert, RGBA insideColor, bool thicken = false)
        {
            int outlineModifier = 0;
            if (thicken)
            {
                outlineModifier = 1;
            }
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Y + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Width - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2, Rect.Height - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2);
            if (invert)
            {
                KaneFoundation.DrawRect(Rect, insideColor);
                KaneFoundation.DrawRect(insideRect, DefaultColors.OutsideColor);
            }
            else
            {
                KaneFoundation.DrawRect(Rect, DefaultColors.OutsideColor);
                KaneFoundation.DrawRect(insideRect, insideColor);
            }
        }

        public static void SwitchBox(Rect Rect, bool invert, RGBA insideColor, RGBA staticOutsideTex, bool thicken = false)
        {
            int outlineModifier = 0;
            if (thicken)
            {
                outlineModifier = 1;
            }
            Rect insideRect = new Rect(Rect.X + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Y + KaneUI.DefaultOutlineThickness + outlineModifier, Rect.Width - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2, Rect.Height - (KaneUI.DefaultOutlineThickness + outlineModifier) * 2);
            if (invert)
            {
                KaneFoundation.DrawRect(Rect, staticOutsideTex);
                KaneFoundation.DrawRect(insideRect, DefaultColors.OutsideColor);
            }
            else
            {
                KaneFoundation.DrawRect(Rect, staticOutsideTex);
                KaneFoundation.DrawRect(insideRect, insideColor);
            }
        }

        public static void DrawOutline(Rect rect, int thickness, RGBA color)
        {
            KaneFoundation.DrawRect(new Rect(rect.X, rect.Y, rect.Width, thickness), color);
            KaneFoundation.DrawRect(new Rect(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            KaneFoundation.DrawRect(new Rect(rect.X, rect.Y, thickness, rect.Height), color);
            KaneFoundation.DrawRect(new Rect(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }

        /// <summary>
        /// Creates 4 rects that form an outline of the input rect
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rect[] TransparentOutlineRects(Rect rect, int thickness)
        {
            Rect[] result = new Rect[4];
            result[0] = new Rect(rect.X, rect.Y, rect.Width, thickness);
            result[1] = new Rect(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness);
            result[2] = new Rect(rect.X, rect.Y + thickness, thickness, rect.Height - thickness);
            result[3] = new Rect(rect.X + rect.Width - thickness, rect.Y + thickness, thickness, rect.Height - thickness);
            return result;
        }


    }
}
