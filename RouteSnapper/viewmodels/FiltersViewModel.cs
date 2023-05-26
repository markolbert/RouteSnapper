using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using ABI.Windows.Devices.SmartCards;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RouteSnapper;

public class FiltersViewModel : ObservableObject
{
    private string? _selectedSnapper;
    private bool _removeGarmin;

    public FiltersViewModel()
    {
        Snappers = new List<string> { Constants.BingSnapper, Constants.GoogleSnapper };
    }

    public List<string> Snappers { get; }

    public string? SelectedSnapper
    {
        get=> _selectedSnapper;
        set => SetProperty( ref _selectedSnapper, value );
    }

    public ConsolidatePointsFilter ConsolidatePointsFilter { get; } = new();
    public ConsolidateBearingFilter ConsolidateBearingFilter { get; } = new();
    public MergeRoutesFilter MergeRoutesFilter { get; } = new();
    public RemoveClustersFilter RemoveClustersFilter { get; } = new();

    public bool RemoveGarminMessages
    {
        get => _removeGarmin;
        set => SetProperty( ref _removeGarmin, value );
    }
}
