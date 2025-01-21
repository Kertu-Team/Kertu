using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models;
using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Board : ComponentBase
    {
        private readonly Func<Models.Element, RadzenDropZone<Models.Element>, bool> _itemSelector = (item, zone) =>
            item.ParentID == (int?)zone.Value;

        private string _title = string.Empty;
        private IList<Models.Element>? _elements;

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
            _title = DbContext.Boards.Find(IdValue).Name;
            _elements = DbContext.Boards.Include(b => b.Children).Single(b => b.Id == IdValue).Children;
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

        private void CreateItem() { }
    }
}
