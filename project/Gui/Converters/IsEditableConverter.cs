using Microsoft.UI.Xaml.Data;
using System;

namespace Project.Utils
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolean)
            {
                return !boolean;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolean)
            {
                return !boolean;
            }
            return value;
        }
    }
}
