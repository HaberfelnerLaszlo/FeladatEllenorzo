using Microsoft.Extensions.Logging;
using FeladatEllenorzo_CP.Data;

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

		builder.Services.AddSingleton<WeatherForecastService>();
		// <ProgramSnippet>
		Console.WriteLine(".NET Graph App-only Tutorial\n");

		var settings = Settings.LoadSettings();

		// Initialize Graph
		GraphService.InitializeGraphForUserAuth(settings);
		GraphService.InitializeGraphForAppOnlyAuth(settings);

		return builder.Build();
	}
}
