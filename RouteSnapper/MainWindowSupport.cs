using Windows.Graphics;
using J4JSoftware.WindowsUtilities;
using Microsoft.UI.Xaml;

namespace RouteSnapper;

internal class MainWindowSupport : J4JMainWindowSupport
{
    public MainWindowSupport(
        Window mainWindow, 
        IJ4JWinAppSupport winAppSupport
        ) : base(mainWindow, winAppSupport)
    {
    }

    protected override RectInt32 GetDefaultWindowPositionAndSize() => new(100, 100, 1000, 1000);
}