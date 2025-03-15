using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SlateAPI.Persistence;

var AllowSameDomain = "_allowSameDomain";

// Load env variables
Env.Load();
string dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddDbContext<SlateDbContext>(opt =>
    opt.UseMySQL(
        dbConnection
    )
);


builder.WebHost.UseUrls("https://localhost:7073");
var app = builder.Build();

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
