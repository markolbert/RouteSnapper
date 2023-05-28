using System;
using System.Collections.Generic;
using ABI.Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

        SelectConfigurationCommand = new RelayCommand<string>(SelectConfigurationHandler);
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

    public RelayCommand<string> SelectConfigurationCommand { get; }

    private void SelectConfigurationHandler(string? cmd) =>
        WeakReferenceMessenger.Default.Send(new MainMenuSelection(cmd ?? "intro"));
}