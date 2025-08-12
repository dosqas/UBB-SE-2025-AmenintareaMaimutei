using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utils
{
    class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan timeSpan)
                return timeSpan.ToString(@"hh\:mm");
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string timeString && TimeSpan.TryParse(timeString, out TimeSpan parsedTime))
                return parsedTime;
            return TimeSpan.Zero;
        }
    }
}
