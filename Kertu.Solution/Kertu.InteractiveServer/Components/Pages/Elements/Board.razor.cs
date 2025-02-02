﻿using Kertu.InteractiveServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Board : ComponentBase
    {
        private readonly Func<Models.Card, RadzenDropZone<Models.Card>, bool> _itemSelector = (item, zone) =>
            item.ParentID == (int?)zone.Value;

        private string _title = string.Empty;
        private IList<Models.Element>? _elements;
        private IList<Models.Card>? _childrenCards;

        [Parameter]
        public required string Id { get; set; }

        private int IdValue => int.Parse(Id);

        [Inject]
        private ApplicationDbContext DbContext { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        protected override void OnInitialized()
        {
            var board = DbContext
                .Boards.Include(b => b.Children) // Include immediate children
                .ThenInclude(child => (child as Models.List).Children) // Include children of child Lists
                .Single(b => b.Id == IdValue);

            _title = board.Name;
            _elements = board.Children.OrderBy(e => e.Position).ToList();
            _childrenCards = _elements
                .OfType<Models.List>() // Filter elements to only those of type List
                .SelectMany(list => list.Children) // Select all children (Cards) from each List
                .OrderBy(e => e.Position)
                .ToList(); // Convert the result to a List<Models.Card>
        }

        private void OnElementClick(Models.Element element)
        {
            if (element is Models.Card card)
            {
                NavigationManager.NavigateTo($"/card/{card.Id}", true);
            }
            else if (element is Models.Board board)
            {
                NavigationManager.NavigateTo($"/board/{board.Id}", true);
            }
        }

        private void OnDrop(RadzenDropZoneItemEventArgs<Models.Card> args)
        {
            // Update item zone
            DbContext.Elements.Single(e => e.Id == args.Item.Id).ParentID = null;
            DbContext.Elements.Single(e => e.Id == args.Item.Id).ListID = null;
            DbContext.Elements.Single(e => e.Id == args.Item.Id).BoardID = null;
            if (DbContext.Elements.Any(e => e.ParentID == (int?)args.ToZone.Value))
            {
                args.Item.Position =
                    DbContext.Elements.Where(e => e.ParentID == (int?)args.ToZone.Value).OrderBy(e => e.Position).Last().Position + 1;
            }
            DbContext.Elements.Single(e => e.Id == args.Item.Id).AddTo(DbContext.Elements.Single(b => b.Id == (int?)args.ToZone.Value));

            DbContext.SaveChanges();
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}
