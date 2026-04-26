using DigimonDawnDusk.Database;
using Microsoft.Extensions.Logging;

namespace DigimonDawnDusk;

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
		builder.Services.AddQuickGridEntityFrameworkAdapter();
		builder.Services.AddDbContextFactory<DigimonDbContext>();
		builder.Services.AddSingleton<DigimonDbInitializer>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		using var scope = app.Services.CreateScope();
		var initializer = scope.ServiceProvider.GetRequiredService<DigimonDbInitializer>();
		initializer.Initialize();

		return app;
	}
}
