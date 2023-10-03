using Microsoft.Extensions.Logging;
using FeladatEllenorzo_CP.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FeladatEllenorzo_CP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddScoped<GraphService>();
		builder.Services.AddSingleton<GlobalData>();
		builder.Services.AddSingleton<IDialogService,DialogService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		//builder.Services.AddSingleton<WeatherForecastService>();
		// <ProgramSnippet>
		Console.WriteLine(".NET Graph App-only Tutorial\n");
#if ANDROID
		var a = Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream("FeladatEllenorzo_CP.appsettings.json");

		var config = new ConfigurationBuilder()
					.AddJsonStream(stream)
					.Build();
		builder.Configuration.AddConfiguration(config);
var settings=config.GetRequiredSection("Settings").Get<Settings>() ??
			   throw new Exception("Could not load app settings. See README for configuration instructions.");
#else
		var settings = Settings.LoadSettings();
#endif   
		// Initialize Graph
		GraphService.InitializeGraphForUserAuth(settings);
		GraphService.InitializeGraphForAppOnlyAuth(settings);

		return builder.Build();
	}
}
