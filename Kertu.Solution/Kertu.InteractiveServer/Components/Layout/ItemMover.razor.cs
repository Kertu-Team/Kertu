using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models;
using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Linq;

namespace Kertu.InteractiveServer.Components.Layout
{
    public partial class ItemMover : ComponentBase
    {
        [Parameter]
        public int ElementId { get; set; }

        Element Element;
        
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

            Element = dbContext.Elements.Single(e => e.Id == ElementId);

            TreeViewItem parent = new TreeViewItem(null);
            parent.Name = _currentUser.UserName;
            _treeViewItems.Add(parent);

            var elements = dbContext
                .Users.Include(u => u.UserElements)
                .Single(u => u == _currentUser)
                .UserElements.OrderBy(e => e.Position);
            foreach (var element in elements)
            {
                if ((Element is Card && element is not Card) || (Element is not Card && element is Board))
                {
                    TreeViewItem treeViewItem = new(element);
                    var fromDB = dbContext.Elements.Where(ke => ke == element);
                    treeViewItem.Children = LoadTree(fromDB);
                    _treeViewItems.First().Children.Add(treeViewItem);
                }
            }
        }

        private bool ShouldExpand(object data)
        {
            if (UserStateService.GetLastOpenedElement(_currentUser.Id) != null)
            {
                var element = (data as TreeViewItem).Element;
                if (element == null)
                    return true;
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

            if (Element.ApplicationUserId != null)
                Element.ApplicationUserId = null;

            dbContext.Elements.Single(e => e.Id == Element.Id).ParentID = null;
            dbContext.Elements.Single(e => e.Id == Element.Id).ListID = null;
            dbContext.Elements.Single(e => e.Id == Element.Id).BoardID = null;
            if (item.Element == null)
                dbContext.Users.Single(u => u.Id == _currentUser.Id).UserElements.Add(dbContext.Elements.Single(e => e.Id == Element.Id));
            else
                dbContext.Elements.Single(e => e.Id == Element.Id).AddTo(dbContext.Elements.Single(e => e.Id == item.Element.Id));

            dbContext.SaveChanges();
            DialogService.Close(true);
        }

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
                if ((Element is Card && child is not Card) || (Element is not Card && child is Board))
                {
                    TreeViewItem item = new(child);
                    var fromDB = dbContext.Elements.Where(ke => ke == child).OrderBy(e => e.Position);
                    item.Children = LoadTree(fromDB);
                    treeViewItems.Add(item);
                }
            }

            return treeViewItems;
        }
    }
}