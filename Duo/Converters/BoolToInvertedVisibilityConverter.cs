using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Duo.Converters
{
    public class BoolToInvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                return !b ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is string s)
            {
                return string.IsNullOrEmpty(s) ? Visibility.Collapsed : Visibility.Visible;
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(bool))
            {
                return value is Visibility v && v == Visibility.Collapsed;
            }
            else if (targetType == typeof(string))
            {
                return value is Visibility v && v == Visibility.Visible ? "True" : string.Empty;
            }
            
            return null;
        }
    }
} 