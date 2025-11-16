using FileRepositories;
using RepositoryContracts;

// Explicit Usings
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// WebApi.GlobalExceptionHandler
using WebAPI.GlobalExceptionHandler;        

var builder = WebApplication.CreateBuilder(args);

// MVC controllers
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// File Repositories (DI)
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICowRepository, CowFileRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerFileRepository>();
builder.Services.AddScoped<IContainerRepository, ContainerFileRepository>();
builder.Services.AddScoped<IMilkRepository, MilkFileRepository>();
builder.Services.AddScoped<ISaleRepository, SaleFileRepository>();
builder.Services.AddScoped<ITransferRecordRepository, TransferRecordFileRepository>();

// For later on CORS for Blazor client later
// builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
//     p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// register global exception middleware
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

//plug middleware in early so it catches downstream exceptions
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger for UI
}

app.UseHttpsRedirection();
app.MapControllers();  
app.Run();
