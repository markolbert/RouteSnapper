// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RouteSnapper;

public partial class App
{
    public new static App Current => (App) Application.Current;

    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;
    private readonly IDataProtector _dataProtector;
    private AppConfig? _appConfig;

#pragma warning disable CS8618
    public App()
#pragma warning restore CS8618
    {
        this.InitializeComponent();

        var logFile = Path.Combine( AppConfig.UserFolder, "log.txt" );

        var seriLogger = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .WriteTo.Debug()
                        .WriteTo.File( logFile, rollingInterval: RollingInterval.Hour )
                        .CreateLogger();

        _loggerFactory = new LoggerFactory().AddSerilog( seriLogger );
        _logger = _loggerFactory.CreateLogger<App>();

        var localAppFolder = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );

        var dpProvider = DataProtectionProvider.Create(
            new DirectoryInfo( Path.Combine( localAppFolder, "ASP.NET", "DataProtection-Keys" ) ) );
        _dataProtector = dpProvider.CreateProtector( nameof( RouteSnapper ) );

        var configPath = Path.Combine( AppConfig.UserFolder, "userConfig.json" );

        try
        {
            Services = new HostBuilder()
                      .ConfigureHostConfiguration( builder => ParseUserConfigFile( configPath, builder ) )
                      .ConfigureServices( ( hbc, s ) => ConfigureServices( hbc, s ) )
                      .Build()
                      .Services;
        }
        catch( CryptographicException crypto )
        {
            var logger = _loggerFactory.CreateLogger<App>();
            logger.LogError( "Cryptographic error '{mesg}', deleting user configuration file '{path}'",
                             crypto.Message,
                             configPath );

            File.Delete( configPath );

            Exit();
        }
        catch( JsonException )
        {
            Exit();
        }
    }

    public IServiceProvider Services { get; }

#pragma warning disable IDE0060
    // ReSharper disable once UnusedMethodReturnValue.Local
    // ReSharper disable once UnusedParameter.Local
    private IServiceCollection ConfigureServices( HostBuilderContext hbc, IServiceCollection services )
#pragma warning restore IDE0060
    {
        services.AddSingleton( _loggerFactory );
        services.AddSingleton( _dataProtector );

        services.AddSingleton( _appConfig! );

        return services;
    }

    // ReSharper disable once UnusedParameter.Local
    private void ParseUserConfigFile( string path, IConfigurationBuilder builder )
    {
        var fileExists = File.Exists( path );
        if( !fileExists )
        {
            _logger.LogWarning( "Could not find user config file '{path}', creating default configuration", path );
            _appConfig = new AppConfig { UserConfigurationFilePath = path };

            return;
        }

        var encrypted = JsonSerializer.Deserialize<AppConfig>( File.ReadAllText( path ) );

        if( encrypted == null )
        {
            _logger.LogError( "Could not parse user config file '{path}'", path );
            throw new JsonException( $"Could not parse user config file '{path}'" );
        }

        encrypted.UserConfigurationFilePath = path;

        _appConfig = encrypted.Decrypt( _dataProtector );
        _appConfig.UserConfigurationFilePath = path;
    }

    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        var window = new MainWindow();
        window.Activate();
    }
}
