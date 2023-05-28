// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using J4JSoftware.J4JMapLibrary;
using J4JSoftware.J4JMapWinLibrary;
using J4JSoftware.WindowsUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using RouteSnapper.nav_pages;

namespace RouteSnapper;

public sealed partial class MainWindow
{
    private readonly AppConfig? _appConfig;
    private readonly Dictionary<string, Type> _pageTypes = new( StringComparer.OrdinalIgnoreCase );

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

        _pageTypes.Add( "intro", typeof( IntroHelp ) );
        _pageTypes.Add( "source", typeof( SourceFiles ) );
        _pageTypes.Add( "filters", typeof( Filters ) );
        _pageTypes.Add( "engine", typeof( SnapperEngine ) );
        _pageTypes.Add( "export", typeof( Export ) );
        _pageTypes.Add( "messages", typeof( Messages ) );
    }

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

    private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
        contentFrame.Navigated += ContentFrame_Navigated;
        navView.SelectedItem = navView.MenuItems[ 0 ];
        NavigateTo( typeof( IntroHelp ), null );
    }

    private void NavigateTo( Type pageType, NavigationTransitionInfo? transitionInfo )
    {
        var preNavPageType = contentFrame.CurrentSourcePageType;

        // Only navigate if the selected page isn't currently loaded.
        if (!Type.Equals(preNavPageType, pageType))
            contentFrame.Navigate(pageType, null, transitionInfo);
    }

    private void ContentFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        navView.IsBackEnabled = contentFrame.CanGoBack;

        //if (contentFrame.SourcePageType == typeof(SettingsPage))
        //{
        //    // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
        //    navView.SelectedItem = (NavigationViewItem)navView.SettingsItem;
        //    navView.Header = "Settings";
        //    return;
        //}

        if( contentFrame.SourcePageType == null )
            return;

        // Select the nav view item that corresponds to the page being navigated to.
        navView.SelectedItem = navView.MenuItems
                                      .OfType<NavigationViewItem>()
                                      .First( i => i.Tag.Equals( ((Page) contentFrame.Content).Tag ) );

        navView.Header = ( (NavigationViewItem) navView.SelectedItem )?.Content?.ToString();
    }

    private void NavigationView_OnItemInvoked( NavigationView sender, NavigationViewItemInvokedEventArgs args )
    {
        //if( args.IsSettingsInvoked == true )
        //{
        //    NavigateTo( typeof( SettingsPage ), args.RecommendedNavigationTransitionInfo );
        //    return;
        //}

        if( !_pageTypes.TryGetValue( args.InvokedItemContainer.Tag?.ToString() ?? string.Empty, out var pageType ) )
            return;

        NavigateTo( pageType, args.RecommendedNavigationTransitionInfo );
    }

    private void NavigationView_OnBackRequested( NavigationView sender, NavigationViewBackRequestedEventArgs args )
    {
        TryGoBack();
    }

    private void TryGoBack()
    {
        if( !contentFrame.CanGoBack )
            return;

        // Don't go back if the nav pane is overlayed.
        if (navView.IsPaneOpen &&
            navView.DisplayMode is NavigationViewDisplayMode.Compact or NavigationViewDisplayMode.Minimal)
            return;

        contentFrame.GoBack();
    }
}
