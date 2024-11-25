
using System.Text;
using Backend.Context;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Backend.Utility.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPasswordAndEmailHasher, PasswordAndEmailHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddScoped<IUserAchievementService, UserAchievementsService>();
builder.Services.AddScoped<IUserAchievementsRepository, UserAchievementsRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGeolocationService, GeolocationService>();
builder.Services.AddScoped<IGeolocationRepository, GeolocationRepository>();
builder.Services.AddScoped<IContinentService, ContinentService>();
builder.Services.AddScoped<IContinentRepository, ContinentRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ITokenUtil, TokenUtil>();
builder.Services.AddScoped<IMonthlyUsageService, MonthlyUsageService>();
builder.Services.Configure<Config>(builder.Configuration.GetSection("Keys"));

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
            corsPolicyBuilder
                .WithOrigins("http://localhost:3000")
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithHeaders("Content-Type", "Authorization");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

app.Run();