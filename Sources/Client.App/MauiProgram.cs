using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;


namespace Client.App;

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

        var config = new ConfigurationBuilder().AddJsonFile("wwwroot/appsettings.json", optional: true, reloadOnChange: true).Build();
        builder.Configuration.AddConfiguration(config);

        // MudBlazor
        builder.Services.AddMudServices(config => {
            
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}