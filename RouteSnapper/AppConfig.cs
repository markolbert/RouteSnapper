using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

internal class AppConfig : AppConfigBase
{
    public string ProjectionName { get; set; } = string.Empty;

    [EncryptedProperty]
    public string BingKey { get; set; } = string.Empty;

    [EncryptedProperty]
    public string GoogleKey { get; set; } = string.Empty;

    [EncryptedProperty]
    public string GoogleSignatureSecret { get; set; } = string.Empty;

    [EncryptedProperty]
    public string OpenStreetMapsKey { get; set; } = string.Empty;

    [EncryptedProperty]
    public string OpenTopoMapsKey { get; set; } = string.Empty;

    public string Center { get; set; } = "0,0";
    public double Heading { get; set; }
    public double Scale { get; set; }
}

