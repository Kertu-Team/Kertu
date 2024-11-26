using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class List : ComponentBase
    {
        private string _title = string.Empty;
        private string _newCardName = string.Empty;
        private List<Models.Card> _active = [];
        private List<Models.TaskCard> _completed = [];

        // Field to track the visibility of completed tasks
        private bool _showCompleted = false;

        [Parameter]
        public required string Id { get; set; }

        private int IdValue => int.Parse(Id);

        protected override void OnInitialized()
        {
            Models.List list = dbContext.Lists.Include(l => l.Children).Single(l => l.Id == IdValue);
            _title = list.Name;

            // Separate active and completed cards
            _active = list.Children.Where(card => card is not TaskCard taskCard || !taskCard.IsCompleted).ToList();
            _completed = list.Children.OfType<TaskCard>().Where(task => task.IsCompleted).ToList();
        }

        private async Task MarkAsCompleted(Models.TaskCard task)
        {
            task.IsCompleted = true;
            await dbContext.SaveChangesAsync();

            _active.Remove(task);
            _completed.Add(task);
        }

        private async Task UnmarkAsCompleted(Models.TaskCard task)
        {
            task.IsCompleted = false;
            await dbContext.SaveChangesAsync();

            _completed.Remove(task);
            _active.Add(task);
        }

        private async Task Open(Models.Card card)
        {
            await Task.Run(() => navigationManager.NavigateTo($"/card/{card.Id}", true));
        }

        private async Task AddNewCard()
        {
            if (!string.IsNullOrWhiteSpace(_newCardName))
            {
                Models.Card newCard = new() { Name = _newCardName, Description = string.Empty };

                // Get the current list
                Models.List list = dbContext.Lists.Include(l => l.Children).Single(l => l.Id == IdValue);

                // Add the new card to the list
                list.Children.Add(newCard);

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                // Clear the input field
                _newCardName = string.Empty;

                // Update the local collections
                _active.Add(newCard);

                // Refresh the UI
                StateHasChanged();
            }
        }

        private async Task OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await AddNewCard();
            }
        }

        // Method to toggle the visibility of completed tasks
        private void ToggleCompleted()
        {
            _showCompleted = !_showCompleted;
        }
    }
}
