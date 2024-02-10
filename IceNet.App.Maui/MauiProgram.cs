using System.Reflection;
using Microsoft.Extensions.Configuration;
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

        var project = Assembly.GetCallingAssembly().GetName().Name;
        var appSettings = $"{project}.appsettings.json";

#if DEBUG
        appSettings = $"{project}.appsettings.Development.json";

        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();

#elif RELEASE
        appSettings = $"{project}.appsettings.Production.json";
#endif
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(appSettings);
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(config);

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddSingleton<IPreferenceStoreService, PreferenceStoreService>();

        Environment.SetEnvironmentVariable("App", "Maui");

        return builder.Build();
    }
}