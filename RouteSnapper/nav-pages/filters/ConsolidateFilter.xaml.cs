using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class ConsolidateFilter
{
    public ConsolidateFilter()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .FiltersViewModel
            .ConsolidatePointsViewModel;
    }

    public ConsolidatePointsViewModel ViewModel { get; }
}
