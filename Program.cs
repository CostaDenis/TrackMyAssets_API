using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.ModelsViews;


var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConnectionString));

//Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => Results.Json(new HomeModelView())).WithTags("Home").AllowAnonymous();


app.UseSwagger();
app.UseSwaggerUI();


app.Run();