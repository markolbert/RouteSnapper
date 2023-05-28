using System.Collections.Generic;
using System.Text.Json.Serialization;
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

    [JsonIgnore]
    public List<UnitType> DistanceUnits { get; set; } = new( Constants.MeasurementUnits );

    public UnitType DistanceUnit
    {
        get => _distUnit;
        set => SetProperty( ref _distUnit, value );
    }
}
