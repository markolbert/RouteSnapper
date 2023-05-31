#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// ConsolidatePointsViewModel.cs
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

using CommunityToolkit.Mvvm.ComponentModel;
using J4JSoftware.RouteSnapper;

namespace RouteSnapper;

public class ConsolidatePointsViewModel : ObservableObject
{
    private bool _apply;

    public bool Apply
    {
        get => _apply;
        set => SetProperty(ref _apply, value);
    }

    public DistanceViewModel MinimumPointGap { get; set; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMinimumPointGapMeters
    };

    public DistanceViewModel MaximumOverallGap { get; set; } = new()
    {
        DistanceUnit = UnitType.Meters, DistanceValue = GeoConstants.DefaultMaximumOverallGapMeters
    };
}
