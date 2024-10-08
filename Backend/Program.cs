
using System.Text;
using Backend.Context;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<AchievementRepository>();
builder.Services.AddScoped<FriendsService>();
builder.Services.AddScoped<FriendsRepository>();
builder.Services.AddScoped<LeaderboardService>();
builder.Services.AddScoped<LeaderboardRepository>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<StatisticsRepository>();
builder.Services.AddScoped<UserAchievementsService>();
builder.Services.AddScoped<UserAchievementsRepository>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<GeolocationService>();
builder.Services.AddScoped<GeolocationRepository>();
builder.Services.AddScoped<ContinentService>();
builder.Services.AddScoped<ContinentRepository>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<CityRepository>();
builder.Services.AddTransient<TokenUtil>();
builder.Services.Configure<Config>(builder.Configuration.GetSection("Keys"));

// Add DbContext configuration
builder.Services.AddDbContext<GuessItContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseNetTopologySuite()));

var tokenSettings = builder.Configuration.GetSection("Keys").Get<Config>();
if (string.IsNullOrEmpty(tokenSettings?.TokenKey))
{
    throw new ArgumentNullException(nameof(tokenSettings.TokenKey), "TokenKey cannot be null or empty.");
}

var key = Encoding.ASCII.GetBytes(tokenSettings.TokenKey);
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new PointConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        corsPolicyBuilder =>
        {
            // Adjust these as needed
            corsPolicyBuilder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Optional: If you want to allow credentials (cookies, etc.)
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "AmiSwinka!");

app.Run();