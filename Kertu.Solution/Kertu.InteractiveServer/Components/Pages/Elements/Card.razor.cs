using Microsoft.AspNetCore.Components;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Card : ComponentBase
    {
        private Models.Card? _card;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private bool _busy;

        [Parameter]
        public required string Id { get; set; }

        private int IdValue => int.Parse(Id);

        protected override void OnInitialized()
        {
            _card = dbContext.Cards.Find(IdValue);
            if (_card is null)
            {
                return;
            }

            _title = _card.Name;
            _description = _card.Description;
        }

        private async Task OnBusyClick()
        {
            if (_card is null)
            {
                return;
            }

            _busy = true;
            _card.Name = _title;
            _card.Description = _description;
            _ = await Task.Run(dbContext.SaveChanges);
            notificationService.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Duration = 4000,
                }
            );
            _busy = false;
        }
    }
}
