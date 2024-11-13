using Microsoft.AspNetCore.Components;

namespace Kertu.InteractiveServer.Services
{
    public class RecentElementService(
        NavigationManager navigation,
        Blazored.LocalStorage.ILocalStorageService localStorage,
        UserStateService userState
    )
    {
        private NavigationManager Navigation { get; } = navigation;
        private Blazored.LocalStorage.ILocalStorageService LocalStorage { get; } = localStorage;
        private UserStateService UserState { get; } = userState;

        public async Task Open()
        {
            if (UserState.RecentElementAlreadyOpened)
            {
                return;
            }
            UserState.RecentElementAlreadyOpened = true;
            string? lastUrl = await LocalStorage.GetItemAsync<string>("lastUrl");
            if (!string.IsNullOrWhiteSpace(lastUrl) && !Navigation.Uri.EndsWith(lastUrl))
            {
                Navigation.NavigateTo(lastUrl);
            }
        }

        public async Task Save()
        {
            await LocalStorage.SetItemAsync("lastUrl", Navigation.ToBaseRelativePath(Navigation.Uri));
        }
    }
}
