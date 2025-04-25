using Data_Api.Data;
using Data_Api.Endpoints;
using Data_Api.Services;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FeladatDb>(opt => opt.UseInMemoryDatabase("Feladat"));
//builder.Services.AddDbContext<FeladatSQL> (options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.Parse("8.2.1-mysql")));

builder.Services.AddScoped<SzorgalmiService>();
builder.Services.AddSingleton<SzovegService>();
builder.Services.AddScoped<HianyService>();
builder.Services.AddSingleton<TanuloService>();
builder.Services.AddScoped<PontService>();
builder.Services.AddScoped<HibaService>();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.AddEndpoints();
app.AddHianyEndPoints();
app.AddHibaEndPoints();
app.AddSzorgalmiEndPoints();
app.AddSzovegEndPoints();
app.AddTanuloEndpoints();
app.AddPontEndPoints();

app.Run();
