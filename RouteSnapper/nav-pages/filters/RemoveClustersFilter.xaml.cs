using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class RemoveClustersFilter
{
    public RemoveClustersFilter()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .FiltersViewModel
            .RemoveClustersViewModel;
    }

    public RemoveClustersViewModel ViewModel { get; }
}
