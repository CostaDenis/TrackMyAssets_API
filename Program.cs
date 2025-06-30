using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TrackMyAssets_API;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.Enums;
using TrackMyAssets_API.Domain.ModelsViews;
using TrackMyAssets_API.Infrastructure;

#region Builder

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetSection("Jwt")?.Value ?? "";
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConnectionString));

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

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IUserAssetService, UserAssetService>();

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

app.MapGet("/", () => Results.Json(new HomeModelView())).WithTags("Home").AllowAnonymous();

#endregion


#region Administrators

string GenerateTokenJwt(Guid id, string email, string role)
{
    if (string.IsNullOrEmpty(key))
    {
        return string.Empty;
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administrators/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{
    var adm = administratorService.Login(loginDTO);

    if (adm == null)
    {
        return Results.Unauthorized();
    }

    string token = GenerateTokenJwt(adm.Id, adm.Email, "Admin");

    return Results.Ok(new LoggedAdministratorModelView
    {
        Id = adm.Id,
        Email = adm.Email,
        Token = token
    });

}).AllowAnonymous().WithTags("Administrator");


app.MapGet("/administrators/users", ([FromQuery] int? page, IAdministratorService administratorService) =>
{

    var usersModelView = new List<UserModelView>();
    var users = administratorService.GetAllUsers(page);

    foreach (var usr in users)
    {
        usersModelView.Add(new UserModelView
        {
            Id = usr.Id,
            Email = usr.Email
        });
    }

    return Results.Ok(users);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administrator");


app.MapGet("administrators/users/{id}", ([FromRoute] Guid id, IAdministratorService administratorService) =>
{

    var user = administratorService.GetUserById(id);

    if (user == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new UserModelView
    {
        Id = user.Id,
        Email = user.Email
    });

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administrator");


app.MapDelete("administrators/users/{id}", ([FromRoute] Guid id, IAdministratorService administratorService) =>
{

    var user = administratorService.GetUserById(id);

    if (user == null)
    {
        return Results.NotFound();
    }

    administratorService.DeleteUser(user);
    return Results.NoContent();

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Administrator");

#endregion



#region Users

app.MapPost("/users/login", ([FromBody] LoginDTO loginDTO, IUserService userService) =>
{

    var user = userService.Login(loginDTO);

    if (user == null)
    {
        return Results.Unauthorized();
    }
    string token = GenerateTokenJwt(user.Id, user.Email, "User");

    return Results.Ok(new LoggedUserModelView
    {
        Id = user.Id,
        Email = user.Email,
        Token = token
    });

}).AllowAnonymous().WithTags("User");

app.MapPost("/users", ([FromBody] UserDTO userDTO, IUserService userService) =>
{
    var user = new User
    {
        Email = userDTO.Email,
        Password = userDTO.Password
    };

    userService.Create(user);

    return Results.Created($"/users/{user.Id}", user);

}).AllowAnonymous().WithTags("User");

app.MapPut("/users", ([FromBody] UserDTO userDTO, HttpContext http, IUserService userService) =>
{

    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();

    var user = userService.GetById(userId.Value);

    user.Email = userDTO.Email;
    user.Password = userDTO.Password;

    userService.Update(user);

    return Results.Ok(user);

}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("User");

app.MapDelete("/users", (HttpContext http, IUserService userService) =>
{
    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();

    var user = userService.GetById(userId.Value);
    userService.DeleteOwnUser(user);

    return Results.NoContent();
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("User");


#endregion


#region Asset

app.MapPost("/assets", ([FromBody] AssetDTO assetDTO, IAssetService assetService) =>
{

    if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
    {
        return Results.BadRequest("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency");
    }

    var asset = new Asset
    {
        Name = assetDTO.Name,
        Symbol = assetDTO.Symbol,
        Type = parsedType
    };

    assetService.Create(asset);

    return Results.Created($"/assets/{asset.Id}", asset);

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Asset");

app.MapGet("/assets", ([FromQuery] int? page, IAssetService AssetService) =>
{
    var assetsModelView = new List<AssetModelView>();
    var assets = AssetService.GetAll(page);

    foreach (var assts in assets)
    {
        assetsModelView.Add(new AssetModelView
        {
            Name = assts.Name,
            Symbol = assts.Symbol!,
            Type = assts.Type.ToString()
        });
    }

    return Results.Ok(assets);

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, User" }).WithTags("Asset");

app.MapGet("/assets/{id}", ([FromRoute] Guid id, IAssetService assetService) =>
{

    var asset = assetService.GetById(id);

    if (asset == null)
        return Results.NotFound();

    return Results.Ok(new AssetModelView
    {
        Name = asset.Name,
        Symbol = asset.Symbol!,
        Type = asset.Type.ToString()
    });

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, User" }).WithTags("Asset");

app.MapPut("/assets/{id}", ([FromBody] AssetDTO assetDTO, Guid id, IAssetService assetService) =>
{

    var asset = assetService.GetById(id);
    if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
    {
        return Results.BadRequest("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency");
    }

    if (asset == null)
        return Results.NotFound();


    asset.Name = assetDTO.Name;
    asset.Symbol = assetDTO.Symbol;
    asset.Type = parsedType;

    assetService.Update(asset);

    return Results.Ok(asset);

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Asset");

app.MapDelete("/assets/{id}", ([FromRoute] Guid id, IAssetService assetService) =>
{

    var asset = assetService.GetById(id);

    if (asset == null)
        return Results.NotFound();

    assetService.Delete(asset);

    return Results.NoContent();

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }).WithTags("Asset");

#endregion


#region UserAsset

app.MapPost("/users/assets", ([FromBody] UserAssetAddDTO userAssetDTO, HttpContext http, IAssetService assetService, IUserAssetService userAssetService) =>
{
    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();

    if (assetService.GetById(userAssetDTO.AssetId) == null)
        return Results.NotFound();


    var result = userAssetService.AddUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

    return Results.Ok(result);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("UserAsset");

app.MapDelete("/users/assets", ([FromBody] UserAssetRemoveDTO userAssetDTO, HttpContext http, IUserAssetService userAssetService) =>
{
    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();


    var result = userAssetService.RemoveUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

    return Results.Ok(result);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("UserAsset");

app.MapGet("/users/assets", (HttpContext http, IUserAssetService userAssetService) =>
{
    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();

    var userAssets = userAssetService.UserAssets(userId.Value);
    var userAssetsModelView = new List<UserAssetModelView>();

    if (userAssets == null)
        return Results.NotFound();

    foreach (var usr in userAssets)
    {
        userAssetsModelView.Add(new UserAssetModelView
        {
            UserId = usr.UserId,
            AssetId = usr.AssetId,
            Units = usr.Units
        });
    }

    return Results.Ok(userAssets);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("UserAsset");

app.MapGet("/users/assets/{assetId}", ([FromRoute] Guid assetId, HttpContext http, IUserAssetService userAssetService) =>
{
    var userId = JwtUtils.GetUserId(http);

    if (userId == null)
        return Results.Unauthorized();

    var userAsset = userAssetService.GetUserAssetByAssetId(userId.Value, assetId);

    if (userAsset == null)
        return Results.NotFound();

    return Results.Ok(new UserAssetModelView
    {
        UserId = userAsset.UserId,
        AssetId = userAsset.AssetId,
        Units = userAsset.Units
    });
}).RequireAuthorization(new AuthorizeAttribute { Roles = "User" }).WithTags("UserAsset");


#endregion

app.UseSwagger();
app.UseSwaggerUI();


app.Run();