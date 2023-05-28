using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class RemoveGarminMessagesFilter
{
    public RemoveGarminMessagesFilter()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .FiltersViewModel;
    }

    public FiltersViewModel ViewModel { get; }
}
