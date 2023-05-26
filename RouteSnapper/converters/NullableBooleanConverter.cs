using System;
using Microsoft.UI.Xaml.Data;

namespace RouteSnapper;

public class NullableBooleanConverter : IValueConverter
{
    public object Convert( object? value, Type targetType, object parameter, string language ) => value is bool and true;

    public object ConvertBack( object value, Type targetType, object parameter, string language ) => throw new NotImplementedException();
}