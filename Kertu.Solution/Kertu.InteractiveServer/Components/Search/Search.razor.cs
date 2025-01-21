using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Data.Models.Elements;
using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Kertu.InteractiveServer.Components.Search
{
    public partial class Search : ComponentBase
    {
        private string _userID;

        public string SearchQuery { get; set; } = string.Empty;

        public bool ShowDropdown { get; set; }

        public SearchScope CurrentScope { get; set; } = SearchScope.Global;

        public List<Element> SearchResults { get; set; } = new();

        [Inject] private ApplicationDbContext DbContext { get; set; } = default!;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] private UserStateService UserStateService { get; set; } = default!;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public void ShowResults()
        {
            ShowDropdown = true;

            if (string.IsNullOrWhiteSpace(SearchQuery) || !SearchResults.Any())
            {
                SearchResults.Clear();
            }
        }

        public void HideResults()
        {
            ShowDropdown = false;
        }

        public void ClearSearch()
        {
            SearchQuery = string.Empty;
            ShowDropdown = false;
        }

        public void SetScope(SearchScope scope)
        {
            CurrentScope = scope;
            ShowResults();
        }

        public async Task HandleSearchInput(string value)
        {
            SearchQuery = value;

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchResults.Clear();
                HideResults();
                return;
            }

            int? parentId = CurrentScope == SearchScope.Children
                ? UserStateService.GetLastOpenedElement(_userID)?.Id
                : null;

            SearchResults = await SearchAsync(SearchQuery, CurrentScope, parentId);
            ShowDropdown = true;
        }

        public void HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                if (string.IsNullOrWhiteSpace(SearchQuery) || !SearchResults.Any())
                {
                    ShowResults();
                }
                else
                {
                    ShowResults();
                }
            }
        }

        public string GetThemeClass()
        {
            return ThemeService.Theme == "material-dark" ? "dark-theme" : "light-theme";
        }

        protected override async Task OnInitializedAsync()
        {
            await SetCurrentUserID();

            ThemeService.ThemeChanged += () =>
                StateHasChanged();
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

            // Zamiana zapytania na małe litery
            query = query.ToLower();

            // Wyszukiwanie po Name dla wszystkich elementów
            var nameResults = await searchQuery
                .Where(e => EF.Functions.Like(e.Name.ToLower(), $"%{query}%"))
                .ToListAsync();

            // Wyszukiwanie po Description tylko dla Card
            var descriptionResults = await DbContext.Elements
                .OfType<Card>() // Wybiera tylko elementy typu Card
                .Where(c => EF.Functions.Like(c.Description.ToLower(), $"%{query}%"))
                .ToListAsync();

            // Połączenie wyników
            return nameResults.Union(descriptionResults).ToList();
        }

        private void NavigateToElement(Element element)
        {
            switch (element)
            {
                case Card card:
                    NavigationManager.NavigateTo($"/card/{card.Id}", true);
                    break;
                case List list:
                    NavigationManager.NavigateTo($"/list/{list.Id}", true);
                    break;
                case Board board:
                    NavigationManager.NavigateTo($"/board/{board.Id}", true);
                    break;
            }
        }

        public enum SearchScope
        {
            Global,
            Children,
        }
    }
}