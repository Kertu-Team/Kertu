using Microsoft.AspNetCore.Components;

namespace Kertu.InteractiveServer.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private string? _userEmail;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            _userEmail = authState.User.Identity?.Name;
        }

        [Parameter]
        public string LightTheme { get; set; }

        [Parameter]
        public string DarkTheme { get; set; }

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

        private bool _value;

        protected override void OnInitialized()
        {
            ThemeService.ThemeChanged += OnThemeChanged;
            _value = ThemeService.Theme != CurrentDarkTheme;
        }

        private void OnThemeChanged()
        {
            _value = ThemeService.Theme != CurrentDarkTheme;
            InvokeAsync(StateHasChanged);
        }

        void Change()
        {
            ThemeService.SetTheme(!_value ? CurrentLightTheme : CurrentDarkTheme);
        }

        private string Icon => _value ? "dark_mode" : "light_mode";

        public void Dispose()
        {
            ThemeService.ThemeChanged -= OnThemeChanged;
        }
    }
}
