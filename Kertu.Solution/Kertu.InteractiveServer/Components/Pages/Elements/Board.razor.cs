using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Board : ComponentBase
    {
        // The new task name entered by the user
        private string? _newTaskName;
        // Filter items by zone value
        Func<MyTask, RadzenDropZone<MyTask>, bool> _itemSelector = (item, zone) => item.Status == (Status)zone.Value && item.Status != Status.Deleted;
        Func<RadzenDropZoneItemEventArgs<MyTask>, bool> _canDrop = request =>
            // Allow item drop only in the same zone, in "Deleted" zone or in the next/previous zone.
            request.FromZone == request.ToZone || (Status)request.ToZone.Value == Status.Deleted ||
                   Math.Abs((int)request.Item.Status - (int)request.ToZone.Value) == 1;
        IList<MyTask> _data;

        void OnItemRender(RadzenDropZoneItemRenderEventArgs<MyTask> args)
        {
            // Customize item appearance
            if (args.Item.Name == "Task2")
            {
                args.Attributes["draggable"] = "false";
                args.Attributes["style"] = "cursor:not-allowed";
                args.Attributes["class"] = "rz-card rz-variant-flat rz-background-color-primary-lighter rz-color-on-primary-lighter";
            }
            else
            {
                args.Attributes["class"] = "rz-card rz-variant-filled rz-background-color-primary-light rz-color-on-primary-light";
            }

            // Do not render item if deleted
            args.Visible = args.Item.Status != Status.Deleted;
        }

        async Task OnDrop(RadzenDropZoneItemEventArgs<MyTask> args)
        {
            if (args.FromZone != args.ToZone)
            {
                // Update item zone
                args.Item.Status = (Status)args.ToZone.Value;
                await SaveTasksToLocalStorage();
            }

            if (args.ToItem != null && args.ToItem != args.Item)
            {
                // Reorder items in same zone or place the item at specific index in new zone
                _data.Remove(args.Item);
                _data.Insert(_data.IndexOf(args.ToItem), args.Item);
                await SaveTasksToLocalStorage();
            }
        }

        protected override void OnInitialized()
        {
            _data = Enumerable.Range(0, 5)
                .Select(i =>
                    new MyTask()
                    {
                        Id = i,
                        Name = $"Task{i}",
                        Status = i < 3 ? Status.NotStarted : Status.Started
                    })
                .ToList();
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadTasksFromLocalStorage();
        }

        private async Task CreateItem()
        {
            // Create a new task with the user-provided name
            if (!string.IsNullOrWhiteSpace(_newTaskName))
            {
                _data.Add(new MyTask()
                {
                    Id = _data.Max(t => t.Id) + 1,
                    Name = _newTaskName, // Use the entered task name
                    Status = Status.NotStarted
                });

                // Clear the input field after creating the task
                _newTaskName = string.Empty;
                await SaveTasksToLocalStorage();
            }
        }
        private async Task SaveTasksToLocalStorage()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.clear");  // Clear localStorage before saving updated data

            for (int i = 0; i < _data.Count; i++)
            {
                var task = _data[i];
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", $"task_name_{i}", task.Name);
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", $"task_status_{i}", task.Status.ToString());
            }

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "task_count", _data.Count.ToString());
        }

        private async Task LoadTasksFromLocalStorage()
        {

            string taskCountString = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "task_count");
            if (int.TryParse(taskCountString, out int taskCount) && taskCount > 0)
            {
                _data = [];

                for (int i = 0; i < taskCount; i++)
                {
                    string name = await jsRuntime.InvokeAsync<string>("localStorage.getItem", $"task_name_{i}");
                    string statusString = await jsRuntime.InvokeAsync<string>("localStorage.getItem", $"task_status_{i}");
                    if (Enum.TryParse(statusString, out Status status))
                    {
                        _data.Add(new MyTask
                        {
                            Id = i,
                            Name = name,
                            Status = status
                        });
                    }
                }
            }
            else
            {
                // Initialize empty list if no tasks are found in localStorage
                _data = [];
            }
        }

        public class MyTask
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Status Status { get; set; } = Status.NotStarted;
        }

        public enum Status
        {
            NotStarted,
            Started,
            Completed,
            Deleted
        }
    }
}
