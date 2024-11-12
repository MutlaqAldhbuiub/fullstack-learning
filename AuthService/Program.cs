using AuthService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AuthService.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.OpenApi.Models;
using AuthService.Repository;
using AuthService.Services;
using AuthService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mysqlOptions =>
        {
            mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore); // Ignore schema definitions
        }));

builder.Services.AddHttpClient();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultScheme = IdentityConstants.ApplicationScheme;
//     options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
// })
//     .AddCookie(IdentityConstants.ApplicationScheme)
//     .AddBearerToken(IdentityConstants.BearerScheme);

// builder.Services.AddIdentityCore<User>()
//     .AddEntityFrameworkStores<AuthDbContext>()
//     .AddApiEndpoints();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthServicee>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

//Jwt Service 
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.AddAppAuthetication();
builder.Services.AddAuthorization();

// Configure Swagger for AuthService
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await RolesSeed.InitializeAsync(serviceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // AuthService Swagger endpoint
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");

        // NetworkService Swagger endpoint
        c.SwaggerEndpoint("http://localhost:5294/swagger/v1/swagger.json", "NetworkService API");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigrations();
app.Run();

void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}