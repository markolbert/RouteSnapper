using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class ConsolidateBearingViewModel : ObservableObject
{
    private bool _apply;
    private double _bearingTolerance = GeoConstants.DefaultBearingToleranceDegrees;

    public bool Apply
    {
        get => _apply;
        set => SetProperty(ref _apply, value);
    }

    public double BearingTolerance
    {
        get => _bearingTolerance;
        set => SetProperty( ref _bearingTolerance, value );
    }

    public DistanceViewModel MaxConsolidationDistance { get; set; } =
        new() { DistanceUnit = UnitType.Kilometers, DistanceValue = GeoConstants.DefaultMaxPointSeparationKm };
}
