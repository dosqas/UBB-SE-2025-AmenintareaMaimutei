using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Project.Utils
{
    public class MessageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string message)
            {
                if (message.Contains("successfully"))
                {
                    return "Green";
                }
                else
                {
                    return "Red";
                }
            }
            return "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
