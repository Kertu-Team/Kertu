using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models.Elements;
using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Kertu.InteractiveServer.Components.Search
{
    public partial class Search : ComponentBase
    {
        private string _userID;

        public string SearchQuery { get; set; } = string.Empty;

        public SearchScope CurrentScope { get; set; } = SearchScope.Global;

        public List<Element> SearchResults { get; set; } = [];

        [Inject] private ApplicationDbContext DbContext { get; set; } = default!;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] private UserStateService UserStateService { get; set; } = default!;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await SetCurrentUserID();
        }

        protected async Task OnSearchInput()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchResults.Clear();
                return;
            }

            int? parentId = CurrentScope == SearchScope.Children
                ? UserStateService.GetLastOpenedElement(_userID)?.Id
                : null;

            SearchResults = await SearchAsync(SearchQuery, CurrentScope, parentId);
        }

        private async Task SetCurrentUserID()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                _userID = user.Identity.Name;
            }
        }

        private async Task<List<Element>> SearchAsync(string query, SearchScope scope, int? parentId = null)
        {
            IQueryable<Element> searchQuery = DbContext.Elements;

            // Filtrowanie po parentId dla zakresu Children
            if (scope == SearchScope.Children && parentId.HasValue)
            {
                searchQuery = searchQuery.Where(e => e.ParentID == parentId);
            }

            // Wyszukiwanie po Name dla wszystkich elementów
            var nameResults = await searchQuery
                .Where(e => EF.Functions.Like(e.Name, $"%{query}%"))
                .ToListAsync();

            // Wyszukiwanie po Description tylko dla Card
            var descriptionResults = await DbContext.Elements
                .OfType<Card>() // Wybiera tylko elementy typu Card
                .Where(c => EF.Functions.Like(c.Description, $"%{query}%"))
                .ToListAsync();

            // Połączenie wyników
            return nameResults.Union(descriptionResults).ToList();
        }

        private void NavigateToElement(Element element)
        {
            switch (element)
            {
                case Card card:
                    NavigationManager.NavigateTo($"/card/{card.Id}");
                    break;
                case List list:
                    NavigationManager.NavigateTo($"/list/{list.Id}");
                    break;
                case Board board:
                    NavigationManager.NavigateTo($"/board/{board.Id}");
                    break;
            }
        }

        public enum SearchScope
        {
            Global,
            Children
        }
    }
}
