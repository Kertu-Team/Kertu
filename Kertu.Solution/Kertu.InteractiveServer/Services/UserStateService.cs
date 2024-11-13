namespace Kertu.InteractiveServer.Services
{
    public class UserStateService
    {
        public bool TreeViewNavigationPanelOpened { get; set; }
        public Data.Models.Elements.Element LastOpenedElement { get; set; }
        public bool RecentElementAlreadyOpened { get; set; } = false;
    }
}
