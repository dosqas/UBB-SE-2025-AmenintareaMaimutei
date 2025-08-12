using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utils
{
    class DateOnlyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateOnly date)
            {
                return date.ToString("yyyy-MM-dd"); // Format it as a string (you can adjust the format)
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && DateOnly.TryParse(str, out DateOnly date))
            {
                return date;
            }
            return new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day); // Default value if the string is not valid
        }
    }
}
