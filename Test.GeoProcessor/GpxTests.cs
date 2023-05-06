﻿using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using J4JSoftware.GeoProcessor;
using Microsoft.Extensions.DependencyInjection;
using J4JSoftware.GeoProcessor.RouteBuilder;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Test.GeoProcessor;

public class GpxTests : TestBase
{
    [ Theory ]
    [ InlineData( "Stinson Beach 2023-4-27.gpx" ) ]
    public async Task CreateBuilder( string fileName )
    {
        var path = Path.Combine( Environment.CurrentDirectory, "gpx", fileName );
        File.Exists( path ).Should().BeTrue();

        var routeBuilder = Services.GetService<RouteBuilder>();
        routeBuilder.Should().NotBeNull();

        routeBuilder!.UseProcessor( "Bing", Config.BingKey )
                     .AddSourceFile( path, "gpx" )
                     .SendStatusReportsTo( LogStatus )
                     .SendMessagesTo( LogMessage );

        var result = await routeBuilder!.BuildAsync( "Test Route" );

        result.Status.Should().NotBe( ProcessRouteStatus.NoResults );
        result.Status.Should().NotBe( ProcessRouteStatus.AllFailed );
    }

    [ Theory ]
    [InlineData(37.4596, -122.28607, 37.44124, -122.2508,3,3.723)]
    public void TestHaversine(double lat1, double long1, double lat2, double long2, int rounding, double result)
    {
        var distance = GeoExtensions.GetDistance( lat1, long1,lat2, long2 );
        Math.Round( distance, rounding ).Should().Be( result );
    }
}