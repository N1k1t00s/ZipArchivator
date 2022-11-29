using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace P1_Nikita.Converter;

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
            return s == string.Empty ? Visibility.Visible : Visibility.Collapsed;
        return value == null ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}