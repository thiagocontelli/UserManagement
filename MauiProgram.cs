using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using UraniumUI;
using UserManagement.Repositories;
using UserManagement.Services.UserServices;
using UserManagement.ViewModels;
using UserManagement.Views;
using UserManagement.Services;
using UserManagement.Services.WindowServices;

#if WINDOWS
using Microsoft.Maui.LifecycleEvents;
#endif

namespace UserManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if WINDOWS
            bool isMainWindowCreated = false;

            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                        if (!isMainWindowCreated)
                        {
                            isMainWindowCreated = true;

                            if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter)
                            {
                                overlappedPresenter.SetBorderAndTitleBar(true, true);
                                overlappedPresenter.Maximize();
                            }
                        }
                        else
                        {
                            if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter secondaryPresenter)
                            {
                                secondaryPresenter.SetBorderAndTitleBar(true, true);
                            }
                        }
                    });
                });
            });
#endif

            builder.Services.AddTransient<UsersView>();
            builder.Services.AddTransient<UsersViewModel>();
            builder.Services.AddTransient<UpsertUserView>();
            builder.Services.AddTransient<UpsertUserViewModel>();
            builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddTransient<GetUsersService>();
            builder.Services.AddTransient<AddUserService>();
            builder.Services.AddTransient<UpdateUserService>();
            builder.Services.AddTransient<GetUserService>();
            builder.Services.AddTransient<DeleteUserService>();
            builder.Services.AddTransient<OpenWindowService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
