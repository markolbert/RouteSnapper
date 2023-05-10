﻿using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.GeoProcessor;

[BeforeUserFilters(DefaultFilterName, 50)]
public class ConsolidatePoints : ImportFilter
{
    public const string DefaultFilterName = "Consolidate Points";

    private Distance2 _minSep = new( UnitType.Meters, GeoConstants.DefaultMinimumPointGapMeters );
    private Distance2 _maxOverallGap = new( UnitType.Meters, GeoConstants.DefaultMaximumOverallGapMeters );

    public ConsolidatePoints(
        ILoggerFactory? loggerFactory
    )
    : base( loggerFactory )
    {
    }

    public Distance2 MinimumPointGap
    {
        get => _minSep;

        set =>
            _minSep = value.Value < 0
                ? new Distance2( UnitType.Meters, GeoConstants.DefaultMinimumPointGapMeters )
                : value;
    }

    public Distance2 MaximumOverallGap
    {
        get => _maxOverallGap;

        set =>
            _maxOverallGap = value.Value < 0
                ? new Distance2( UnitType.Meters, GeoConstants.DefaultMaximumOverallGapMeters )
                : value;
    }

    public override List<IImportedRoute> Filter( List<IImportedRoute> input )
    {
        var retVal = new List<IImportedRoute>();

        foreach( var rawRoute in input )
        {
            Coordinate2? prevPoint = null;
            Coordinate2? originPoint = null;

            var filteredRoute = new ImportedRoute()
            {
                RouteName = rawRoute.RouteName, 
                Description = rawRoute.Description
            };

            foreach( var curPoint in rawRoute )
            {
                if( prevPoint == null || originPoint == null )
                {
                    filteredRoute.Points.Add( curPoint );
                    continue;
                }

                var curPair = new PointPair( prevPoint, curPoint );
                var curGap = curPair.GetDistance();

                var originPair = new PointPair( originPoint, curPoint );
                var originGap = originPair.GetDistance();
                
                prevPoint = curPoint;

                if( curGap > MinimumPointGap )
                {
                    filteredRoute.Points.Add(curPoint);
                    originPoint = curPoint;

                    continue;
                }

                Logger?.LogTrace("Points within minimum gap: ({lat1}, {long1}), ({lat2}, {long2})",
                                  prevPoint.Latitude,
                                  prevPoint.Longitude,
                                  curPoint.Latitude,
                                  curPoint.Longitude);

                if (originGap >= MaximumOverallGap)
                {
                    filteredRoute.Points.Add(curPoint);
                    originPoint = curPoint;
                    continue;
                }

                Logger?.LogTrace("Points within maximum gap: ({lat1}, {long1}), ({lat2}, {long2})",
                                  originPoint.Latitude,
                                  originPoint.Longitude,
                                  curPoint.Latitude,
                                  curPoint.Longitude);
            }

            if( filteredRoute.Points.Count > 1 )
                retVal.Add( filteredRoute );
            else
                Logger?.LogInformation( "Route {name} {text}, excluding",
                                         filteredRoute.RouteName,
                                         filteredRoute.Points.Count switch
                                         {
                                             0 => "has no points",
                                             _ => "has only 1 point"
                                         } );
        }

        return retVal;
    }
}
