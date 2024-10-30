using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class List : ComponentBase
    {
        string _title = string.Empty;
        List<Models.Card> _active = [];
        List<Models.Card> _completed = [];

        [Parameter]
        public required string Id { get; set; }
        int IdValue => int.Parse(Id);

        protected override async Task OnInitializedAsync()
        {
            Models.List list = dbContext.Lists.Include(l => l.Children).Single(l => l.Id == IdValue);
            _title = list.Name;

            // Separate active and completed lists
            _active = list.Children
                .Where(card => !card.IsTask || !card.IsCompleted)
                .ToList();

            _completed = list.Children
                .Where(card => card.IsTask && card.IsCompleted)
                .ToList();
        }

        async Task MarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = true;
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _active.Remove(task);
            _completed.Add(task);
        }

        async Task UnmarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = false;
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _completed.Remove(task);
            _active.Add(task);
        }

        async Task Open(Models.Card card)
        {
            await Task.Run(() => navigationManager.NavigateTo($"/card/{card.Id}", true));
        }
    }
}
