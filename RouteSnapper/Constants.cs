using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public static class Constants
{
    public const string GoogleSnapper = "Google";
    public const string BingSnapper = "Bing";

    public static List<UnitType> MeasurementUnits { get; } = Enum.GetValues<UnitType>().ToList();
}
