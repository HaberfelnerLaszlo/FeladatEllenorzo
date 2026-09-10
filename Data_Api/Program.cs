using Data_Api;
using Data_Api.Data;
using Data_Api.Endpoints;
using Data_Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<FeladatDb>(opt => { opt.UseInMemoryDatabase("Feladat"); 
//opt.EnableSensitiveDataLogging(); 
//opt.EnableDetailedErrors(); });
//builder.Services.AddDbContext<FeladatSQL> (options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.Parse("9.6.0-mysql")).EnableDetailedErrors());
builder.Services.AddDbContext<FeladatSQL>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
builder.Services.AddScoped<SzorgalmiService>();
builder.Services.AddScoped<SzovegService>();
builder.Services.AddScoped<HianyService>();
builder.Services.AddScoped<TanuloService>();
builder.Services.AddScoped<PontsService>();
builder.Services.AddScoped<HibaService>();
builder.Services.AddScoped<DataSaving>();
builder.Services.AddSingleton<Settings>();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
//Az adatb zist hozzal tre ha m g nincs k sz.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // add 10 seconds delay to ensure the db server is up to accept connections
        // this won't be needed in real world application
        System.Threading.Thread.Sleep(10000);
        var context = services.GetRequiredService<FeladatSQL>();
        context.Database.Migrate();
        var created = context.Database.EnsureCreated();
        var logger = services.GetRequiredService<ILogger<Program>>();
        if (created) logger.LogInformation("Adatbázis létrejött");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Hiba lépett fel az adatbázis létrehozásakor.");
    }
}
    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
app.AddEndpoints();
//app.UseAntiforgery();
app.AddHianyEndPoints();
app.AddHibaEndPoints();
app.AddSzorgalmiEndPoints();
app.AddSzovegEndPoints();
app.AddTanuloEndpoints();
app.AddPontEndPoints();
app.AddManageEndpoints();

app.Run();

namespace Data_Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MigrationDB(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<FeladatSQL>(); // use your DbContext type
                context.Database.Migrate();
                var logger = services.GetRequiredService<ILogger<WebApplication>>();
                logger.LogInformation("Database migrations applied.");
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetService<ILogger<WebApplication>>();
                logger?.LogError(ex, "An error occurred while applying migrations.");
                throw;
            }
        }
    }
}
