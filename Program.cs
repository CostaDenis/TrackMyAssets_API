using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ModelsViews;


var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConnectionString));

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

//Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => Results.Json(new HomeModelView())).WithTags("Home").AllowAnonymous();


#region Administrators

app.MapPost("/administrators/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{
    //Adicinar o Token JWT aqui
    var adm = administratorService.Login(loginDTO);

    if (adm != null)
    {
        return Results.Ok(new LoggedAdministratorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
        });
    }
    else
    {
        return Results.Unauthorized();
    }
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

}).RequireAuthorization().WithTags("Administrator");


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

}).RequireAuthorization().WithTags("Administrator");


app.MapDelete("administrators/users/{id}", ([FromRoute] Guid id, IAdministratorService administratorService) =>
{

    var user = administratorService.GetUserById(id);

    if (user == null)
    {
        return Results.NotFound();
    }

    administratorService.DeleteUser(user);
    return Results.NoContent();

}).RequireAuthorization().WithTags("Administrator");

#endregion


app.UseSwagger();
app.UseSwaggerUI();


app.Run();