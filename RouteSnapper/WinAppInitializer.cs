using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J4JSoftware.WindowsUtilities;
using Serilog;

namespace RouteSnapper;

internal class WinAppInitializer : WinAppInitializerBase<App, AppConfig>
{
    public WinAppInitializer()
        : base( "userConfig.json" )
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
}