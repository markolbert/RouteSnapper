using System;
using Microsoft.UI.Xaml.Data;

namespace RouteSnapper;

public class NullToFalseConverter : IValueConverter
{
    public object Convert( object? value, Type targetType, object parameter, string language ) => value != null;

    public object ConvertBack( object value, Type targetType, object parameter, string language ) => throw new NotImplementedException();
}