using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;

namespace RouteSnapper;

public class SourceFilesViewModel : ObservableObject
{
    private int _selectedSrcIdx;
    private bool _enableSelectClearSrc;

    public SourceFilesViewModel()
    {
        AddSourceFileCommand = new AsyncRelayCommand( AddSourceFileHandlerAsync );
        RemoveSourceFileCommand = new RelayCommand<int>( RemoveSourceFileHandler );
        ClearSelectedSourceFileCommand = new RelayCommand( ClearSelectedSourceFileHandler );
        SelectAllSourceFilesCommand = new RelayCommand( SelectAllSourceFilesHandler );
        ClearAllSourceFilesCommand = new RelayCommand( ClearAllSourceFilesHandler );
    }

    public ObservableCollection<SourceFileInfo> SourceFiles { get; set; } = new();

    [JsonIgnore]
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

    [JsonIgnore]
    public RelayCommand<int> RemoveSourceFileCommand { get; }

    private void RemoveSourceFileHandler( int index )
    {
        if( index < 0 || index >= SourceFiles.Count ) 
            return;

        SourceFiles.RemoveAt( index );
        EnableSelectClearSourceFiles = SourceFiles.Any();
    }

    [JsonIgnore]
    public int SelectedSourceFileIndex
    {
        get => _selectedSrcIdx;
        set => SetProperty( ref _selectedSrcIdx, value );
    }

    [JsonIgnore]
    public RelayCommand ClearSelectedSourceFileCommand { get; }

    private void ClearSelectedSourceFileHandler() => SelectedSourceFileIndex = -1;

    [JsonIgnore]
    public RelayCommand SelectAllSourceFilesCommand { get; }

    private void SelectAllSourceFilesHandler()
    {
        foreach( var file in SourceFiles )
        {
            file.IsSelected = true;
        }

        OnPropertyChanged(nameof(SourceFiles));
    }

    [JsonIgnore]
    public RelayCommand ClearAllSourceFilesCommand { get; }

    private void ClearAllSourceFilesHandler()
    {
        foreach (var file in SourceFiles)
        {
            file.IsSelected = false;
        }

        OnPropertyChanged(nameof(SourceFiles));
    }

}
