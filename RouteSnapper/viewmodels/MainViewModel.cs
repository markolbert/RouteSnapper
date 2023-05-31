#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// MainViewModel.cs
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