using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class MergeRoutesFilter : ObservableObject
{
    private bool _apply;

    public bool Apply
    {
        get => _apply;
        set => SetProperty(ref _apply, value);
    }

    public DistanceViewModel MaximumRouteGap { get; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMaxRouteGapMeters
    };
}
