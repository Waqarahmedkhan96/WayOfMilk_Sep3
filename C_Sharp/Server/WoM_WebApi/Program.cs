using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sep3.WayOfMilk.Grpc;               // gRPC stubs
using WoM_WebApi.Configuration;         // JwtOptions class
using WoM_WebApi.GlobalExceptionHandler;
using WoM_WebApi.Services.Implementation;
using WoM_WebApi.Services.Interfaces;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sep3.WayOfMilk.Grpc;
//using WebAPI.GlobalExceptionHandler;
using WoM_WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers ----------
builder.Services.AddControllers();

// ---------- Swagger + JWT UI ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WoM WebApi",
        Version = "v1"
    });

    // Swagger: JWT header
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste only the JWT token string here (no 'Bearer ' prefix)."
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ---------- JWT Options ----------
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

// ---------- Authentication (JWT) ----------
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var key        = jwtSection["Key"]
                         ?? throw new InvalidOperationException("Jwt:Key missing");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

            ValidateIssuer = true,
            ValidIssuer    = jwtSection["Issuer"],

            ValidateAudience = true,
            ValidAudience    = jwtSection["Audience"],

            ValidateLifetime = true,

            // must match what we use in AuthController
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

// ---------- Authorization policies ----------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly",     p => p.RequireRole("Owner"));
    options.AddPolicy("VetOnly",       p => p.RequireRole("Vet"));
    options.AddPolicy("WorkerOrOwner", p => p.RequireRole("Worker", "Owner"));
});

// ---------- gRPC clients to Java (plain HTTP) ----------
var javaGrpcAddress = new Uri("http://localhost:9090"); // Java gRPC URL

builder.Services.AddGrpcClient<UserService.UserServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<CowService.CowServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<ContainerService.ContainerServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<CustomerService.CustomerServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<DepartmentService.DepartmentServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<MilkService.MilkServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<SaleService.SaleServiceClient>(o => o.Address = javaGrpcAddress);
builder.Services.AddGrpcClient<TransferRecordService.TransferRecordServiceClient>(o => o.Address = javaGrpcAddress);
// add RecallService later if you have it in proto

// ---------- DI: Services ----------
builder.Services.AddScoped<ITokenService, JwtTokenServiceImpl>();          // JWT helper
builder.Services.AddScoped<IAuthService, GrpcAuthServiceImpl>();          // login / passwords
builder.Services.AddScoped<IUserService, GrpcUserServiceImpl>();          // user CRUD
builder.Services.AddScoped<ICowService, GrpcCowServiceImpl>();            // cows
builder.Services.AddScoped<IMilkService, GrpcMilkServiceImpl>();          // milk
builder.Services.AddScoped<ICustomerService, GrpcCustomerServiceImpl>();  // customers
builder.Services.AddScoped<ISaleService, GrpcSaleServiceImpl>();          // sales
builder.Services.AddScoped<IDepartmentService, GrpcDepartmentServiceImpl>(); // departments
builder.Services.AddScoped<ITransferRecordService, GrpcTransferRecordServiceImpl>(); // transfers
builder.Services.AddScoped<IContainerService, GrpcContainerServiceImpl>();    // containers

// ---------- Global exception middleware ----------
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

// global error handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();