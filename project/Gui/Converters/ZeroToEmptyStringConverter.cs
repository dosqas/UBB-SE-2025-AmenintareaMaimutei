using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Project.Utils
{
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value is float floatValue && floatValue == 0) || (value is int intValue && intValue == 0))
            {
                return string.Empty;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                return 0f;
            }
            if (float.TryParse(value.ToString(), out float result))
            {
                return result;
            }
            return 0f;
        }
    }
}

