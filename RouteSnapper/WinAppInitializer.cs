using System.IO;
using J4JSoftware.J4JMapLibrary;
using J4JSoftware.J4JMapWinLibrary;
using J4JSoftware.WindowsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace RouteSnapper;

internal class WinAppInitializer : WinAppInitializerBase<AppConfig>
{
    public WinAppInitializer(
        IWinApp winApp
        )
        : base( winApp, jsonOptions:MainWinSerializer.CreateJsonOptions() )
    {
    }

    protected override LoggerConfiguration? GetSerilogConfiguration()
    {
        var logFile = Path.Combine(AppConfigBase.UserFolder, "log.txt");

        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Debug()
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Hour);
    }

    protected override IServiceCollection ConfigureServices(HostBuilderContext hbc, IServiceCollection services)
    {
        base.ConfigureServices(hbc, services);

        services.AddSingleton(new ProjectionFactory(LoggerFactory));
        services.AddSingleton(new CredentialsFactory(hbc.Configuration, LoggerFactory));
        services.AddSingleton(new CredentialsDialogFactory(LoggerFactory));

        //services.AddSingleton(sp =>
        //{
        //    var appConfig = sp.GetRequiredService<AppConfig>();
        //    return appConfig.SourceFilesViewModel;
        //});

        //services.AddSingleton(sp =>
        //{
        //    var appConfig = sp.GetRequiredService<AppConfig>();
        //    return appConfig.FiltersViewModel;
        //});

        //services.AddSingleton(sp =>
        //{
        //    var appConfig = sp.GetRequiredService<AppConfig>();
        //    return appConfig.ExportViewModel;
        //});

        //services.AddSingleton(sp =>
        //{
        //    var appConfig = sp.GetRequiredService<AppConfig>();
        //    return appConfig.EngineViewModel;
        //});

        return services;
    }
}