using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using WoM_BlazorApp;
using WoM_BlazorApp.Components;
using WoM_BlazorApp.Services.Http;
using WoM_BlazorApp.Services.Interfaces;
using WoM_BlazorApp.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. FRAMEWORK SERVICES
// ==========================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// ==========================================
// 2. INFRASTRUCTURE (Token & Http) - THE FIX
// ==========================================

// A. TokenService (Scoped to the User Circuit)
builder.Services.AddScoped<ITokenService, TokenServiceImpl>();

// B. The Handler (Scoped to the User Circuit)
builder.Services.AddScoped<JwtAuthHandler>();

// C. THE MANUAL HTTP CLIENT REGISTRATION
// This ensures the Client uses the specific Handler instance from THIS scope.
builder.Services.AddScoped(sp =>
{
    // 1. Get the Handler from the current user's scope
    var handler = sp.GetRequiredService<JwtAuthHandler>();

    // 2. Important: Set the inner handler to handle the actual network call
    handler.InnerHandler = new HttpClientHandler();

    // 3. Create the Client manually with the Base Address
    var client = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5098")
    };

    return client;
});

// ==========================================
// 3. AUTH & DOMAIN SERVICES
// ==========================================

// Auth Provider
builder.Services.AddScoped<SimpleAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<SimpleAuthProvider>());

// Domain Services (Injects the Scoped HttpClient we defined above)
builder.Services.AddScoped<IMilkService, MilkServiceImpl>();
builder.Services.AddScoped<IContainerService, ContainerServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
// Add other services here...

var app = builder.Build();

// ==========================================
// 4. MIDDLEWARE PIPELINE
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();