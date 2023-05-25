using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RouteSnapper;

public class MainViewModel : ObservableObject
{
    private string? _exportPath;
    private int _selectedSrcIdx;
    private bool _enableSelectClearSrc;
    private StorageFile? _exportFile;

    public MainViewModel()
    {
        AddSourceFileCommand = new AsyncRelayCommand( AddSourceFileHandlerAsync );
        RemoveSourceFileCommand = new RelayCommand<int>( RemoveSourceFileHandler );
        ClearSelectedSourceFileCommand = new RelayCommand( ClearSelectedSourceFileHandler );
        SelectAllSourceFilesCommand = new RelayCommand( SelectAllSourceFilesHandler );
        ClearAllSourceFilesCommand = new RelayCommand( ClearAllSourceFilesHandler );
        SelectExportFileCommandAsync = new AsyncRelayCommand( SelectExportFileHandlerAsync );
        ExportCommand = new RelayCommand( ExportHandler );
        ClearMessagesCommand = new RelayCommand( ClearMessagesHandler );
    }

    public ObservableCollection<SourceFileInfo> SourceFiles { get; } = new();
    public ObservableCollection<string> Messages { get; } = new();

    public string? ExportPath
    {
        get => _exportPath;
        set => SetProperty( ref _exportPath, value );
    }

    public List<string> ExportTypes { get; } = new();

    public AsyncRelayCommand AddSourceFileCommand { get; }

    private async Task AddSourceFileHandlerAsync()
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.List, 
            SuggestedStartLocation = PickerLocationId.Desktop
        };

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        picker.FileTypeFilter.Add(".gpx");
        picker.FileTypeFilter.Add(".kml");
        picker.FileTypeFilter.Add(".kmz");

        var files = await picker.PickMultipleFilesAsync();

        foreach( var file in files )
        {
            if( SourceFiles.Any( x => x.Path?.Equals( file.Path, StringComparison.OrdinalIgnoreCase ) ?? false ) )
                continue;

            SourceFiles.Add( new SourceFileInfo { IsSelected = true, Path = file.Path } );
        }

        EnableSelectClearSourceFiles = SourceFiles.Any();
    }

    public bool EnableSelectClearSourceFiles
    {
        get => _enableSelectClearSrc;
        set => SetProperty( ref _enableSelectClearSrc, value );
    }

    public RelayCommand<int> RemoveSourceFileCommand { get; }

    private void RemoveSourceFileHandler( int index )
    {
        if( index < 0 || index >= SourceFiles.Count ) 
            return;

        SourceFiles.RemoveAt( index );
        EnableSelectClearSourceFiles = SourceFiles.Any();
    }

    public int SelectedSourceFileIndex
    {
        get => _selectedSrcIdx;
        set => SetProperty( ref _selectedSrcIdx, value );
    }

    public RelayCommand ClearSelectedSourceFileCommand { get; }

    private void ClearSelectedSourceFileHandler() => SelectedSourceFileIndex = -1;

    public RelayCommand SelectAllSourceFilesCommand { get; }

    private void SelectAllSourceFilesHandler()
    {
        foreach( var file in SourceFiles )
        {
            file.IsSelected = true;
        }

        OnPropertyChanged(nameof(SourceFiles));
    }

    public RelayCommand ClearAllSourceFilesCommand { get; }

    private void ClearAllSourceFilesHandler()
    {
        foreach (var file in SourceFiles)
        {
            file.IsSelected = false;
        }

        OnPropertyChanged(nameof(SourceFiles));
    }

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

    public RelayCommand ExportCommand { get; }

    private void ExportHandler()
    {
    }

    public RelayCommand ClearMessagesCommand { get; }

    private void ClearMessagesHandler()
    {
    }
}
