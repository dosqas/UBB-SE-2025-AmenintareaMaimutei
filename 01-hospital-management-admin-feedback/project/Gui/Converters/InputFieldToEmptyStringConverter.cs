using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Project.Utils
{
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value is float floatValue && floatValue == 0) ||
                (value is int intValue && intValue == 0) ||
                (value is DateOnly dateValue && dateValue == DateOnly.MinValue) ||
                (value is TimeSpan timeValue && timeValue == TimeSpan.Zero))
            {
                return string.Empty;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                if (targetType == typeof(float) || targetType == typeof(int))
                {
                    return 0;
                }
                if (targetType == typeof(DateOnly))
                {
                    return DateOnly.MinValue;
                }
                if (targetType == typeof(TimeSpan))
                {
                    return TimeSpan.Zero;
                }
            }
            if (targetType == typeof(float) && float.TryParse(value.ToString(), out float floatResult))
            {
                return floatResult;
            }
            if (targetType == typeof(int) && int.TryParse(value.ToString(), out int intResult))
            {
                return intResult;
            }
            if (targetType == typeof(DateOnly) && DateOnly.TryParse(value.ToString(), out DateOnly dateResult))
            {
                return dateResult;
            }
            if (targetType == typeof(TimeSpan) && TimeSpan.TryParse(value.ToString(), out TimeSpan timeResult))
            {
                return timeResult;
            }
            return value;
        }
    }
}

