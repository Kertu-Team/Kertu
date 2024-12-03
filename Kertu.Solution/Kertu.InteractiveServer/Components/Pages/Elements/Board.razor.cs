﻿using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Board : ComponentBase
    {
        private readonly Func<MyTask, RadzenDropZone<MyTask>, bool> _itemSelector = (item, zone) =>
            item.Status == (Status)zone.Value && item.Status != Status.Deleted;

        private readonly Func<RadzenDropZoneItemEventArgs<MyTask>, bool> _canDrop = request =>
            request.FromZone == request.ToZone
            || (Status)request.ToZone.Value == Status.Deleted
            || Math.Abs((int)request.Item.Status - (int)request.ToZone.Value) == 1;

        private IList<MyTask>? _data;

        public enum Status
        {
            NotStarted,
            Started,
            Completed,
            Deleted,
        }

        [Parameter]
        public required string Id { get; set; }

        private int IdValue => int.Parse(Id);

        protected override void OnInitialized()
        {
            _data = Enumerable
                .Range(0, 5)
                .Select(i => new MyTask()
                {
                    Id = i,
                    Name = $"Task{i}",
                    Status = i < 3 ? Status.NotStarted : Status.Started,
                })
                .ToList();
        }

        private static void OnItemRender(RadzenDropZoneItemRenderEventArgs<MyTask> args)
        {
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

            args.Visible = args.Item.Status != Status.Deleted;
        }

        private void OnDrop(RadzenDropZoneItemEventArgs<MyTask> args)
        {
            if (args.FromZone != args.ToZone)
            {
                args.Item.Status = (Status)args.ToZone.Value;
            }

            if (args.ToItem != null && args.ToItem != args.Item)
            {
                _data.Remove(args.Item);
                _data.Insert(_data.IndexOf(args.ToItem), args.Item);
            }
        }

        private void CreateItem()
        {
            _data.Add(
                new MyTask()
                {
                    Id = _data.Max(t => t.Id) + 1,
                    Name = "New Task",
                    Status = Status.NotStarted,
                }
            );
        }

        public class MyTask
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public Status Status { get; set; } = Status.NotStarted;
        }
    }
}
