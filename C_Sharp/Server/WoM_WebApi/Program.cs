
var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers ----------
builder.Services.AddControllers(); // add MVC controllers

// ---------- Swagger + JWT UI ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WoM WebApi",
        Version = "v1"
    });

    // Swagger JWT definition
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});

// ---------- JWT Options ----------
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

// ---------- Authentication (JWT) ----------
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key missing");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true
        };
    });

// ---------- Authorization (roles) ----------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly", p => p.RequireRole("Owner"));
    options.AddPolicy("VetOnly", p => p.RequireRole("Vet"));
    options.AddPolicy("WorkerOrOwner", p => p.RequireRole("Worker", "Owner"));
});

// ---------- gRPC clients to Java (PLAIN HTTP on localhost) ----------
// Java gRPC server listens on http://localhost:9090 WITHOUT TLS.
// This is only used internally between WebApi and Java backend.
var javaGrpcAddress = new Uri("http://localhost:9090");

// Default HttpClient handler is enough (no TLS, no custom cert validation).

// User service
builder.Services
    .AddGrpcClient<UserService.UserServiceClient>(o => o.Address = javaGrpcAddress);

// Auth service
builder.Services
    .AddGrpcClient<AuthService.AuthServiceClient>(o => o.Address = javaGrpcAddress);

// Cow service
builder.Services
    .AddGrpcClient<CowService.CowServiceClient>(o => o.Address = javaGrpcAddress);

// Container service
builder.Services
    .AddGrpcClient<ContainerService.ContainerServiceClient>(o => o.Address = javaGrpcAddress);

// Customer service
builder.Services
    .AddGrpcClient<CustomerService.CustomerServiceClient>(o => o.Address = javaGrpcAddress);

// Department service
builder.Services
    .AddGrpcClient<DepartmentService.DepartmentServiceClient>(o => o.Address = javaGrpcAddress);

// Milk service
builder.Services
    .AddGrpcClient<MilkService.MilkServiceClient>(o => o.Address = javaGrpcAddress);

// Sale service
builder.Services
    .AddGrpcClient<SaleService.SaleServiceClient>(o => o.Address = javaGrpcAddress);

// TransferRecord service
builder.Services
    .AddGrpcClient<TransferRecordService.TransferRecordServiceClient>(o => o.Address = javaGrpcAddress);

// Recall service
builder.Services
    .AddGrpcClient<RecallService.RecallServiceClient>(o => o.Address = javaGrpcAddress);

// ---------- Global exception middleware ----------
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

// use global exception handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();   // WebApi itself still runs over HTTPS for browser & Swagger

app.UseAuthentication(); // JWT auth
app.UseAuthorization();  // role checks

app.MapControllers();

app.Run();
