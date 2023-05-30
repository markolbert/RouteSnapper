// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using J4JSoftware.J4JMapLibrary;
using J4JSoftware.J4JMapWinLibrary;
using J4JSoftware.WindowsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using RouteSnapper.nav_pages;

namespace RouteSnapper;

public sealed partial class MainWindow
{
    private record MainMenuItem(string Title, UserControl Control);

    private readonly Dictionary<string, MainMenuItem> _mainMenuItems = new(StringComparer.OrdinalIgnoreCase);
    private readonly AppConfig? _appConfig;

    public MainWindow()
    {
        var winSupport = new MainWinSerializer( this );
        MapControlViewModelLocator.Initialize( App.Current.Services );

        this.InitializeComponent();

        winSupport.SetSizeAndPosition();

        mapControl.FileSystemCachePath = Path.Combine( AppConfigBase.UserFolder, "map-cache" );
        mapControl.NewCredentials += MapControlOnNewCredentials;

        _appConfig = App.Current.Services.GetService<AppConfig>();

        if( _appConfig is { UserConfigurationFileExists: true } )
        {
            mapControl.MapProjection = string.IsNullOrEmpty( _appConfig.MapViewModel.ProjectionName )
                ? "BingMaps"
                : _appConfig.MapViewModel.ProjectionName;

            mapControl.Center = _appConfig.MapViewModel.Center;
            mapControl.Heading = _appConfig.MapViewModel.Heading;
            mapControl.MapScale = _appConfig.MapViewModel.Scale;
        }
        else
        {
            mapControl.MapProjection = "BingMaps";
            mapControl.Center = "37.5072N,122.2605W";
        }

        ViewModel = new MainViewModel();

        _mainMenuItems.Add("intro", new MainMenuItem("Intro/Help", new IntroHelp()));
        _mainMenuItems.Add("source", new MainMenuItem("Source Files", new SourceFiles()));
        _mainMenuItems.Add("filters", new MainMenuItem("Filters", new Filters()));
        _mainMenuItems.Add("engine", new MainMenuItem("Snapping Engine", new SnapperEngine()));
        _mainMenuItems.Add("export", new MainMenuItem("Export Targets", new Export()));

        menuItems.SelectedIndex = 0;
        menuItemHeader.Text = _mainMenuItems["intro"].Title;
        contentFrame.Content = _mainMenuItems["intro"].Control;

        WeakReferenceMessenger.Default.Register<MainMenuSelectionMessage>(this, MenuMenuSelectionHandler);
    }

    private void MenuMenuSelectionHandler(object recipient, MainMenuSelectionMessage message)
    {
        var menuItem = _mainMenuItems.TryGetValue(message.MenuItem, out var contentControl)
            ? contentControl
            : _mainMenuItems["intro"];

        menuItemHeader.Text = menuItem.Title;
        contentFrame.Content = menuItem.Control;
        menuItems.SelectedIndex = _mainMenuItems.Keys.ToList().IndexOf(message.MenuItem);
    }

    public MainViewModel ViewModel { get; }

    private void MapControlOnNewCredentials( object? sender, NewCredentialsEventArgs e )
    {
        if( _appConfig == null )
            return;

        _appConfig.MapViewModel.ProjectionName = e.ProjectionName;

        foreach( var credProp in e.Credentials
                                  .CredentialProperties
                                  .Where( x => x.Value?.ToString() != null ) )
        {
            switch( e.ProjectionName.ToLower() )
            {
                case "bingmaps":
                    _appConfig.EngineViewModel.BingKey = (string) credProp.Value!;
                    break;

                case "googlemaps":
                    switch( credProp.PropertyName )
                    {
                        case nameof( GoogleCredentials.ApiKey ):
                            _appConfig.EngineViewModel.GoogleKey = (string) credProp.Value!;
                            break;

                        case nameof( GoogleCredentials.SignatureSecret ):
                            _appConfig.EngineViewModel.GoogleSignatureSecret = (string) credProp.Value!;
                            break;
                    }

                    break;

                case "openstreetmaps":
                    _appConfig.EngineViewModel.OpenStreetMapsKey = (string) credProp.Value!;
                    break;

                case "opentopomaps":
                    _appConfig.EngineViewModel.OpenTopoMapsKey = (string) credProp.Value!;
                    break;
            }
        }
    }
}
