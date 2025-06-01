using FeladatEllenorzo_CP.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using FeladatEllenorzo_CP.Services;
using CommunityToolkit.Maui;
using Microsoft.FluentUI.AspNetCore.Components;
namespace FeladatEllenorzo_CP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .AddAppSettings()
            .RegisterAppServices();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddFluentUIComponents();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        //builder.Logging.AddDebug();
#endif
        return builder.Build();
    }

    private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
	{
#if ANDROID
		var assembly = typeof(App).GetTypeInfo().Assembly;
		var config = new ConfigurationBuilder() .SetBasePath(Directory.GetCurrentDirectory()) .AddJsonFile(new EmbeddedFileProvider(assembly),"appsettings.json", optional: true, reloadOnChange: false) .Build();
		builder.Configuration.AddConfiguration(config);
#else
		// Load settings
		IConfiguration config = new ConfigurationBuilder()
            //.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            // appsettings.json is required
            .AddJsonFile("appsettings.json", optional: false)
            // appsettings.Development.json" is optional, values override appsettings.json
            .AddJsonFile($"appsettings.Development.json", optional: true)
            // User secrets are optional, values override both JSON files
            //.AddUserSecrets<MauiProgram>()
            .Build();
		builder.Configuration.AddConfiguration(config);
#endif
		return builder;
	}
	private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<IAlertService, AlertService>();
		builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
		builder.Services.AddSingleton<IGraphService, GraphService>();
		builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
		builder.Services.AddSingleton<IHianyService, HianyService>();
		builder.Services.AddSingleton<IHibaService, HibaService>();
		builder.Services.AddSingleton<ISzovegService, SzovegService>();
        builder.Services.AddSingleton<ISzorgalmiService, SzorgalmiService>();
        builder.Services.AddSingleton<IPontService, PontService>();
        builder.Services.AddSingleton<ITanuloService, TanuloService>();
        builder.Services.AddSingleton<GlobalData>();
		return builder;
	}
}
