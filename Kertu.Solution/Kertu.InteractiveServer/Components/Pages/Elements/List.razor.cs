using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class List : ComponentBase
    {
        [Parameter]
        public required string Id { get; set; }
        int IdValue => int.Parse(Id);

        string _title = string.Empty;
        List<Models.Card> _active = [];
        List<Models.TaskCard> _completed = [];

        protected override void OnInitialized()
        {
            Models.List list = dbContext.Lists.Include(l => l.Children).Single(l => l.Id == IdValue);
            _title = list.Name;

            // Separate active cards (includes both TaskCard and Card) and completed task cards
            _active = list.Children.Where(card => card is not TaskCard taskCard || !taskCard.IsCompleted).ToList();

            _completed = list.Children.OfType<TaskCard>().Where(task => task.IsCompleted).ToList();
        }

        async Task MarkAsCompleted(Models.TaskCard task)
        {
            task.IsCompleted = true;
            await dbContext.SaveChangesAsync();

            _active.Remove(task);
            _completed.Add(task);
        }

        async Task UnmarkAsCompleted(Models.TaskCard task)
        {
            task.IsCompleted = false;
            await dbContext.SaveChangesAsync();

            _completed.Remove(task);
            _active.Add(task);
        }

        async Task Open(Models.Card card)
        {
            await Task.Run(() => navigationManager.NavigateTo($"/card/{card.Id}", true));
        }
    }
}
