using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class ConsolidateBearingFilter
{
    public ConsolidateBearingFilter()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .FiltersViewModel
            .ConsolidateBearingViewModel;
    }

    public ConsolidateBearingViewModel ViewModel { get; }
}
