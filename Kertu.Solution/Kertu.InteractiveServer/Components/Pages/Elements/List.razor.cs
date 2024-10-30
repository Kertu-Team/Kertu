using Microsoft.AspNetCore.Components;
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
        List<Models.Card> _completed = [];

        protected override void OnInitialized()
        {
            //Models.List list = dbContext.Elements.Find(IdValue) as Models.List;
            // _title = 
            List<Models.Card> list = [.. dbContext.Cards.Where(kc => kc.Name != null)];

            // Separate active and completed lists
            _active = list//.Children
                .Where(card => !card.IsTask || !card.IsCompleted)
                .ToList();

            _completed = list//.Children
                .Where(card => card.IsTask && card.IsCompleted)
                .ToList();
        }

        private async Task MarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = true;
            dbContext.Cards.Update(task);
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _active.Remove(task);
            _completed.Add(task);
        }

        private async Task UnmarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = false;
            dbContext.Cards.Update(task);
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _completed.Remove(task);
            _active.Add(task);
        }

        private async Task Open(Models.Card card)
        {
            await Task.Run(() => navigationManager.NavigateTo($"/card/{card.Id}", true));
        }
    }
}
