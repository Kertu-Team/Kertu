using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.KertuElements;
using Microsoft.AspNetCore.Components;

namespace Kertu.InteractiveServer.Components.KertuElements
{
    public partial class CardView : ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }
        int IdValue => int.Parse(Id ?? "");

        [Inject]
        ApplicationDbContext? DbContext { get; set; }

        string? _title;
        string? _description;
        bool _busy;

        protected override void OnInitialized()
        {
            KertuCard? card = DbContext?.KertuElements.Find(IdValue) as KertuCard;
            _title = card?.Name;
            _description = card?.Description;
        }

        async Task OnBusyClick()
        {
            _busy = true;
            await Task.Delay(2000);
            _busy = false;
        }
    }
}
