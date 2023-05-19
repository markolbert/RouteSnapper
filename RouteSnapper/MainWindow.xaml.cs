// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RouteSnapper;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow
{
    private readonly IDataProtector _protector;
    private readonly ILogger? _logger;
    private readonly AppConfig _appConfig;
    private readonly JsonSerializerOptions _jsonOptions;

    public MainWindow()
    {
        var loggerFactory = GetRequiredService<ILoggerFactory>();
        _protector = GetRequiredService<IDataProtector>();
        _appConfig = GetRequiredService<AppConfig>();

        _jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        _logger = loggerFactory.CreateLogger<MainWindow>();

        this.InitializeComponent();

        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        Task.Run(async () => await SizeWindow(appWindow));

        this.Closed += (_, _) => SaveConfiguration();
    }

    private async Task SizeWindow(AppWindow appWindow)
    {
        var displayList = await DeviceInformation.FindAllAsync(DisplayMonitor.GetDeviceSelector());

        if (!displayList.Any())
            return;

        var monitorInfo = await DisplayMonitor.FromInterfaceIdAsync(displayList[0].Id);

        var winSize = new SizeInt32();

        if (monitorInfo == null)
        {
            winSize.Width = 800;
            winSize.Height = 1200;
        }
        else
        {
            winSize.Height = monitorInfo.NativeResolutionInRawPixels.Height;
            winSize.Width = monitorInfo.NativeResolutionInRawPixels.Width;

            var widthInInches = Convert.ToInt32(8 * monitorInfo.RawDpiX);
            var heightInInches = Convert.ToInt32(12 * monitorInfo.RawDpiY);

            winSize.Height = winSize.Height > heightInInches ? heightInInches : winSize.Height;
            winSize.Width = winSize.Width > widthInInches ? widthInInches : winSize.Width;
        }

        appWindow.Resize(winSize);
    }

    private T GetRequiredService<T>()
        where T : class
    {
        var retVal = App.Current.Services.GetService<T>();
        if (retVal != null)
            return retVal;

        _logger?.LogCritical("{service} is not available", typeof(T));
        throw new ApplicationException($"Service {typeof(T)} is not available");
    }

    private void SaveConfiguration()
    {
        if (string.IsNullOrEmpty(_appConfig.UserConfigurationFilePath))
            return;

        var encrypted = _appConfig.Encrypt(_protector);

        try
        {
            var jsonText = JsonSerializer.Serialize(encrypted, _jsonOptions);

            File.WriteAllText(_appConfig.UserConfigurationFilePath!, jsonText);
        }
        catch (Exception ex)
        {
            _logger?.LogError("Failed to write configuration file, exception was '{exception}'", ex.Message);
        }
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        myButton.Content = "Clicked";
    }
}
