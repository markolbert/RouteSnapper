#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// FiltersViewModel.cs
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

namespace RouteSnapper;

public class FiltersViewModel : ObservableObject
{
    private bool _removeGarmin;

    public ConsolidatePointsViewModel ConsolidatePointsViewModel { get; set; } = new();
    public ConsolidateBearingViewModel ConsolidateBearingViewModel { get; set; } = new();
    public MergeRoutesViewModel MergeRoutesViewModel { get; set; } = new();
    public RemoveClustersViewModel RemoveClustersViewModel { get; set; } = new();

    public bool RemoveGarminMessages
    {
        get => _removeGarmin;
        set => SetProperty( ref _removeGarmin, value );
    }
}
