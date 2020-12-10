﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace J4JSoftware.KMLProcessor
{
    public static class CoordinateExtensions
    {
        public static Coordinate TargetCoordinate = new Coordinate( 38.49203, -122.65806 );

        public static bool NearTargetPoint( this Coordinate point )
        {
            if( Math.Abs( point.Latitude - TargetCoordinate.Latitude ) < .00001
                && Math.Abs( point.Longitude - TargetCoordinate.Longitude ) < .00001 )
                return true;

            return false;

            //if (GetDistance(TargetCoordinate, point).GetValue(UnitTypes.Miles) < 0.1)
            //    System.Diagnostics.Debugger.Break();
        }

        public static Distance GetDistance(Coordinate c1, Coordinate c2)
        {
            var deltaLat = c2.LatitudeRadians - c1.LatitudeRadians;
            var deltaLong = c2.LongitudeRadians - c1.LongitudeRadians;

            var h1 = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                     Math.Cos(c1.LatitudeRadians) * Math.Cos(c2.LatitudeRadians) *
                     Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);

            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            return new Distance(UnitTypes.Miles, h2 * 3958.8);
        }

        public static double GetBearing(Coordinate c1, Coordinate c2)
        {
            var deltaLongitude = c2.LongitudeRadians - c1.LongitudeRadians;

            var y = Math.Sin(deltaLongitude) * Math.Cos(c2.LatitudeRadians);

            var x = Math.Cos(c1.LatitudeRadians) * Math.Sin(c2.LatitudeRadians)
                    - Math.Sin(c1.LatitudeRadians) * Math.Cos(c2.LatitudeRadians) * Math.Cos(deltaLongitude);

            var theta = Math.Atan2(y, x);

            return (theta.ToDegrees() + 360) % 360;
        }

        public static (double avg, double stdev) GetBearingStatistics( 
            this LinkedListNode<Coordinate> startNode,
            LinkedListNode<Coordinate> endNode )
        {
            var curNode = startNode;

            var bearings = new List<double>();

            while( curNode != endNode )
            {
                bearings.Add( GetBearing( curNode!.Value, curNode.Next!.Value ) );

                curNode = curNode!.Next;
            }

            return ( bearings.Average(), GetStandardDeviation( bearings ) );
        }

        private static double GetStandardDeviation(List<double> values)
        {
            if (values.Count == 0)
                return 0.0;

            var avg = values.Average();
            var sum = values.Sum(d => (d - avg) * (d - avg));

            return Math.Sqrt(sum / values.Count);
        }

        public static double ToRadians( this double degrees ) => degrees * Math.PI / 180;
        public static double ToDegrees( this double radians ) => 180 * radians / Math.PI;
    }
}