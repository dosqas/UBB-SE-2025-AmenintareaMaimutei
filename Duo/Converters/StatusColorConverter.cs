using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace Duo.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool status)
            {
                return new SolidColorBrush(status == true ? Microsoft.UI.Colors.Green : Microsoft.UI.Colors.Gray);
            }
            return new SolidColorBrush(Microsoft.UI.Colors.Gray); // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
