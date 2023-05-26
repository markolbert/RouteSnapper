using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class DistanceViewModel : ObservableObject
{
    private double _distValue;
    private UnitType _distUnit;

    public double DistanceValue
    {
        get => _distValue;
        set => SetProperty( ref _distValue, value );
    }

    public List<UnitType> DistanceUnits { get; } = new( Constants.MeasurementUnits );

    public UnitType DistanceUnit
    {
        get => _distUnit;
        set => SetProperty( ref _distUnit, value );
    }
}
