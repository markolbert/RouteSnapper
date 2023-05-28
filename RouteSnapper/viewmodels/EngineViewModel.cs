using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

public class EngineViewModel : ObservableObject
{
    private string? _selectedSnapper;

    public EngineViewModel()
    {
        Snappers = new List<string> { Constants.BingSnapper, Constants.GoogleSnapper };
    }

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

    [JsonIgnore]
    public List<string> Snappers { get; }

    public string? SelectedSnapper
    {
        get=> _selectedSnapper;
        set => SetProperty( ref _selectedSnapper, value );
    }
}
