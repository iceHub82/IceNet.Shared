using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using IceNet.Shared;
using IceNet.Service.Preferences;

namespace IceNet.App.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddSingleton<IPreferenceStoreService, PreferenceStoreService>();

        Environment.SetEnvironmentVariable("App", "Maui");

        return builder.Build();
    }
}