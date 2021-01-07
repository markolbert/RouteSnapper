﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoogleApi.Entities.Maps.Roads.Common;
using GoogleApi.Entities.Maps.Roads.SnapToRoads.Request;
using J4JSoftware.Logging;
using Location = GoogleApi.Entities.Common.Location;

namespace J4JSoftware.GeoProcessor
{
    [RouteProcessor(ProcessorType.Google)]
    public class GoogleProcessor : CloudRouteProcessor
    {
        public GoogleProcessor(
            IImportConfig config,
            IJ4JLogger? logger
        )
            : base( config, logger )
        {
            Type = GeoExtensions.GetTargetType<RouteProcessorAttribute>(GetType())!.Type;
        }

        public ProcessorType Type { get; }

        protected override async Task<List<Coordinate>?> ExecuteRequestAsync( 
            List<Coordinate> coordinates,
            CancellationToken cancellationToken )
        {
            var request = new SnapToRoadsRequest
            {
                Interpolate = true,
                Key = APIKey,
                Path = coordinates.Select( c => new Location( c.Latitude, c.Longitude ) )
            };

            var result = await GoogleApi.GoogleMaps.SnapToRoad.QueryAsync( request, cancellationToken );

            if( result == null )
            {
                Logger?.Error("Snap to road request failed");
                return null;
            }

            var errors = result.Errors?.ToList() ?? new List<Error>();

            if( errors.Count <= 0 )
                return result.SnappedPoints
                    .Select( p => new Coordinate( p.Location.Latitude, p.Location.Longitude ) )
                    .ToList();

            foreach( var error in errors )
            {
                Logger?.Error<string>( "Snap to road error: {0}", error.ErrorMessage );
            }

            return null;
        }
    }
}