using Windows.Graphics;
using J4JSoftware.WindowsUtilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RouteSnapper;

internal class MainWinSerializer : MainWinSerializerBase<AppConfig>
{
    public MainWinSerializer(
        Window mainWindow
    )
        : base( mainWindow )
    {
    }

    protected override RectInt32 GetDefaultRectangle() => new(100, 100, 1000, 1000);
}