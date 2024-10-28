using Microsoft.AspNetCore.Components;

namespace Kertu.InteractiveServer.Components.KertuElements
{
    public partial class CardView : ComponentBase
    {
        string _descriptionValue = string.Empty;
        bool _busy;

        async Task OnBusyClick()
        {
            _busy = true;
            await Task.Delay(2000);
            _busy = false;
        }
    }
}
