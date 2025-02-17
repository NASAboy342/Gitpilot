using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Gitpilot.Services.Interfaces;
using Gitpilot.Services;
using Gitpilot.Repositories.Interfaces;
using Gitpilot.Repositories;
using Gitpilot.Caches;
using Gitpilot.Queues;
using Gitpilot.Helpers;

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
            builder.Services.AddSingleton<IGitpilotRepository, GitpilotRepository>();
            builder.Services.AddSingleton<GitRepositoryCache>();
            builder.Services.AddSingleton<MessageQueue>();
            builder.Services.AddSingleton<OnloadingQueue>();
            builder.Services.AddTransient<OnLoading>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
