using Microsoft.AspNetCore.Components.WebView.Maui;
using AlcoholLimit.Data;

namespace AlcoholLimit;

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
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		#endif
		
		builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<DrinkDatabase>();
        builder.Services.AddSingleton<ProfileDatabase>();

        return builder.Build();
	}
}
