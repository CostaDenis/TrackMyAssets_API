using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.Enums;
using TrackMyAssets_API.Domain.ModelsViews;

#region Builder

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration["Jwt"];
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                             builder.Configuration.GetConnectionString("DefaultConnection"); ;


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IUserAssetService, UserAssetService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT: "
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{

                Reference = new OpenApiReference{

                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"

                }

            },

        new string[]{ }
        }

    });

});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


#endregion


app.Run();