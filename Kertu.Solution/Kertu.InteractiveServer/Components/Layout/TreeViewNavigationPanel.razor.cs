using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models;
using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

namespace Kertu.InteractiveServer.Components.Layout
{
    public partial class TreeViewNavigationPanel : ComponentBase
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        readonly List<TreeViewItem> _treeViewItems = [];
        RadzenTree _tree = new();
        object _selection;

        ApplicationUser _currentUser;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            HttpContext = httpContextAccessor.HttpContext;
            _currentUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

            var elements = dbContext.Users.Include(u => u.UserElements).Single(u => u == _currentUser).UserElements;
            foreach (var element in elements)
            {
                TreeViewItem treeViewItem = new(element);
                var fromDB = dbContext.Elements.Where(ke => ke == element);
                treeViewItem.Children = LoadTree(fromDB);
                _treeViewItems.Add(treeViewItem);
            }
        }

        private void OnChange()
        {
            TreeViewItem item = _selection as TreeViewItem;

            if (item.Element is Card card)
            {
                NavigationManager.NavigateTo($"/card/{card.Id}", true);
            }
            else if (item.Element is List)
            {
                //clicked list
            }
            else if (item.Element is Board)
            {
                //clicked card board
            }
        }

        void TreeItemContextMenu(TreeItemContextMenuEventArgs args)
        {
            ContextMenuService.Open(args,
            [
                new ContextMenuItem(){ Text = "Add card", Value = 11, Icon = "post_add" },
                new ContextMenuItem(){ Text = "Add list", Value = 12, Icon = "splitscreen_add" },
                new ContextMenuItem(){ Text = "Add board", Value = 13, Icon = "dashboard_customize" },
                new ContextMenuItem(){ Text = "Rename", Value = 2, Icon = "edit" },
                new ContextMenuItem(){ Text = "Delete", Value = 3, Icon = "delete_forever" },
                                    ],
            async (e) =>
            {
                switch (e.Value)
                {
                    case 11:

                        break;

                    case 12:

                        break;

                    case 13:

                        break;

                    case 2:
                        await RenameElement((args.Value as TreeViewItem).Element);
                        break;

                    case 3:
                        await DeleteElement((args.Value as TreeViewItem).Element);
                        break;
                }
            }
         );
        }

        private async Task DeleteElement(Element element)
        {
            bool? result = await DialogService.Confirm($"Are you sure to delete {element.Name} and all of its children?", "Confirm", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnOverlayClick = true, CloseDialogOnEsc = true });

            if (result == true)
            {
                CascadeDelete(dbContext.Elements.Where(ke => ke.Id == element.Id));

                NavigationManager.Refresh(true);
            }
        }
        void CascadeDelete(IQueryable<Element> parent)
        {
            List<Element> children = [];
            if (parent.First() is List)
            {
                var list = parent.Cast<List>();
                children = list.Include(kl => kl.Children).First().Children.Cast<Element>().ToList();
            }
            else if (parent.First() is Board)
            {
                var board = parent.Cast<Board>();
                children = board.Include(kl => kl.Children).First().Children.Cast<Element>().ToList();
            }

            foreach (var child in children)
            {
                var fromDB = dbContext.Elements.Where(ke => ke == child);
                CascadeDelete(fromDB);
            }
            dbContext.Remove(parent.First());
            dbContext.SaveChanges();
        }

        private List<TreeViewItem> LoadTree(IQueryable<Element> parent)
        {
            List<TreeViewItem> treeViewItems = [];
            List<Element> children = [];
            if (parent.First() is List)
            {
                var list = parent.Cast<List>();
                children = list.Include(kl => kl.Children).First().Children.Cast<Element>().ToList();
            }
            else if (parent.First() is Board)
            {
                var board = parent.Cast<Board>();
                children = board.Include(kl => kl.Children).First().Children.Cast<Element>().ToList();
            }

            foreach (var child in children)
            {
                TreeViewItem item = new(child);
                var fromDB = dbContext.Elements.Where(ke => ke == child);
                item.Children = LoadTree(fromDB);
                treeViewItems.Add(item);
            }

            return treeViewItems;
        }
    }
}
