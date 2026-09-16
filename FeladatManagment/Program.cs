using FeladatLibrary.Data;
using FeladatLibrary.Services;

using FeladatManagment;
using FeladatManagment.Components;
using FeladatManagment.Interop.TeamsSDK;
using FeladatManagment.Services;

using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var config = builder.Configuration.Get<ConfigOptions>();
builder.Services.AddTeamsFx(config.TeamsFx.Authentication);
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

builder.Services.AddSingleton<LibraryConfiguration>();
builder.Services.AddFluentUIComponents();
builder.Services.AddSingleton<IPontService, PontService>();
builder.Services.AddSingleton<ICsoportService, CsoportService>();
builder.Services.AddScoped<TeszterApiService>();
builder.Services.AddScoped<MathService>();
builder.Services.AddSingleton<GlobalData>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(o => o.ContentSecurityFrameAncestorsPolicy = "'self' *");

app.Run();