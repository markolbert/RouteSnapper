using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

internal class AppConfig : AppConfigBase
{
    [EncryptedProperty]
    public string BingKey { get; set; } = string.Empty;

    [EncryptedProperty]
    public string GoogleKey { get; set; } = string.Empty;
}

