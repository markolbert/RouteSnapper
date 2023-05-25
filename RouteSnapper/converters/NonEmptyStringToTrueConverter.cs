using System;
using Microsoft.UI.Xaml.Data;

namespace RouteSnapper;

public class NonEmptyStringToTrueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        if( value is not string text )
            return false;

        return !string.IsNullOrEmpty( text );
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
