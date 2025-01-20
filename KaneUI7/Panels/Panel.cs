using KaneUI7.Foundation;

namespace KaneUI7.Panels
{

    /// <summary>
    /// Base Class Which can hold elements inside and move and hide them
    /// Only used for parts which can overlap, for the specific purpose of preventing clickthrough
    /// and to allow dynamic layer switching
    /// elements inside the panel must be rendered from the panels update function
    /// </summary>
    public abstract class Panel
    {
        public Action UpdateFunction;

        /// <summary>
        /// Panel is by default hidden unless this is set to true
        /// used to calculate mouseClickthrough so it will be auto set to false by the PanelManager
        /// if it is not updated
        /// </summary>
        public bool ShowPanel;

        public Rect PanelArea;

        public Panel(Action UpdateFunction)
        {
            this.UpdateFunction = UpdateFunction;
            PanelManager.AddPanel(this);
        }

        public void Dispose()
        {
            PanelManager.RemovePanel(this);
        }
        /// <summary>
        /// Check if the mouse is within the given rect and in the panel and not blocked
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool IsMouseInRect(Rect rect)
        {
            return (PanelManager.IsMouseInPanel(this) && KaneUtils.IsMouseInRect(rect));
        }

        public virtual void Update()
        {
            UpdateFunction();
        }
    }
}
