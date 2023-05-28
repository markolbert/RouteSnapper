using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

internal class AppConfig : AppConfigBase
{
    public EngineViewModel EngineViewModel { get; set; } = new();
    public MapViewModel MapViewModel { get; set; } = new();
    public ExportViewModel ExportViewModel { get; set; } = new();
    public FiltersViewModel FiltersViewModel { get; set; } = new();
    public SourceFilesViewModel SourceFilesViewModel { get; set; } = new();
}