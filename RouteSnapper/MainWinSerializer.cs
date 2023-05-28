using Windows.Graphics;
using J4JSoftware.WindowsUtilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RouteSnapper;

internal class MainWinSerializer : MainWinSerializerBase<AppConfig>
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public MainWinSerializer(
        Window mainWindow
    )
        : base( mainWindow, CreateJsonOptions() )
    {
    }

    internal static JsonSerializerOptions CreateJsonOptions()
    {
        var retVal = new JsonSerializerOptions { WriteIndented = true };
        //retVal.Converters.Add(new JsonStringEnumConverter());

        return retVal;
    }

    protected override RectInt32 GetDefaultRectangle() => new(100, 100, 1000, 1000);
}