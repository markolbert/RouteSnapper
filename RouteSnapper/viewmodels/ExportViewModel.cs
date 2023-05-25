using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RouteSnapper;

public class ExportViewModel : ObservableObject
{
    private string? _exportPath;
    private StorageFile? _exportFile;

    public ExportViewModel()
    {
        SelectExportFileCommandAsync = new AsyncRelayCommand( SelectExportFileHandlerAsync );
    }

    public string? ExportPath
    {
        get => _exportPath;
        set => SetProperty( ref _exportPath, value );
    }

    public List<string> ExportTypes { get; } = new();

    public AsyncRelayCommand SelectExportFileCommandAsync { get; }

    private async Task SelectExportFileHandlerAsync()
    {
        var picker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.Desktop, 
            SuggestedFileName = "Snapped Route",

        };

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        picker.SuggestedStartLocation = PickerLocationId.Desktop;
        picker.FileTypeChoices.Add("Gpx", new List<string>(){".gpx"}   );
        picker.FileTypeChoices.Add("Kml", new List<string>(){".kml"});
        picker.FileTypeChoices.Add("Kmz", new List<string>() { ".kmz" });

        _exportFile = await picker.PickSaveFileAsync();
        ExportPath = _exportFile?.Path;
    }
}
