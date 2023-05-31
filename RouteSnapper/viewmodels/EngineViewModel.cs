#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// EngineViewModel.cs
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

using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.EncryptedConfiguration;
using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

public class EngineViewModel : ObservableObject
{
    private string? _selectedSnapper;

    public EngineViewModel()
    {
        Snappers = new List<string> { Constants.BingSnapper, Constants.GoogleSnapper };
    }

    public MapCredentials? MapCredentials { get; set; }

    [JsonIgnore]
    public List<string> Snappers { get; }

    public string? SelectedSnapper
    {
        get=> _selectedSnapper;
        set => SetProperty( ref _selectedSnapper, value );
    }
}
