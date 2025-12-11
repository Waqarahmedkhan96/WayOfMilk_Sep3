using WoM_BlazorApp.Components;
using WoM_BlazorApp.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7216") // same port as the API project
});

builder.Services.AddScoped<ICowService, HttpCowService>();
builder.Services.AddScoped<IContainerService, HttpContainerService>();
builder.Services.AddScoped<ICustomerService, HttpCustomerService>();
builder.Services.AddScoped<ISaleService, HttpSaleService>();
builder.Services.AddScoped<IMilkService, HttpMilkService>();
builder.Services.AddScoped<ITransferRecordService, HttpTransferRecordService>();
builder.Services.AddScoped<IUserService, HttpUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);
//createScopeForErrors not working at this time
app.UseStatusCodePagesWithReExecute("/not-found");


app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
