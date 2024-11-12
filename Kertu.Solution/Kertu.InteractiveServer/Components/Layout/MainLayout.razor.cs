using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components;

namespace Kertu.InteractiveServer.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        [Parameter]
        public string LightTheme { get; set; }

        [Parameter]
        public string DarkTheme { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private NavigationStateService NavigationState { get; set; }

        [Inject]
        private Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }

        string? _userEmail;
        string Icon => _value ? "dark_mode" : "light_mode";

        private bool _value;
        private string CurrentLightTheme =>
            LightTheme
            ?? ThemeService.Theme?.ToLowerInvariant() switch
            {
                "dark" => "default",
                "material-dark" => "material",
                "fluent-dark" => "fluent",
                "material3-dark" => "material3",
                "software-dark" => "software",
                "humanistic-dark" => "humanistic",
                "standard-dark" => "standard",
                _ => ThemeService.Theme,
            };
        private string CurrentDarkTheme =>
            DarkTheme
            ?? ThemeService.Theme?.ToLowerInvariant() switch
            {
                "default" => "dark",
                "material" => "material-dark",
                "fluent" => "fluent-dark",
                "material3" => "material3-dark",
                "software" => "software-dark",
                "humanistic" => "humanistic-dark",
                "standard" => "standard-dark",
                _ => ThemeService.Theme,
            };

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            _userEmail = authState.User.Identity?.Name;
            ThemeService.ThemeChanged += OnThemeChanged;
            _value = ThemeService.Theme != CurrentDarkTheme;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !NavigationState.HasNavigatedToLastUrl)
            {
                NavigationState.HasNavigatedToLastUrl = true;
                string? lastUrl = await LocalStorage.GetItemAsync<string>("lastUrl");
                if (!string.IsNullOrWhiteSpace(lastUrl) && !Navigation.Uri.EndsWith(lastUrl))
                {
                    Navigation.NavigateTo(lastUrl);
                }
            }
        }

        void IDisposable.Dispose()
        {
            ThemeService.ThemeChanged -= OnThemeChanged;
        }

        void Change()
        {
            ThemeService.SetTheme(!_value ? CurrentLightTheme : CurrentDarkTheme);
        }

        void OnLinkClick()
        {
            Navigation.NavigateTo("/", forceLoad: true);
        }

        private void OnThemeChanged()
        {
            _value = ThemeService.Theme != CurrentDarkTheme;
            InvokeAsync(StateHasChanged);
        }
    }
}
