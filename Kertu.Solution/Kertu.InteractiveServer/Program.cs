using Blazored.LocalStorage;
using Kertu.InteractiveServer.Components;
using Kertu.InteractiveServer.Components.Account;
using Kertu.InteractiveServer.Data;
using Kertu.InteractiveServer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

string? connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Persist Data Protection Keys in Docker volume (for encryption and token protection)
builder
    .Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/root/.aspnet/DataProtection-Keys")) // Ensure this volume is properly mounted
    .SetApplicationName("KertuInteractiveServer"); // This ensures the same keys across multiple instances

//Radzen Components
builder.Services.AddRadzenComponents();

// Blazored local storage
builder.Services.AddBlazoredLocalStorage();

//User state service
builder.Services.AddSingleton<UserStateService>();

// Navigation state service
builder.Services.AddSingleton<NavigationStateService>();

//App cookies
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "ApplicationTheme"; // The name of the cookie
    options.Duration = TimeSpan.FromDays(365); // The duration of the cookie
});

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>(); // Your DbContext class
        context.Database.Migrate(); // Apply pending migrations
    }
    catch (Exception ex)
    {
        // Log errors during migration
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
