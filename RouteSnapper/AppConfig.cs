using System.IO;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json.Serialization;
using Windows.Foundation;

namespace RouteSnapper;

internal class AppConfig
{
    public static string UserFolder { get; } = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

    [JsonIgnore]
    public string? UserConfigurationFilePath { get; set; }

    [JsonIgnore ]
    public bool UserConfigurationFileExists =>
        !string.IsNullOrEmpty( UserConfigurationFilePath )
     && File.Exists( Path.Combine( UserFolder, UserConfigurationFilePath ) );

    public Point UpperLeft { get; set; }
    public Size Size { get; set; }
    public string BingKey { get; set; } = string.Empty;
    public string GoogleKey { get; set; } = string.Empty;

    public AppConfig Encrypt(IDataProtector protector)
    {
        var retVal = new AppConfig
        {
            UserConfigurationFilePath = UserConfigurationFilePath,
        };

        if (!string.IsNullOrEmpty(BingKey))
            retVal.BingKey = protector.Protect(BingKey);

        if (!string.IsNullOrEmpty(GoogleKey))
            retVal.GoogleKey = protector.Protect(GoogleKey);

        return retVal;
    }

    public AppConfig Decrypt(IDataProtector protector)
    {
        var retVal = new AppConfig
        {
            UserConfigurationFilePath = UserConfigurationFilePath,
        };

        if( !string.IsNullOrEmpty( BingKey ) )
            retVal.BingKey = protector.Unprotect( BingKey );

        if( !string.IsNullOrEmpty( GoogleKey ) )
            retVal.GoogleKey = protector.Unprotect( GoogleKey );

        return retVal;
    }
}
