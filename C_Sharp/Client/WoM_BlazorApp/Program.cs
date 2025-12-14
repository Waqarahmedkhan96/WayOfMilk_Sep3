using WoM_BlazorApp;
using Microsoft.AspNetCore.Components.Authorization;
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
// 2. INFRASTRUCTURE (Token & Http)
// ==========================================

// A. Register TokenService first (The Handler needs this!)
builder.Services.AddScoped<ITokenService, TokenServiceImpl>();

// B. Register the Handler (The HttpClient needs this!)
builder.Services.AddScoped<JwtAuthHandler>();

// C. THE FIX: Register HttpClient MANUALLY
// This forces the Client + Handler + TokenService to all live in the same "Scope Bucket"
builder.Services.AddScoped(sp =>
{
    // Get the Handler from the current user's scope
    var handler = sp.GetRequiredService<JwtAuthHandler>();

    // Important: The "InnerHandler" is usually null when manually creating.
    // We must set it to a standard HttpClientHandler to actually send network requests.
    handler.InnerHandler = new HttpClientHandler();

    // Create the Client manually
    var client = new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5098")
    };

    return client;
});

// ==========================================
// 3. AUTH & DOMAIN SERVICES
// ==========================================

// Auth Provider (Needs TokenService & the HttpClient we just configured)
builder.Services.AddScoped<SimpleAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<SimpleAuthProvider>());

// Domain Services
builder.Services.AddScoped<IMilkService, MilkServiceImpl>();
builder.Services.AddScoped<IContainerService, ContainerServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
//builder.Services.AddScoped<ICowService, CowServiceImpl>

var app = builder.Build();

// ==========================================
// 4. MIDDLEWARE PIPELINE (Order is Critical Here)
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // app.UseHsts(); // Disabled for local HTTP dev
}

app.UseStatusCodePagesWithReExecute("/not-found");

// app.UseHttpsRedirection(); // Disabled for local HTTP dev

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();