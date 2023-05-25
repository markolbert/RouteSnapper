using CommunityToolkit.Mvvm.ComponentModel;

namespace RouteSnapper;

public class SourceFileInfo : ObservableObject
{
    private bool _isSelected;
    private string? _path;

    public string? FileName => System.IO.Path.GetFileName( Path );

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty( ref _isSelected, value );
    }

    public string? Path
    {
        get => _path;

        set
        {
            SetProperty( ref _path, value );
            OnPropertyChanged( nameof( FileName ) );
        }
    }
}
