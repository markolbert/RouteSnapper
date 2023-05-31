#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// MainWindow.xaml.cs
//
// This file is part of JumpForJoy Software's RouteSnapper.
// 
// RouteSnapper is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// RouteSnapper is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with RouteSnapper. If not, see <https://www.gnu.org/licenses/>.
#endregion

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

        this.InitializeComponent();

        winSupport.SetSizeAndPosition();

        mapControl.FileSystemCachePath = Path.Combine( WinUIConfigBase.UserFolder, "map-cache" );
        mapControl.ValidCredentials += MapControlOnValidCredentials;

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
        }

        mapControl.Center = "37.5072N,122.2605W";

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

    private void MapControlOnValidCredentials( object? sender, ValidCredentialsEventArgs e )
    {
        if( _appConfig == null )
            return;

        _appConfig.MapViewModel.ProjectionName = e.ProjectionName;

        // no need to update if the initial attempt, based on the config file,
        // succeeded
        if( e.AttemptNumber == 0 )
            return;

        _appConfig.EngineViewModel.MapCredentials ??= new MapCredentials();

        switch( e.Credentials )
        {
            case BingCredentials bingCredentials:
                _appConfig.EngineViewModel.MapCredentials.BingCredentials = bingCredentials;
                break;

            case GoogleCredentials googleCredentials:
                _appConfig.EngineViewModel.MapCredentials.GoogleCredentials = googleCredentials;
                break;

            case OpenStreetCredentials openStreetCredentials:
                _appConfig.EngineViewModel.MapCredentials.OpenStreetCredentials = openStreetCredentials;
                break;

            case OpenTopoCredentials openTopoCredentials:
                _appConfig.EngineViewModel.MapCredentials.OpenTopoCredentials = openTopoCredentials;
                break;
        }
    }
}
