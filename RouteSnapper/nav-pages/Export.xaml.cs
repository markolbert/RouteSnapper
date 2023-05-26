using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class Export
{
    public Export()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services.GetRequiredService<ExportViewModel>();
    }

    public ExportViewModel ViewModel { get; }
}