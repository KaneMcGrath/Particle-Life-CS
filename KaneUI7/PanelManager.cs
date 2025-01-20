using KaneUI7.Foundation;
using KaneUI7.Panels;

namespace KaneUI7
{
    /// <summary>
    /// Keeps track of panel layering to prevent mouse clickthrough
    /// and runs updating of panel objects 
    /// </summary>
    public static class PanelManager
    {
        public static List<Panel> AllPanels = new List<Panel>();

        public static void AddPanel(Panel panel)
        {
            AllPanels.Add(panel);
        }

        public static void RemovePanel(Panel panel)
        {
            AllPanels.Remove(panel);
        }

        public static void UpdatePanels()
        {
            for (int i = 0; i < AllPanels.Count; i++)
            {
                KaneUI.SetActivePanel(AllPanels[i]);
                AllPanels[i].Update();
            }
            KaneUI.ClearActivePanel();
            if (BringToFrontFlag)
            {
                while (BringToFrontStack.Count > 0)
                {
                    Panel panel = BringToFrontStack.Pop();
                    AllPanels.Remove(panel);
                    AllPanels.Add(panel);
                }
            }
        }

        //Queue the panels to bring to the front at the end of the frame
        //so we dont run update twice when something moves from the begining to the end
        //maybe we can have more than one panel to bring to the front per frame IDK.
        private static Stack<Panel> BringToFrontStack = new Stack<Panel>();
        private static bool BringToFrontFlag = false;

        public static void BringToFront(Panel panel)
        {
            BringToFrontFlag = true;
            BringToFrontStack.Push(panel);
        }

        /// <summary>
        /// check if the mouse is in a rectangle and not covered by any visible panels
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool IsMouseInRectAndUnoccluded(Rect rect)
        {
            return (KaneUtils.IsMouseInRect(rect) && !IsMouseInAnyPanel());
        }

        public static bool IsMouseInActivePanelAndRect(Rect rect)
        {
            if (KaneUI.ActivePanel == null)
            {
                return IsMouseInRectAndUnoccluded(rect);
            }
            else
            {
                return (IsMouseInPanel(KaneUI.ActivePanel) && KaneUtils.IsMouseInRect(rect));
            }
        }

        public static bool IsMouseInPanelAndRect(Panel panel, Rect rect)
        {
            if (panel == null)
            {
                return IsMouseInRectAndUnoccluded(rect);
            }
            else
            {
                return (IsMouseInPanel(panel) && KaneUtils.IsMouseInRect(rect));
            }
        }

        //is the mouse in the panel and not currently behind another panel
        public static bool IsMouseInPanel(Panel panel)
        {
            if (panel.ShowPanel && KaneUtils.IsMouseInRect(panel.PanelArea))
            {
                int myPanelIndex = AllPanels.IndexOf(panel);
                //check if the mouse is within each panel above the current panel
                for (int i = myPanelIndex + 1; i < AllPanels.Count; i++)
                {
                    if (AllPanels[i].ShowPanel && KaneUtils.IsMouseInRect(AllPanels[i].PanelArea))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsMouseInAnyPanel()
        {
            foreach (Panel panel in AllPanels)
            {
                if (panel.ShowPanel && KaneUtils.IsMouseInRect(panel.PanelArea))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
