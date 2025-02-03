using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Gitpilot.Services.Interfaces;
using Gitpilot.Services;

namespace Gitpilot
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IGitRepositoryService, GitRepositoryService>();
            builder.Services.AddSingleton<IFolderPickerService, WindowsFolderPickerService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
