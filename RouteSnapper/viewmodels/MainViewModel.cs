using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ABI.Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace RouteSnapper;

public class MainViewModel : ObservableObject
{
    private bool _lockUiWhenBuilding;
    private bool _uiLocked;
    private bool _lockRequested;

    public MainViewModel()
    {
        var appConfig = App.Current.Services.GetRequiredService<AppConfig>();

        MainMenuItems.Add(new MainMenuItem("Intro/Help", "intro")  );
        MainMenuItems.Add(new MainMenuItem("Source Files", "source"));
        MainMenuItems.Add(new MainMenuItem("Filters", "filters"));
        MainMenuItems.Add(new MainMenuItem("Snapping Engine", "engine"));
        MainMenuItems.Add(new MainMenuItem("Export Targets", "export"));
    }

    public bool LockUiWhenBuilding
    {
        get => _lockUiWhenBuilding;

        set
        {
            SetProperty(ref _lockUiWhenBuilding, value);

            if (!LockRequested)
                return;

            UiLocked = value;
            LockRequested = value;
        }
    }

    public bool LockRequested
    {
        get => _lockRequested;

        set
        {
            SetProperty(ref _lockRequested, value);

            if (LockUiWhenBuilding)
                UiLocked = value;
        }
    }

    public bool UiLocked
    {
        get => _uiLocked;
        private set => SetProperty(ref _uiLocked, value);
    }

    public ObservableCollection<MainMenuItem> MainMenuItems { get; } = new();
}