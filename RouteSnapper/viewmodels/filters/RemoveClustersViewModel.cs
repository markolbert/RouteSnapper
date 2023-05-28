using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class RemoveClustersViewModel : ObservableObject
{
    private bool _apply;

    public bool Apply
    {
        get => _apply;
        set => SetProperty(ref _apply, value);
    }

    public DistanceViewModel MaximumClusterRadius { get; set; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMaxClusterRadiusMeters
    };
}
