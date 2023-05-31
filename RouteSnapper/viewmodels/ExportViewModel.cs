#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// ExportViewModel.cs
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
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;

namespace RouteSnapper;

public class ExportViewModel : ObservableObject
{
    private string? _exportFolder;
    private StorageFolder? _storageFolder;
    private string? _fileNameStub;
    private bool _gpx;
    private bool _kml;
    private bool _kmz;

    public ExportViewModel()
    {
        SelectExportFolderCommandAsync = new AsyncRelayCommand( SelectExportFolderHandlerAsync );
    }

    public string? ExportFolder
    {
        get => _exportFolder;
        private set => SetProperty( ref _exportFolder, value );
    }

    public string? FileNameStub
    {
        get => _fileNameStub;

        set
        {
            value = Path.GetFileName( value );
            SetProperty( ref _fileNameStub, value );
        }
    }

    public bool ExportToGpx
    {
        get => _gpx;

        set
        {
            SetProperty( ref _gpx, value );
            OnPropertyChanged( nameof( ExportEnabled ) );
        }
    }

    public bool ExportToKml
    {
        get => _kml;

        set
        {
            SetProperty( ref _kml, value );
            OnPropertyChanged( nameof( ExportEnabled ) );
        }
    }

    public bool ExportToKmz
    {
        get => _kmz;

        set
        {
            SetProperty( ref _kmz, value );
            OnPropertyChanged( nameof( ExportEnabled ) );
        }
    }

    [JsonIgnore]
    public bool ExportEnabled => ExportToGpx || ExportToKml || ExportToKmz;

    [JsonIgnore]
    public AsyncRelayCommand SelectExportFolderCommandAsync { get; }

    private async Task SelectExportFolderHandlerAsync()
    {
        var picker = new FolderPicker { SuggestedStartLocation = PickerLocationId.Desktop };

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle( App.Current.MainWindow );
        WinRT.Interop.InitializeWithWindow.Initialize( picker, hWnd );

        picker.SuggestedStartLocation = PickerLocationId.Desktop;

        _storageFolder = await picker.PickSingleFolderAsync();
        ExportFolder = _storageFolder?.Path;
    }
}
