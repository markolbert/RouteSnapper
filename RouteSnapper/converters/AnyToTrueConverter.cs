using System;
using System.Collections;
using Microsoft.UI.Xaml.Data;

namespace RouteSnapper;

public class AnyToTrueConverter : IValueConverter
{
    public object Convert( object value, Type targetType, object parameter, string language )
    {
        if( value is not IList list )
            return false;

        return list.Count > 0;
    }

    public object ConvertBack( object value, Type targetType, object parameter, string language ) => throw new NotImplementedException();
}
