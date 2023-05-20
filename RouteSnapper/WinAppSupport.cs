using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J4JSoftware.WindowsUtilities;
using Serilog;

namespace RouteSnapper;

internal class WinAppSupport : J4JWinAppSupport<App, AppConfig>
{
    public WinAppSupport()
    :base("userConfig.json")
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