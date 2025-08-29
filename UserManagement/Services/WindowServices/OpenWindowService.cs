using UserManagement.Models;

namespace UserManagement.Services.WindowServices;

public class OpenWindowService : IService<OpenWindowInput, bool>
{
    public Task<bool> Execute(OpenWindowInput input)
    {
        if (input.Page == null)
            throw new ArgumentNullException(nameof(input.Page));

        var window = new Window(input.Page)
        {
            Title = input.Title
        };

        if (input.OnClose != null)
            window.Destroying += (_, _) => input.OnClose();

#if WINDOWS
        EventHandler handler = null;
        handler = (s, e) =>
        {
            var mauiWindow = s as Microsoft.Maui.Controls.Window;
            if (mauiWindow?.Handler?.PlatformView == null)
                return;

            var nativeWindow = WinRT.Interop.WindowNative.GetWindowHandle(mauiWindow.Handler.PlatformView);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(nativeWindow);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter)
            {
                var desiredHeight = input.Height ?? (int)appWindow.Size.Height;
                appWindow.Resize(new Windows.Graphics.SizeInt32(input.Width, desiredHeight));

                var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Primary);
                var centerX = (displayArea.WorkArea.Width - appWindow.Size.Width) / 2;
                var centerY = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32((int)centerX, (int)centerY));
            }

            mauiWindow.HandlerChanged -= handler;
        };

        window.HandlerChanged += handler;
#endif

        Application.Current?.OpenWindow(window);

        return Task.FromResult(true);
    }
}

