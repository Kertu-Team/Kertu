using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Kertu.InteractiveServer.Components.Pages
{
    public partial class Home : ComponentBase, IDisposable
    {
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        ISessionStorageService SessionStorage { get; set; } = default!;

        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        UserStateService UserStateService { get; set; } = default!;

        private string? _currentUrl;

        protected override void OnInitialized()
        {
            _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            _currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        async void ClearBrowserCache()
        {
            await LocalStorage.ClearAsync();
            await SessionStorage.ClearAsync();
            UserStateService.TreeViewNavigationPanelOpened = default!;
            UserStateService.LastOpenedElement = default!;
        }
    }
}
