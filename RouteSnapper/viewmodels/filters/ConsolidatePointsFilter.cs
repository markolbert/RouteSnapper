using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class ConsolidatePointsFilter : ObservableObject
{
    private bool _apply;

    public bool Apply
    {
        get => _apply;
        set => SetProperty(ref _apply, value);
    }

    public DistanceViewModel MinimumPointGap { get; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMinimumPointGapMeters
    };

    public DistanceViewModel MaximumOverallGap { get; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMaximumOverallGapMeters
    };
}
