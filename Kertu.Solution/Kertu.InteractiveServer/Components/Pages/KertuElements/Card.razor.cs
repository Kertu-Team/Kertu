using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.KertuElements;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Kertu.InteractiveServer.Components.Pages.KertuElements
{
    public partial class Card : ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }
        int IdValue => int.Parse(Id ?? "");

        [Inject]
        ApplicationDbContext? DbContext { get; set; }
        [Inject]
        NotificationService? NotificationService { get; set; }

        KertuCard? _card;
        string _title = string.Empty;
        string _description = string.Empty;
        bool _busy;

        protected override void OnInitialized()
        {
            _card = DbContext?.KertuElements.Find(IdValue) as KertuCard;
            if (_card is null)
            {
                return;
            }
            _title = _card.Name;
            _description = _card.Description;
        }

        async Task OnBusyClick()
        {
            if (_card is null)
            {
                return;
            }
            _busy = true;
            _card.Name = _title;
            _card.Description = _description;
            await Task.Run(() => DbContext?.SaveChanges());
            NotificationService?.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Duration = 4000 });
            _busy = false;
        }
    }
}
