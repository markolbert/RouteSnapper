using CommunityToolkit.Mvvm.ComponentModel;

namespace RouteSnapper;

public class FiltersViewModel : ObservableObject
{
    private bool _removeGarmin;

    public ConsolidatePointsViewModel ConsolidatePointsViewModel { get; set; } = new();
    public ConsolidateBearingViewModel ConsolidateBearingViewModel { get; set; } = new();
    public MergeRoutesViewModel MergeRoutesViewModel { get; set; } = new();
    public RemoveClustersViewModel RemoveClustersViewModel { get; set; } = new();

    public bool RemoveGarminMessages
    {
        get => _removeGarmin;
        set => SetProperty( ref _removeGarmin, value );
    }
}
