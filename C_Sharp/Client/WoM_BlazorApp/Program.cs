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
builder.Services.AddTransient<JwtAuthHandler>();

// C. Register the Named Client (Configures the Base Address + Handler)
builder.Services.AddHttpClient("WomAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5098");
})
.AddHttpMessageHandler<JwtAuthHandler>();

// D. Register the Global HttpClient Override
//    (Now safe because "WomAPI" is definitely defined above)
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WomAPI"));

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