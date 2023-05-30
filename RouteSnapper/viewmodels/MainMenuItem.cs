using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace RouteSnapper;

public class MainMenuItem
{
    public MainMenuItem(string title, string tag)
    {
        Title = title;
        Tag = tag;
    }

    public RelayCommand<string> SendMenuSelectionCommand { get; } = new( SendMenuSelectionHandler );
    public string Title { get; }
    public string Tag { get; }

    private static void SendMenuSelectionHandler( string? cmd ) =>
        WeakReferenceMessenger.Default.Send( new MainMenuSelectionMessage( cmd ?? "intro" ) );
}
