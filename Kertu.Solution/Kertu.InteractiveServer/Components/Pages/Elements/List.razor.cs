using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class List : ComponentBase
    {
        List<Models.Card> _activeTasks = [];
        List<Models.Card> _completedTasks = [];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Fetch and filter tasks based on completion status
            var allTasks = await dbContext.Cards.Where(kc => kc.Name != null).ToListAsync();
            _activeTasks = allTasks.Where(t => !t.IsCompleted).ToList();
            _completedTasks = allTasks.Where(t => t.IsCompleted).ToList();
        }

        private async Task MarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = true;
            dbContext.Cards.Update(task);
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _activeTasks.Remove(task);
            _completedTasks.Add(task);
        }

        private async Task UnmarkAsCompleted(Models.Card task)
        {
            // Update the task's completion status
            task.IsCompleted = false;
            dbContext.Cards.Update(task);
            await dbContext.SaveChangesAsync();

            // Refresh the lists
            _completedTasks.Remove(task);
            _activeTasks.Add(task);
        }
    }
}
