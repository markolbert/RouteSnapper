﻿#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// FileHandler.cs
//
// This file is part of JumpForJoy Software's GeoProcessor.
// 
// GeoProcessor is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// GeoProcessor is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with GeoProcessor. If not, see <https://www.gnu.org/licenses/>.
#endregion

using Microsoft.Extensions.Logging;

namespace J4JSoftware.GeoProcessor;

public class FileHandler
{
    protected FileHandler( 
        IGeoConfig config, 
        ILoggerFactory? loggerFactory = null 
    )
    {
        Configuration = config;

        Logger = loggerFactory?.CreateLogger( GetType() );
    }

    protected ILogger? Logger { get; }
    protected IGeoConfig Configuration { get; }
}