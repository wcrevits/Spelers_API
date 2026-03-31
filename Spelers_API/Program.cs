using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Spelers_API.Domain.DataDB;
using Spelers_API.Repositories;
using Spelers_API.Services;
using Spelers_API.Services.Interfaces;
using SpelersAPI.Repositories.Interfaces;
using System.Text;
// ... keep your other using statements

var builder = WebApplication.CreateBuilder(args);

// 1. Database & Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Change this to false for easier testing (so you don't need to manually set EmailConfirmed = 1)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 2. JWT Authentication Configuration (ADD THIS)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:JwtIssuer"],
        ValidAudience = builder.Configuration["JwtConfig:JwtIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:JwtKey"]))
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISpelerDAO, SpelerDAO>();
builder.Services.AddScoped<ISpelerService, SpelerService>();

var app = builder.Build();

// 3. Middleware Pipeline (ORDER IS CRITICAL HERE)
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // <-- ADD THIS (Must be between Routing and Authorization)
app.UseAuthorization();

app.MapControllers(); // Ensure your API controllers are mapped
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();