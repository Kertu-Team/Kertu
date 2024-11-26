namespace Kertu.InteractiveServer.Services
{
    public class UserStateService
    {
        private Dictionary<string, bool> navigationPanelOpens = new();

        public bool GetTreeViewNavigationPanelOpened(string userID)
        {
            if (!navigationPanelOpens.ContainsKey(userID))
            {
                navigationPanelOpens.Add(userID, false);
            }
            return navigationPanelOpens[userID];
        }

        public void SetTreeViewNavigationPanelOpened(string userID, bool value)
        {
            if (navigationPanelOpens.ContainsKey(userID))
            {
                navigationPanelOpens[userID] = value;
            }
            else
            {
                navigationPanelOpens.Add(userID, value);
            }
        }

        private Dictionary<string, Data.Models.Elements.Element> lastOpenedElements = new();

        public Data.Models.Elements.Element GetLastOpenedElement(string userID)
        {
            if (!lastOpenedElements.ContainsKey(userID))
            {
                lastOpenedElements.Add(userID, null);
            }
            return lastOpenedElements[userID];
        }

        public void SetLastOpenedElement(string userID, Data.Models.Elements.Element value)
        {
            if (lastOpenedElements.ContainsKey(userID))
            {
                lastOpenedElements[userID] = value;
            }
            else
            {
                lastOpenedElements.Add(userID, value);
            }
        }
    }
}
