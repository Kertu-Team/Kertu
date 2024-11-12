using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kertu.InteractiveServer.Services
{
    public class UserStateService
    {
        public bool TreeViewNavigationPanelOpened { get; set; }
        public Data.Models.Elements.Element LastOpenedElement { get; set; }
    }
}
