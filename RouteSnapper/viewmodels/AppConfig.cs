#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// AppConfig.cs
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

using J4JSoftware.WindowsUtilities;

namespace RouteSnapper;

internal class AppConfig : WinUIConfigBase
{
    public EngineViewModel EngineViewModel { get; set; } = new();
    public MapViewModel MapViewModel { get; set; } = new();
    public ExportViewModel ExportViewModel { get; set; } = new();
    public FiltersViewModel FiltersViewModel { get; set; } = new();
    public SourceFilesViewModel SourceFilesViewModel { get; set; } = new();
}