using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class SourceFiles
{
    public SourceFiles()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services.GetRequiredService<SourceFilesViewModel>();
    }

    public SourceFilesViewModel ViewModel { get; }
}
