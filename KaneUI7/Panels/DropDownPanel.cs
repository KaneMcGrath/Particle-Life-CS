using KaneUI7.Foundation;

namespace KaneUI7.Panels
{
    //spawns from a control and displays a panel under the control
    //has an IndexToRect so you can put anything in the dropdown
    public class DropDownPanel : Panel
    {
        public string SelectedValue;
        public string[] Names = new string[0];
        public int DropdownHeight;
        private bool justSpawnedPanel = false;

        public DropDownPanel(Action? UpdateFunction, int DropdownHeight) : base(UpdateFunction)
        {
            this.DropdownHeight = DropdownHeight;
        }

        public void SpawnDropdown(Rect source, string[] names)
        {
            Names = names;
            ShowPanel = true;
            PanelArea = source;
            justSpawnedPanel = true;
            PanelManager.BringToFront(this);
        }

        /// <summary>
        /// Divides the entire window into rows based on RowHeight and returns a rect that coresponds to the index provided
        /// </summary>
        /// <param name="i">The Index of the row you want to use.  0 will be the top of the window</param>
        public Rect IndexToRect(int i)
        {
            return new Rect(PanelArea.X, PanelArea.Y + (i * KaneUI.DefaultItemHeight), PanelArea.Width, KaneUI.DefaultItemHeight);
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
            return new Rect(PanelArea.X + PanelArea.Width / divisions * n, PanelArea.Y + i * KaneUI.DefaultItemHeight, PanelArea.Width / divisions, KaneUI.DefaultItemHeight);
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
            return new Rect(PanelArea.X + PanelArea.Width / divisions * n, PanelArea.Y + i * KaneUI.DefaultItemHeight, width * (PanelArea.Width / divisions), KaneUI.DefaultItemHeight);
        }

        public override void Update()
        {
            if (Window.DebugText)
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
            if (ShowPanel)
            {
                KaneBlocks.Box(PanelArea);
                if (KaneFoundation.IsLeftClickDown() && !PanelManager.IsMouseInPanel(this) && !justSpawnedPanel)
                {
                    ShowPanel = false;
                }
                base.Update();
            }
            justSpawnedPanel = false;

        }
    }
}
