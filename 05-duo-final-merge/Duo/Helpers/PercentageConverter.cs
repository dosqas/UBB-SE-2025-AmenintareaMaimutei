using Microsoft.UI.Xaml.Data;
using System;

namespace DuolingoNou.Helpers
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double percentage)
            {
                return (percentage * 100).ToString("F0") + "%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}