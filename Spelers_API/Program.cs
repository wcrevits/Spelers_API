using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Spelers_API.Domain.DataDB;
using Spelers_API.Repositories;
using Spelers_API.Services;
using Spelers_API.Services.Interfaces;
using SpelersAPI.Repositories.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------
// 1. Database & Identity
// --------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // set true in production
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// --------------------------
// 2. JWT Authentication
// --------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:JwtIssuer"],
        ValidAudience = builder.Configuration["JwtConfig:JwtIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:JwtKey"])),
        ClockSkew = TimeSpan.Zero
    };
});

// --------------------------
// 3. Services & Repositories
// --------------------------
builder.Services.AddScoped<ISpelerDAO, SpelerDAO>();
builder.Services.AddScoped<ISpelerService, SpelerService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --------------------------
// 4. Controllers & CORS
// --------------------------
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------------
// 5. Swagger Configuration
// --------------------------
var swaggerOptions = new Spelers_API.Options.OptionsSwagger();
builder.Configuration.GetSection(nameof(Spelers_API.Options.OptionsSwagger)).Bind(swaggerOptions);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API Spelers",
        Version = "v1",
        Description = "An API to perform Speler operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "CDW",
            Email = "wout.crevits@student.vives.be",
            Url = new Uri("https://vives.be")
        },
        License = new OpenApiLicense
        {
            Name = "Speler API LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    // JWT Authentication support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// --------------------------
// 6. Build Application
// --------------------------
var app = builder.Build();

// --------------------------
// 7. Middleware Pipeline
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

// Swagger Middleware
app.UseSwagger(option =>
{
    option.RouteTemplate = swaggerOptions.JsonRoute; // e.g., "swagger/{documentName}/swagger.json"
});

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});

// --------------------------
// 8. Routing & Endpoints
// --------------------------
app.MapControllers();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// --------------------------
// 9. Run Application
// --------------------------
app.Run();