using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper.nav_pages;

public sealed partial class SnapperEngine
{
    public SnapperEngine()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services
            .GetRequiredService<AppConfig>()
            .EngineViewModel;
    }

    public EngineViewModel ViewModel { get; }
}
