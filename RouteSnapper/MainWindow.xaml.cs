// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RouteSnapper;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow
{
    private readonly MainWindowSupport _winSupport;

    public MainWindow()
    {
        this.InitializeComponent();

        _winSupport = new MainWindowSupport(this, App.Current.AppSupport);
        _winSupport.SetMainWindowSizeAndPosition();
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        myButton.Content = "Clicked";
    }
}
