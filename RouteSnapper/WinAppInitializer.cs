using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J4JSoftware.WindowsUtilities;
using Serilog;

namespace RouteSnapper;

internal class WinAppInitializer : WinAppInitializerBase<AppConfig>
{
    public WinAppInitializer(
        IWinApp winApp
        )
        : base( winApp )
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