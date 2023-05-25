// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using J4JSoftware.WindowsUtilities;
using Microsoft.UI.Xaml;

namespace RouteSnapper;

public partial class App : IWinApp
{
    internal new static App Current => (App) Application.Current;

#pragma warning disable CS8618
    public App()
#pragma warning restore CS8618
    {
        this.InitializeComponent();

        var appInitializer = new WinAppInitializer( this );
        if (!appInitializer.Initialize())
            Exit();
    }

    public MainWindow? MainWindow { get; private set; }
    public IServiceProvider Services { get; set; }
    public bool SaveConfigurationOnExit { get; set; } = true;

    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }
}
