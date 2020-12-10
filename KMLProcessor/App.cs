﻿using System.Threading;
using System.Threading.Tasks;
using J4JSoftware.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace J4JSoftware.KMLProcessor
{
    public class App : IHostedService
    {
        private readonly IHost _host;
        private readonly IAppConfig _config;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IJ4JLogger _logger;

        public App( 
            IHost host,
            IAppConfig config,
            IHostApplicationLifetime lifetime,
            IJ4JLogger logger
        )
        {
            _host = host;
            _config = config;
            _lifetime = lifetime;

            _logger = logger;
            _logger.SetLoggedType( this.GetType() );
        }

        public async Task StartAsync( CancellationToken cancellationToken )
        {
            if( !_config.IsValid( out var error ) )
            {
                _logger.Fatal(error!);
                _lifetime.StopApplication();

                return;
            }

            var kDoc = _host.Services.GetRequiredService<KmlDocument>();

            if( !await kDoc.LoadAsync( _config.InputFile, cancellationToken ) )
                return;

            var numCoalesced = ( _config.CoalesenceTypes & CoalesenceTypes.Distance ) == CoalesenceTypes.Distance
                ? kDoc.CoalescePointsByDistance( _config.CoalesenceDistance )
                : 0;

            _logger.Information( "Coalesced {0:n0} points based on distance", numCoalesced );

            numCoalesced = ( _config.CoalesenceTypes & CoalesenceTypes.Distance ) == CoalesenceTypes.Distance
                ? kDoc.CoalescePointsByBearing( _config.MaxBearingDelta )
                : 0;

            _logger.Information("Coalesced {0:n0} points based on bearing", numCoalesced);

            _logger.Information( "{0:n0} points are in the track before route binding", kDoc.Count );

            numCoalesced = await kDoc.SnapToRoute( cancellationToken );

            if( numCoalesced == 0 )
                _logger.Error( "Writing un-snapped points to file" );
            else _logger.Information( "Route expanded to include {0:n0} points", numCoalesced );

            if (!await kDoc.SaveAsync(_config.OutputFile, cancellationToken))
                return;

            _logger.Information<string>("Wrote file '{0}'", _config.OutputFile);

            _lifetime.StopApplication();
        }

        public async Task StopAsync( CancellationToken cancellationToken )
        {
        }
    }
}