using WoM_BlazorApp;
using Microsoft.AspNetCore.Components.Authorization;
using WoM_BlazorApp.Components;
using WoM_BlazorApp.Services.Interfaces;
using WoM_BlazorApp.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// razor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// http client to WebApi
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:5098") // <-- your WebApi base URL
});

// auth core for <AuthorizeView> / [Authorize]
builder.Services.AddAuthorizationCore();

// auth state provider (your SimpleAuthProvider from other project)
// make sure namespace + ctor match
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();

// token store
builder.Services.AddScoped<ITokenService, TokenServiceImpl>();

// domain services
builder.Services.AddScoped<IMilkService, MilkServiceImpl>();
builder.Services.AddScoped<IContainerService, ContainerServiceImpl>();

// (you can also register IUserService, ICowService, etc. later)

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
//TODO come back and uncomment this then gigure out what it does
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
