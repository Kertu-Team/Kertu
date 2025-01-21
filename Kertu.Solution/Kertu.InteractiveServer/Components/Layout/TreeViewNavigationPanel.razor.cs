using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models;
using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

namespace Kertu.InteractiveServer.Components.Layout
{
    public partial class TreeViewNavigationPanel : ComponentBase
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        readonly List<TreeViewItem> _treeViewItems = new();
        RadzenTree _tree = new();
        object _selection;

        ApplicationUser _currentUser;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HttpContext = httpContextAccessor.HttpContext;
            var userName = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (userName != null)
            {
                _currentUser = dbContext.Users.Single(u => u.UserName == userName);
            }

            if (_currentUser == null)
            {
                return;
            }

            var elements = dbContext
                .Users.Include(u => u.UserElements)
                .Single(u => u == _currentUser)
                .UserElements.OrderBy(e => e.Position);
            foreach (var element in elements)
            {
                TreeViewItem treeViewItem = new(element);
                var fromDB = dbContext.Elements.Where(ke => ke == element);
                treeViewItem.Children = LoadTree(fromDB);
                _treeViewItems.Add(treeViewItem);
            }
        }

        private bool ShouldExpand(object data)
        {
            if (UserStateService.GetLastOpenedElement(_currentUser.Id) != null)
            {
                var element = (data as TreeViewItem).Element;
                return SearchChildren(dbContext.Elements.Where(e => e.Id == element.Id));
            }
            return false;
        }

        private bool SearchChildren(IQueryable<Element> parent)
        {
            List<Element> children = new();
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
                if (child.Id == UserStateService.GetLastOpenedElement(_currentUser.Id).Id)
                    return true;
                else
                {
                    var fromDB = dbContext.Elements.Where(ke => ke == child);
                    if (SearchChildren(fromDB))
                        return true;
                }
            }
            return false;
        }

        private void OnChange()
        {
            TreeViewItem item = _selection as TreeViewItem;

            UserStateService.SetLastOpenedElement(_currentUser.Id, item.Element);

            if (item.Element is Card card)
            {
                NavigationManager.NavigateTo($"/card/{card.Id}", true);
            }
            else if (item.Element is List list)
            {
                NavigationManager.NavigateTo($"/list/{list.Id}", true);
            }
            else if (item.Element is Board board)
            {
                NavigationManager.NavigateTo($"/board/{board.Id}", true);
            }
        }

        void NavigationPanelContextMenu(MouseEventArgs args)
        {
            Action<MenuItemEventArgs> action = async (e) =>
            {
                switch (e.Value)
                {
                    case 11:
                        await AddElementDialog(null, typeof(Card));
                        break;

                    case 12:
                        await AddElementDialog(null, typeof(List));
                        break;

                    case 13:
                        await AddElementDialog(null, typeof(Board));
                        break;
                }
            };
            List<ContextMenuItem> items = new();
            items =
            [
                new ContextMenuItem()
                {
                    Text = "Add card",
                    Value = 11,
                    Icon = "post_add",
                },
                new ContextMenuItem()
                {
                    Text = "Add list",
                    Value = 12,
                    Icon = "splitscreen_add",
                },
                new ContextMenuItem()
                {
                    Text = "Add board",
                    Value = 13,
                    Icon = "dashboard_customize",
                },
            ];

            ContextMenuService.Open(args, items, action);
        }

        void MoveElementUp(Element element)
        {
            if (element.ParentID == null)
            {
                int index = dbContext
                    .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                    .OrderBy(e => e.Position)
                    .ToList()
                    .IndexOf(element);

                if (index > 0)
                {
                    int higherId = dbContext
                        .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                        .OrderBy(e => e.Position)
                        .ToList()[index - 1].Id;

                    int currentId = dbContext
                        .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                        .OrderBy(e => e.Position)
                        .ToList()[index].Id;

                    int higher = dbContext.Elements.Single(e => e.Id == higherId).Position;

                    dbContext.Elements.Single(e => e.Id == higherId).Position = element.Position;

                    dbContext.Elements.Single(e => e.Id == currentId).Position = higher;

                    dbContext.SaveChanges();
                }

                NavigationManager.Refresh(true);
            }
            else
            {
                int index = dbContext
                    .Elements.Single(e => e.Id == element.ParentID)
                    .GetChildren()
                    .OrderBy(e => e.Position)
                    .ToList()
                    .IndexOf(element);

                if (index > 0)
                {
                    int higherId = dbContext
                        .Elements.Single(e => e.Id == element.ParentID)
                        .GetChildren()
                        .OrderBy(e => e.Position)
                        .ToList()[index - 1].Id;

                    int currentId = dbContext
                        .Elements.Single(e => e.Id == element.ParentID)
                        .GetChildren()
                        .OrderBy(e => e.Position)
                        .ToList()[index].Id;

                    int higher = dbContext.Elements.Single(e => e.Id == higherId).Position;

                    dbContext.Elements.Single(e => e.Id == higherId).Position = element.Position;

                    dbContext.Elements.Single(e => e.Id == currentId).Position = higher;

                    dbContext.SaveChanges();
                }
                NavigationManager.Refresh(true);
            }

        }

        void MoveElementDown(Element element)
        {

            if (element.ParentID == null)
            {
                int index = dbContext
                    .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                    .OrderBy(e => e.Position)
                    .ToList()
                    .IndexOf(element);

                int count = dbContext
                    .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                    .OrderBy(e => e.Position)
                    .ToList()
                    .Count();

                if (index < count - 1)
                {
                    int lowerId = dbContext
                        .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                        .OrderBy(e => e.Position)
                        .ToList()[index + 1].Id;

                    int currentId = dbContext
                        .Elements.Where(e => e.ApplicationUserId == _currentUser.Id)
                        .OrderBy(e => e.Position)
                        .ToList()[index].Id;

                    int lower = dbContext.Elements.Single(e => e.Id == lowerId).Position;

                    dbContext.Elements.Single(e => e.Id == lowerId).Position = element.Position;

                    dbContext.Elements.Single(e => e.Id == currentId).Position = lower;

                    dbContext.SaveChanges();
                }
                NavigationManager.Refresh(true);
            }
            else
            {
                int index = dbContext
                    .Elements.Single(e => e.Id == element.ParentID)
                    .GetChildren()
                    .OrderBy(e => e.Position)
                    .ToList()
                    .IndexOf(element);

                int count = dbContext
                    .Elements.Single(e => e.Id == element.ParentID)
                    .GetChildren()
                    .OrderBy(e => e.Position)
                    .ToList()
                    .Count();

                if (index < count - 1)
                {
                    int lowerId = dbContext
                        .Elements.Single(e => e.Id == element.ParentID)
                        .GetChildren()
                        .OrderBy(e => e.Position)
                        .ToList()[index + 1].Id;

                    int currentId = dbContext
                        .Elements.Single(e => e.Id == element.ParentID)
                        .GetChildren()
                        .OrderBy(e => e.Position)
                        .ToList()[index].Id;

                    int lower = dbContext.Elements.Single(e => e.Id == lowerId).Position;

                    dbContext.Elements.Single(e => e.Id == lowerId).Position = element.Position;

                    dbContext.Elements.Single(e => e.Id == currentId).Position = lower;

                    dbContext.SaveChanges();
                }
                NavigationManager.Refresh(true);
            }
        }

        void TreeItemContextMenu(TreeItemContextMenuEventArgs args)
        {
            Action<MenuItemEventArgs> action = async (e) =>
            {
                switch (e.Value)
                {
                    case 11:
                        await AddElementDialog((args.Value as TreeViewItem).Element, typeof(Card));
                        break;

                    case 12:
                        await AddElementDialog((args.Value as TreeViewItem).Element, typeof(List));
                        break;

                    case 13:
                        await AddElementDialog((args.Value as TreeViewItem).Element, typeof(Board));
                        break;

                    case 2:
                        await RenameElement((args.Value as TreeViewItem).Element);
                        break;

                    case 3:
                        await DeleteElement((args.Value as TreeViewItem).Element);
                        break;

                    case 41:
                        MoveElementUp((args.Value as TreeViewItem).Element);
                        break;
                    case 42:
                        MoveElementDown((args.Value as TreeViewItem).Element);
                        break;
                    case 43:

                        break;
                }
            };

            List<ContextMenuItem> items = new();

            if ((args.Value as TreeViewItem).Element is Card)
            {
                items =
                [
                    new ContextMenuItem()
                    {
                        Text = "Rename",
                        Value = 2,
                        Icon = "edit",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Delete",
                        Value = 3,
                        Icon = "delete_forever",
                    },
                    new ContextMenuItem() { Disabled = true },
                    new ContextMenuItem()
                    {
                        Text = "Move Up",
                        Value = 41,
                        Icon = "arrow_upward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move Down",
                        Value = 42,
                        Icon = "arrow_downward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move...",
                        Value = 43,
                        Icon = "arrow_outward",
                    },
                ];
            }
            else if ((args.Value as TreeViewItem).Element is List)
            {
                items =
                [
                    new ContextMenuItem()
                    {
                        Text = "Add card",
                        Value = 11,
                        Icon = "post_add",
                    },
                    new ContextMenuItem() { Disabled = true },
                    new ContextMenuItem()
                    {
                        Text = "Move Up",
                        Value = 41,
                        Icon = "arrow_upward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move Down",
                        Value = 42,
                        Icon = "arrow_downward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move...",
                        Value = 43,
                        Icon = "arrow_outward",
                    },
                    new ContextMenuItem() { Disabled = true },
                    new ContextMenuItem()
                    {
                        Text = "Rename",
                        Value = 2,
                        Icon = "edit",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Delete",
                        Value = 3,
                        Icon = "delete_forever",
                    },
                ];
            }
            else if ((args.Value as TreeViewItem).Element is Board)
            {
                items =
                [
                    new ContextMenuItem()
                    {
                        Text = "Add card",
                        Value = 11,
                        Icon = "post_add",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Add list",
                        Value = 12,
                        Icon = "splitscreen_add",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Add board",
                        Value = 13,
                        Icon = "dashboard_customize",
                    },
                    new ContextMenuItem() { Disabled = true },
                    new ContextMenuItem()
                    {
                        Text = "Move Up",
                        Value = 41,
                        Icon = "arrow_upward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move Down",
                        Value = 42,
                        Icon = "arrow_downward",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Move...",
                        Value = 43,
                        Icon = "arrow_outward",
                    },
                    new ContextMenuItem() { Disabled = true },
                    new ContextMenuItem()
                    {
                        Text = "Rename",
                        Value = 2,
                        Icon = "edit",
                    },
                    new ContextMenuItem()
                    {
                        Text = "Delete",
                        Value = 3,
                        Icon = "delete_forever",
                    },
                ];
            }

            ContextMenuService.Open(args, items, action);
        }

        private void AddElement(Element parent, Type type, string name)
        {
            Element element;
            if (type == typeof(Card))
            {
                element = new Card();
            }
            else if (type == typeof(List))
            {
                element = new List();
            }
            else
            {
                element = new Board();
            }

            element.Name = name;
            element.Owner = _currentUser;

            dbContext.Add(element);
            dbContext.SaveChanges();

            if (parent == null)
            {
                if(_currentUser.UserElements.Count > 0)
                {
                    element.Position = _currentUser.UserElements.OrderBy(e => e.Position).Last().Position + 1;
                }

                _currentUser.UserElements.Add(element);
            }
            else
            {
                element.AddTo(parent);
            }
            dbContext.SaveChanges();
            UserStateService.SetLastOpenedElement(_currentUser.Id, element);
            NavigationManager.Refresh(true);
        }

        private async Task DeleteElement(Element element)
        {
            bool? result = await DialogService.Confirm(
                $"Are you sure to delete {element.Name} and all of its children?",
                "Confirm",
                new ConfirmOptions()
                {
                    OkButtonText = "Yes",
                    CancelButtonText = "No",
                    CloseDialogOnOverlayClick = true,
                    CloseDialogOnEsc = true,
                }
            );

            if (result == true)
            {
                CascadeDelete(dbContext.Elements.Where(ke => ke.Id == element.Id));

                NavigationManager.Refresh(true);
            }
        }

        void CascadeDelete(IQueryable<Element> parent)
        {
            List<Element> children = new();
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

        Random random = new();

        private List<TreeViewItem> LoadTree(IQueryable<Element> parent)
        {
            List<TreeViewItem> treeViewItems = new();
            List<Element> children = new();
            if (parent.First() is List)
            {
                var list = parent.Cast<List>();
                children = list.Include(kl => kl.Children).First().Children.Cast<Element>().OrderBy(e => e.Position).ToList();
            }
            else if (parent.First() is Board)
            {
                var board = parent.Cast<Board>();
                children = board.Include(kl => kl.Children).First().Children.Cast<Element>().OrderBy(e => e.Position).ToList();
            }

            foreach (var child in children)
            {
                if (child.Position == 0)
                    child.Position = children.OrderBy(e => e.Position).First().Position - 1;
                TreeViewItem item = new(child);
                var fromDB = dbContext.Elements.Where(ke => ke == child).OrderBy(e => e.Position);
                item.Children = LoadTree(fromDB);
                treeViewItems.Add(item);
            }

            return treeViewItems;
        }
    }
}
