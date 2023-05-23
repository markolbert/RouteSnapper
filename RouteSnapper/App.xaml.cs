// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using Microsoft.UI.Xaml;

namespace RouteSnapper;

public partial class App
{
    internal new static App Current => (App) Application.Current;

    public App()
    {
        this.InitializeComponent();

        if (!AppInitializer.Initialize())
            Exit();

        Services = AppInitializer.Services!;
    }

    internal WinAppInitializer AppInitializer { get; } = new();
    internal IServiceProvider Services { get; }

    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        var window = new MainWindow();
        window.Activate();
    }
}
