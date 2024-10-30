using Microsoft.AspNetCore.Components;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Card : ComponentBase
    {
        [Parameter]
        public required string Id { get; set; }
        int IdValue => int.Parse(Id);

        Models.Card? _card;
        string _title = string.Empty;
        string _description = string.Empty;
        bool _busy;

        protected override void OnInitialized()
        {
            _card = dbContext.Elements.Find(IdValue) as Models.Card;
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
            _ = await Task.Run(dbContext.SaveChanges);
            notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Duration = 4000 });
            _busy = false;
        }
    }
}
