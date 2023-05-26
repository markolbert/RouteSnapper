using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RouteSnapper;

public class EngineViewModel : ObservableObject
{
    private string? _selectedSnapper;

    public EngineViewModel()
    {
        Snappers = new List<string> { Constants.BingSnapper, Constants.GoogleSnapper };
    }

    public List<string> Snappers { get; }

    public string? SelectedSnapper
    {
        get=> _selectedSnapper;
        set => SetProperty( ref _selectedSnapper, value );
    }
}
