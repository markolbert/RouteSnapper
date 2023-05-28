using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class MergeRoutesFilter
{
    public MergeRoutesFilter()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .FiltersViewModel
            .MergeRoutesViewModel;
    }

    public MergeRoutesViewModel ViewModel { get; }
}
