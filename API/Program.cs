using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Persistence;

var AllowSameDomain = "_allowSameDomain";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    opt.TokenValidationParameters =
        new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}",
            ValidateLifetime = true,
        };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSameDomain,
                      policy =>
                      {
                          policy.SetIsOriginAllowed(origin =>
                          new Uri(origin).Host == "localhost") // Allow any port on localhost
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Dev mode");
    builder.Services.AddDbContext<SlateDbContext>(opt => 
        opt.UseInMemoryDatabase("SlateDevDb")
    );
}
else
{
    // Load DB_CONNECTION_STRING env variable 
    Env.Load();
    string? dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set.");

    Console.WriteLine("Prod mode");
    builder.Services.AddDbContext<SlateDbContext>(opt =>
        opt.UseSqlServer(dbConnection)
    );
}

builder.WebHost.UseUrls("https://localhost:7073");
var app = builder.Build();

// Check and ensure database creation and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SlateDbContext>();
    dbContext.Database.EnsureCreated();  // Ensures the in-memory database is created
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(AllowSameDomain);
app.UseAuthorization();
app.MapControllers();

app.Run();

// Add this partial class so tests can reference Program
public partial class Program { }
