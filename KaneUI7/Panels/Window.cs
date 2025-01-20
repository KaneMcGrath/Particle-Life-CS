using KaneUI7.Foundation;

namespace KaneUI7.Panels
{
    public class Window : Panel
    {
        public string Title;
        public RGBA insideColor;
        public bool minimize = false;
        public Rect ContentRect;
        public int MinimizedWidth;
        public bool isDragging = false;

        public Constraints constraints = new Constraints();

        private Rect titleBarRect;
        private Rect titleBarDragRect;
        private Rect minimizeButtonRect;
        private Rect closeButtonRect;

        private float dragXOffset = 0f;
        private float dragYOffset = 0f;

        private int lastWindowHeight = 0;

        public Window(Action UpdateFunction, Rect rect, string title, RGBA insideColor) : base(UpdateFunction)
        {
            this.UpdateFunction = UpdateFunction;
            this.PanelArea = rect;
            lastWindowHeight = rect.Height;
            this.Title = title;
            this.insideColor = insideColor;
            MinimizedWidth = rect.Width;
            UpdateRects();
        }

        public Window(Action UpdateFunction, Rect rect, string title) : base(UpdateFunction)
        {
            this.UpdateFunction = UpdateFunction;
            this.PanelArea = rect;
            lastWindowHeight = rect.Height;
            this.Title = title;
            insideColor = DefaultColors.InsideColor;
            MinimizedWidth = rect.Width;
            UpdateRects();
        }

        private void UpdateRects()
        {
            if (minimize)
            {
                titleBarRect = new Rect(PanelArea.X + PanelArea.Width - MinimizedWidth, PanelArea.Y, MinimizedWidth, 30);
                titleBarDragRect = new Rect(PanelArea.X + PanelArea.Width - MinimizedWidth, PanelArea.Y, MinimizedWidth - 60, 30);
            }
            else
            {
                titleBarRect = new Rect(PanelArea.X, PanelArea.Y, PanelArea.Width, 30);
                titleBarDragRect = new Rect(PanelArea.X, PanelArea.Y, PanelArea.Width - 60, 30);
            }
            minimizeButtonRect = new Rect(PanelArea.X + PanelArea.Width - 60, PanelArea.Y, 30, 30);
            closeButtonRect = new Rect(PanelArea.X + PanelArea.Width - 30, PanelArea.Y, 30, 30);
            ContentRect = new Rect(PanelArea.X, PanelArea.Y + 30, PanelArea.Width, PanelArea.Height - 30);
        }

        public static bool DebugText = false;

        public override void Update()
        {
            if (DebugText)
            {
                bool inArea = PanelManager.IsMouseInPanel(this);
                Rect debugRect = new Rect(PanelArea.X, PanelArea.Y - 60, 100, 60);
                KaneFoundation.DrawRect(debugRect, DefaultColors.Black);
                KaneUI.Label(new Rect(PanelArea.X, PanelArea.Y - 30, 300, 30), "IsMouseInPanel? " + inArea.ToString());
                KaneUI.Label(new Rect(PanelArea.X, PanelArea.Y - 60, 300, 30), "showPanel? " + ShowPanel.ToString());
                if (inArea)
                {
                    Rect BigPanel = new Rect(PanelArea.X - 4, PanelArea.Y - 4, PanelArea.Width + 8, PanelArea.Height + 8);
                    Rect[] Outline = KaneBlocks.TransparentOutlineRects(BigPanel, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        KaneFoundation.DrawRect(Outline[i], DefaultColors.White);
                    }
                }
            }
            OnGUI();
            if (ContentVisible())
            {
                base.Update();
            }
            if (PanelManager.IsMouseInPanel(this) && KaneFoundation.IsLeftClickDown())
            {
                PanelManager.BringToFront(this);
            }
        }

        /// <summary>
        /// Returns true if the window contents should be visible
        /// meaning it is not minimized or closed
        /// </summary>
        /// <returns></returns>
        public bool ContentVisible()
        {
            if (ShowPanel)
            {
                if (minimize)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void OnGUI()
        {
            if (ShowPanel)
            {
                if (KaneFoundation.IsLeftClickDown())
                {
                    if (IsMouseInRect(titleBarDragRect))
                    {
                        isDragging = true;
                        dragXOffset = KaneFoundation.GetMouseX() - PanelArea.X;
                        dragYOffset = KaneFoundation.GetMouseY() - PanelArea.Y;
                    }
                }
                if (KaneFoundation.IsLeftClickUp())
                {
                    isDragging = false;
                }
                if (isDragging)
                {
                    PanelArea.X = KaneFoundation.GetMouseX() - (int)dragXOffset;
                    PanelArea.Y = KaneFoundation.GetMouseY() - (int)dragYOffset;
                    ConstrainWindow();
                }
                if (KaneFoundation.IsWindowResized())
                {
                    ConstrainWindow();
                }
                if (!minimize)
                {
                    KaneBlocks.Box(PanelArea, insideColor);
                }
                KaneBlocks.Box(titleBarRect, insideColor);
                KaneUI.Label(titleBarDragRect, Title, 24, 4);
                if (KaneUI.Button(minimizeButtonRect, "-"))
                {
                    minimize = !minimize;
                    if (minimize)
                    {
                        lastWindowHeight = PanelArea.Height;
                        PanelArea.Height = 30;
                    }
                    else
                    {
                        PanelArea.Height = lastWindowHeight;
                    }
                    UpdateRects();
                }
                if (KaneUI.Button(closeButtonRect, "x"))
                {
                    ShowPanel = false;
                }
            }
        }

        private void ConstrainWindow()
        {
            if (PanelArea.X < 0 + constraints.Left)
            {
                PanelArea.X = 0 + constraints.Left;
            }

            if (PanelArea.Y < 0 + constraints.Top)
            {
                PanelArea.Y = 0 + constraints.Top;
            }

            if (PanelArea.X > KaneFoundation.GetScreenWidth() - PanelArea.Width - constraints.Right)
            {
                PanelArea.X = KaneFoundation.GetScreenWidth() - PanelArea.Width - constraints.Right;
            }

            if (PanelArea.Y > KaneFoundation.GetScreenHeight() - 30f - constraints.Bottom)
            {
                PanelArea.Y = KaneFoundation.GetScreenHeight() - 30 - constraints.Bottom;
            }

            UpdateRects();
        }

        /// <summary>
        /// Divides the entire window into rows based on RowHeight and returns a rect that coresponds to the index provided
        /// </summary>
        /// <param name="i">The Index of the row you want to use.  0 will be the top of the window</param>
        public Rect IndexToRect(int i)
        {
            return new Rect(ContentRect.X, ContentRect.Y + i * KaneUI.DefaultItemHeight, ContentRect.Width, KaneUI.DefaultItemHeight);
        }

        /// <summary>
        /// Divides the entire window into rows based on RowHeight and returns a rect that coresponds to the index provided
        /// </summary>
        /// <param name="i">The Index of the row you want to use.  0 will be the top of the window</param>
        /// <param name="divisions">Divide the row into a number of columns</param>
        /// <param name="n">which index of the columns to return (0-indexed)</param>
        /// <example>IndexToRect(0, 4, 2)  Will return the first row and will return the 3rd quarter of that row</example>
        public Rect IndexToRect(int i, int divisions, int n)
        {
            if (divisions < 1)
            {
                divisions = 1;
            }
            return new Rect(ContentRect.X + ContentRect.Width / divisions * n, ContentRect.Y + i * KaneUI.DefaultItemHeight, ContentRect.Width / divisions, KaneUI.DefaultItemHeight);
        }

        /// <summary>
        /// Divides the entire window into rows based on RowHeight and returns a rect that coresponds to the index provided
        /// </summary>
        /// <param name="i">The Index of the row you want to use.  0 will be the top of the window</param>
        /// <param name="divisions">Divide the row into a number of columns</param>
        /// <param name="n">which index of the columns to return (0-indexed)</param>
        /// <param name="width">Number of divided columns to combine.</param>
        /// <example>IndexToRect(0, 6, 1, 2)  first row divided into 6 columns and 2 columns wide starting at the second column</example>
        public Rect IndexToRect(int i, int divisions, int n, int width)
        {
            if (divisions < 1)
            {
                divisions = 1;
            }
            return new Rect(ContentRect.X + ContentRect.Width / divisions * n, ContentRect.Y + i * KaneUI.DefaultItemHeight, width * (ContentRect.Width / divisions), KaneUI.DefaultItemHeight);
        }
    }
}

